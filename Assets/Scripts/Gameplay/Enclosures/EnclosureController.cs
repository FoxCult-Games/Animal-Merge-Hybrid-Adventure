namespace FoxCultGames.Gameplay.Enclosures
{
    using System;
    using System.Collections.Generic;
    using Core.Entities;
    using Entities;
    using Entities.Settings;
    using UnityEngine;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    public class EnclosureController
    {
        public RectTransform Transform { get; }
        public Guid EntityId { get; }
        
        private readonly List<EntityController> entities = new();

        private readonly IGameContext gameContext;
        private readonly EntityController entityController;
        
        public EnclosureController(IGameContext gameContext, EntitySettings entitySettings, RectTransform transform)
        {
            this.gameContext = gameContext;
            EntityId = entitySettings.Guid;
            Transform = transform;
            
            entityController = SpawnEntity(new EntityDataModel(entitySettings.Guid));
        }

        public void Update()
        {
            foreach (var entity in entities)
                entity.UpdateController();
        }

        public void ShowEntities()
        {
            entityController.gameObject.SetActive(true);
            Transform.gameObject.SetActive(true);
        }

        public void HideEntities()
        {
            entityController.gameObject.SetActive(false);
            Transform.gameObject.SetActive(false);
        }

        private EntityController SpawnEntity(EntityDataModel entityDataModel)
        {
            var entitySettings = GameData.GameData.EntitiesSettings[entityDataModel.SettingsId];
            
            var entity = Object.Instantiate(entitySettings.Prefab, Transform).GetComponent<EntityController>();
            ((RectTransform)entity.transform).anchoredPosition = GetRandomPointInsideBounds();
            entity.Initialize(gameContext, entityDataModel); 
            
            entities.Add(entity);
            
            gameContext.EntityManager.CheckIfHasNewEntityUnlocked(entitySettings);

            return entity;
            
            Vector3 GetRandomPointInsideBounds()
            {
                var rect = Transform.rect;
                var randomX = Random.Range(rect.min.x, rect.max.x);
                var randomY = Random.Range(rect.min.y, rect.max.y);
                return new Vector3(randomX, randomY, 0f);
            }
        }
    }
}