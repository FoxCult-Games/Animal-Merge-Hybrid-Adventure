namespace FoxCultGames.Gameplay.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Entities;
    using Core.Entities.SubControllers;
    using Settings;
    using SubControllers;
    using UnityEngine;
    using UnityEngine.UI;

    public class EntityController : MonoBehaviour, IDraggable
    {
        [SerializeField] private EntitySettings entitySettings;
        [Space, SerializeField] private Image entityImage; 
        
        public event Action OnStartedDragging;
        public event Action OnStoppedDragging;
        
        public Guid EntityId => dataModel.SettingsId;
        public EntitySettings EntitySettings => entitySettings;
        
        private readonly List<IEntitySubController> subControllers = new();

        private IGameContext gameContext;
        private EntityDataModel dataModel;

        public void Initialize(IGameContext gameContext, EntityDataModel dataModel)
        {
            this.gameContext = gameContext;
            this.dataModel = dataModel;
            
            entityImage.sprite = entitySettings.Sprite;
            
            subControllers.AddRange(GetComponentsInChildren<IEntitySubController>());
            var uninitializedSubControllers = subControllers.ToList();
            
            ReinitializeSubControllers(dataModel.SubControllers);

            foreach (var subController in uninitializedSubControllers)
            {
                if (subController is ISettingsSubController settingsSubController)
                    settingsSubController.AssignSettings(GetControllerSettings(settingsSubController.SettingsType));
                
                if (subController is ISerializableSubController serializableSubController)
                {
                    var controllerDataModel = serializableSubController.CreateDataModel();
                    serializableSubController.Initialize(gameContext, this, controllerDataModel);
                    dataModel.SubControllers.Add(controllerDataModel);
                    continue;
                }
                
                subController.Initialize(gameContext, this, null);
            }

            void ReinitializeSubControllers(IReadOnlyCollection<SubControllerDataModel> controllerDataModels)
            {
                var controllersByDataModelType = subControllers.OfType<ISerializableSubController>().ToDictionary(x => x.DataModelType);
                
                foreach (var controllerDataModel in controllerDataModels)
                {
                    if (!controllersByDataModelType.TryGetValue(controllerDataModel.GetType(), out var controller))
                        continue;
                    
                    controller.Initialize(gameContext, this, controllerDataModel);
                    uninitializedSubControllers.Remove(controller);
                }
            }
        }

        public void UpdateController()
        {
            foreach (var subController in subControllers)
            {
                subController.UpdateController();
            }
        }

        public TSettings GetControllerSettings<TSettings>() where TSettings : EntitySubControllerSettings
        {
            foreach (var settings in EntitySettings.SubControllersSettings)
            {
                if (settings.GetType() == typeof(TSettings))
                    return (TSettings)settings;
            }
            
            Debug.LogError($"[{EntitySettings.EntityName}] No settings found for {typeof(TSettings).Name}");
            return null;
        }
        
        public EntitySubControllerSettings GetControllerSettings(Type settingsType)
        {
            foreach (var settings in EntitySettings.SubControllersSettings)
            {
                if (settings.GetType() == settingsType)
                    return settings;
            }
            
            Debug.LogError($"[{EntitySettings.EntityName}] No settings found for {settingsType.Name}");
            return null;
        }

        public void StartDragging()
        {
            OnStartedDragging?.Invoke();
        }

        public void UpdateDragging(Vector2 screenPosition)
        {
            var worldPosition = gameContext.CameraManager.Camera.ScreenToWorldPoint(screenPosition);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);
        }

        public void StopDragging()
        {
            OnStoppedDragging?.Invoke();
        }
    }
}