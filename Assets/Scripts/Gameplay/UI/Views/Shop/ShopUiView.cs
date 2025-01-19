namespace FoxCultGames.Gameplay.UI.Views.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameData;
    using Gameplay.Entities.Settings;
    using Money;
    using PrimeTween;
    using UnityEngine;
    using UnityEngine.Pool;
    using UnityEngine.UI;

    public struct ShopEntryData
    {
        public Sprite icon;
        public string name;
        public MoneyEntry price;
        public Action onBoughtCallback;
    }
    
    public class ShopUiView : UiViewBase
    {
        [SerializeField] private ShopEntryController shopEntryPrefab;
        [SerializeField] private Transform shopEntriesParent;
        [SerializeField] private Transform shopPanel;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button closeButton;
        
        [Header("Animations settings")]
        [SerializeField] private float openDuration = 0.4f;
        [SerializeField] private float closeDuration = 0.3f;

        private readonly List<ShopEntryController> shopEntries = new();
        
        private ObjectPool<ShopEntryController> shopEntriesPool;

        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            shopEntriesPool = new ObjectPool<ShopEntryController>(CreateShopEntry, GetShopEntry, ReleaseShopEntry);
            
            shopButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);
        }

        public override void Open()
        {
            base.Open();
            Tween.Scale(shopPanel, Vector3.zero, Vector3.one, openDuration, ease: Ease.OutBack);

            ShowAvailableChests();
            ShowEntities();
        }

        public override void Close()
        {
            Tween.Scale(shopPanel, Vector3.zero, closeDuration, ease: Ease.InBack).OnComplete(() =>
            {
                base.Close();
                
                for (var i = shopEntries.Count - 1; i >= 0; i--)
                {
                    shopEntriesPool.Release(shopEntries[i]);
                }
            });
        }
        
        private void RefreshEntries()
        {
            for (var i = shopEntries.Count - 1; i >= 0; i--)
            {
                shopEntriesPool.Release(shopEntries[i]);
            }
            
            ShowAvailableChests();
            ShowEntities();
        }

        private ShopEntryController CreateShopEntry()
        {
            var entry = Instantiate(shopEntryPrefab, shopEntriesParent);
            entry.Initialize(gameContext);
            return entry;
        }
        
        private void ShowAvailableChests()
        {
            var availableChests = GameData.LootSettings.Values.OrderBy(x => x.Price);
            foreach (var chest in availableChests)
            {
                if (!chest.CanBeBought)
                    continue;
                
                var entry = shopEntriesPool.Get();
                entry.Setup(new ShopEntryData
                {
                    icon = chest.Icon,
                    name = chest.name,
                    price = new MoneyEntry
                    {
                        currency = chest.CurrencyType,
                        amount = chest.Price
                    },
                    onBoughtCallback = () =>
                    {
                        gameContext.LootManager.BuyLoot(chest);
                        RefreshEntries();
                    }
                });
            }
        }

        private void ShowEntities()
        {
            var entitiesSettings = GameData.EntitiesSettings.Values.OrderBy(x => x.Price.amount);
            
            foreach (var entitySettings in entitiesSettings)
            {
                if (entitySettings.EntityType != EntityType.Normal || gameContext.GameSave.Entities.UnlockedEntities.Contains(entitySettings.Guid))
                    continue;

                shopEntriesPool.Get().Setup(new ShopEntryData
                {
                    icon = entitySettings.Sprite,
                    name = entitySettings.EntityName, 
                    price = entitySettings.Price, 
                    onBoughtCallback = () =>
                    {
                        gameContext.EntityManager.CheckIfHasNewEntityUnlocked(entitySettings);
                        RefreshEntries();
                    }
                });
            }
        }

        private void GetShopEntry(ShopEntryController entry)
        {
            shopEntries.Add(entry);
            entry.gameObject.SetActive(true);
        }

        private void ReleaseShopEntry(ShopEntryController entry)
        {
            shopEntries.Remove(entry);
            
            entry.Cleanup();
            entry.gameObject.SetActive(false);
        }
    }
}