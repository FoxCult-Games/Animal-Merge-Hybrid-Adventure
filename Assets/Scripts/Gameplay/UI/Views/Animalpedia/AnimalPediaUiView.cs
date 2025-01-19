namespace FoxCultGames.Gameplay.UI.Views.AnimalPedia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameData;
    using Gameplay.Entities.Settings;
    using UnityEngine;
    using UnityEngine.Pool;
    using UnityEngine.UI;

    public class AnimalPediaUiView : UiViewBase
    {
        [SerializeField] private AnimalPediaEntryController entryPrefab;
        [SerializeField] private Transform entriesParent;
        [SerializeField] private Button animalPediaButton;
        [SerializeField] private Button closeButton;
        
        private readonly List<AnimalPediaEntryController> entries = new();
        private IObjectPool<AnimalPediaEntryController> entryPool;
        private Predicate<EntitySettings> currentFilter;
        private Action<EntitySettings> selectionCallback;
        
        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            entryPool = new ObjectPool<AnimalPediaEntryController>(CreateEntry, GetEntry, ReleaseEntry);
            gameContext.EntityManager.OnNewEntityUnlocked += EntityManager_OnNewEntityUnlocked;
            
            animalPediaButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);
        }

        public override void Open()
        {
            base.Open();

            var entities = GameData.EntitiesSettings.Values
                .Where(x => currentFilter == null || currentFilter.Invoke(x))
                .OrderByDescending(x => gameContext.GameSave.Entities.UnlockedEntities.Contains(x.Guid))
                .ToList();
            
            for (var i = 0; i < entities.Count; i++)
            {
                var entitySettings = entities[i];
                var entry = entryPool.Get();
                entry.transform.SetSiblingIndex(i);
                entry.SetEntityData(entitySettings);
                entries.Add(entry);
            }
        }

        public override void Close()
        {
            base.Close();
            
            foreach (var entry in entries)
                entryPool.Release(entry);
            
            currentFilter = null;
            entries.Clear();
        }

        private AnimalPediaEntryController CreateEntry()
        {
            var entry = Instantiate(entryPrefab, entriesParent);
            entry.Initialize(gameContext, x =>
            {
                if (selectionCallback != null)
                    selectionCallback.Invoke(x);
                else
                    gameContext.EnclosureManager.ChangeEnclosure(x.Guid);
                
                Close();
            });
            return entry;
        }

        private void EntityManager_OnNewEntityUnlocked(Guid entityId)
        {
            if (!IsOpen)
                return;
            
            var entry = entryPool.Get();
            entry.SetEntityData(GameData.EntitiesSettings[entityId]);
        }

        private static void GetEntry(AnimalPediaEntryController entry)
        {
            entry.gameObject.SetActive(true);
        }

        private static void ReleaseEntry(AnimalPediaEntryController entry)
        {
            entry.gameObject.SetActive(false);
        }

        public void Open(Predicate<EntitySettings> entityFilter, Action<EntitySettings> OnEntitySelected)
        {
            currentFilter = entityFilter;
            Open();
            selectionCallback = OnEntitySelected;
        }
    }
}