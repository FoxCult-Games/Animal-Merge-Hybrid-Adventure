namespace FoxCultGames.Gameplay.Enclosures
{
    using System;
    using System.Collections.Generic;
    using GameData;
    using UI.Views.Enclosures;
    using UnityEngine;

    public partial class EnclosureManager : MonoBehaviour, ISubManager, IEnclosureManager
    {
        public EnclosureController CurrentEnclosure => enclosures[currentEnclosureIndex];
        
        private readonly List<EnclosureController> enclosures = new();
        
        private IGameContext gameContext;
        private int currentEnclosureIndex;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
            
            swipingHelper.Initialize(gameContext, this);
            
            gameContext.EntityManager.OnNewEntityUnlocked += SpawnEnclosure;
        }

        private void OnDestroy()
        {
            swipingHelper.Dispose();
        }

        public void PostInitialize()
        {
            foreach (var entityId in gameContext.GameSave.Entities.UnlockedEntities)
                SpawnEnclosure(entityId);

            currentEnclosureIndex = Mathf.Clamp(enclosures.FindIndex(enclosure => enclosure.EntityId == gameContext.GameSave.DefaultEnclosure), 0, enclosures.Count - 1);
            enclosures[currentEnclosureIndex].ShowEntities();
        }

        public void UpdateManager()
        {
            swipingHelper.Update();
            foreach (var enclosure in enclosures)
                enclosure.Update();
        }

        public void SwipeEnclosure(int direction)
        {
            if (enclosures.Count <= 1)
                return;

            enclosures[currentEnclosureIndex].HideEntities();
            currentEnclosureIndex = (currentEnclosureIndex + direction + enclosures.Count) % enclosures.Count;
            enclosures[currentEnclosureIndex].ShowEntities();
        }

        public void SetCurrentAsDefault()
        {
            gameContext.GameSave.DefaultEnclosure = enclosures[currentEnclosureIndex].EntityId;
        }

        public void ChangeEnclosure(Guid entitySettingsGuid)
        {
            var index = enclosures.FindIndex(enclosure => enclosure.EntityId == entitySettingsGuid);
            if (index == -1)
                return;

            enclosures[currentEnclosureIndex].HideEntities();
            currentEnclosureIndex = index;
            enclosures[currentEnclosureIndex].ShowEntities();
        }

        private void SpawnEnclosure(Guid entityId)
        {
            gameContext.UiManager.GetView<EnclosuresUiView>().SpawnEnclosure(enclosure =>
            {
                var enclosureController = new EnclosureController(gameContext, GameData.EntitiesSettings[entityId], enclosure);
                enclosureController.HideEntities();
                enclosures.Add(enclosureController);
            });
        }
    }
}