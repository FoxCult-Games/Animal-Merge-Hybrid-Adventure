namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System;
    using Core.Entities.SubControllers;
    using UnityEngine;

    public abstract class EntitySubController : MonoBehaviour, IEntitySubController
    {
        protected IGameContext gameContext;
        
        void IEntitySubController.Initialize(IGameContext gameContext, EntityController entityController, SubControllerDataModel dataModel)
        {
            this.gameContext = gameContext;
            
            if (this is ISerializableSubController serializableSubController)
            {
                if (dataModel != null)
                {
                    serializableSubController.SetDataModel(dataModel);
                }
                else
                {
                    Debug.LogError($"[{GetType().Name}] Data model is null");
                }
            }
            
            Initialize(gameContext, entityController);
        }

        void IEntitySubController.UpdateController()
        {
            UpdateController();
        }

        protected abstract void Initialize(IGameContext gameContext, EntityController entityController);
        protected abstract void UpdateController();
    }

    public abstract class EntitySubController<TSettings> : EntitySubController, ISettingsSubController where TSettings : EntitySubControllerSettings
    {
        public Type SettingsType => typeof(TSettings);

        protected TSettings Settings { get; private set; }
        
        void IEntitySubController.Initialize(IGameContext gameContext, EntityController entityController, SubControllerDataModel dataModel)
        {
            this.gameContext = gameContext;
            Initialize(gameContext, entityController);
        }
        
        public void AssignSettings(EntitySubControllerSettings settings)
        {
            Settings = (TSettings)settings;
        }
    }

    public abstract class SerializableEntitySubController<TDataModel> : EntitySubController, ISerializableSubController where TDataModel : SubControllerDataModel
    {
        public Type DataModelType => typeof(TDataModel);
        public TDataModel DataModel { get; private set; }

        SubControllerDataModel ISerializableSubController.CreateDataModel()
        {
            return CreateDataModel();
        }

        void ISerializableSubController.SetDataModel(SubControllerDataModel dataModel)
        {
            DataModel = (TDataModel)dataModel;
        }

        protected abstract TDataModel CreateDataModel();
    }
    
    public abstract class SerializableEntitySubController<TSettings, TDataModel> : EntitySubController, ISettingsSubController, ISerializableSubController where TSettings : EntitySubControllerSettings where TDataModel : SubControllerDataModel
    {
        public Type SettingsType => typeof(TSettings);

        public Type DataModelType => typeof(TDataModel);
        
        protected TDataModel DataModel { get; private set; }
        protected TSettings Settings { get; private set; }

        SubControllerDataModel ISerializableSubController.CreateDataModel()
        {
            return CreateDataModel();
        }

        void ISerializableSubController.SetDataModel(SubControllerDataModel dataModel)
        {
            DataModel = (TDataModel)dataModel;
        }
        
        void IEntitySubController.Initialize(IGameContext gameContext, EntityController entityController, SubControllerDataModel dataModel)
        {
            this.gameContext = gameContext;
            
            if (this is ISerializableSubController serializableSubController)
            {
                if (dataModel != null)
                {
                    serializableSubController.SetDataModel(dataModel);
                }
                else
                {
                    Debug.LogError($"[{GetType().Name}] Data model is null");
                }
            }
            
            Initialize(gameContext, entityController);
        }
        
        public void AssignSettings(EntitySubControllerSettings settings)
        {
            Settings = (TSettings)settings;
        }

        protected abstract TDataModel CreateDataModel();
    }
}