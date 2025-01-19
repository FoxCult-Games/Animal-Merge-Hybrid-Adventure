namespace FoxCultGames.Gameplay.Entities
{
    using System;
    using System.Collections.Generic;
    using Core.Entities;
    using Money;
    using Money.Currency;
    using Settings;
    using UnityEngine;
    using Utilities;

    public class EntityManager : MonoBehaviour, ISubManager, IEntityManager
    {
        [SerializeField] private List<EntitySettings> initialEntitySettings;
        [SerializeField] private int entitiesAtStart = 2;
        [SerializeField] private float payoutIntervalInHours = 0.5f;

        public event Action<Guid> OnNewEntityUnlocked;

        private IGameContext gameContext;
        private EntitiesDataModel dataModel;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
            dataModel = gameContext.GameSave.Entities;
        }

        public void PostInitialize()
        {
            if (!gameContext.IsNewGame)
            {
                var hoursSinceLastPayout = (DateTime.Now - dataModel.LastPayoutTime).TotalHours;
                var payoutCount = Mathf.FloorToInt((float)hoursSinceLastPayout / payoutIntervalInHours);
                gameContext.MoneyManager.AddMoney(new MoneyEntry
                {
                    currency = CurrencyType.Coins,
                    amount = dataModel.UnlockedEntities.Count * payoutCount
                });
                dataModel.LastPayoutTime = dataModel.LastPayoutTime.AddHours(payoutCount * payoutIntervalInHours);
                return;
            }
            
            dataModel.LastPayoutTime = DateTime.Now;

            var availableEntities = new List<EntitySettings>(initialEntitySettings);
            for (var i = 0; i < entitiesAtStart; i++)
            {
                dataModel.UnlockedEntities.Add(availableEntities.RandomItem().Guid);
            }
        }

        public void UpdateManager()
        {
            var hoursSinceLastPayout = (DateTime.Now - dataModel.LastPayoutTime).TotalHours;
            var payoutCount = Mathf.FloorToInt((float)hoursSinceLastPayout / payoutIntervalInHours);
            if (payoutCount <= 0)
                return;

            gameContext.MoneyManager.AddMoney(new MoneyEntry
            {
                currency = CurrencyType.Coins,
                amount = dataModel.UnlockedEntities.Count * payoutCount
            });
            dataModel.LastPayoutTime = dataModel.LastPayoutTime.AddHours(payoutCount * payoutIntervalInHours);
        }

        public bool CheckIfHasNewEntityUnlocked(EntitySettings entitySettings)
        {
            if (!dataModel.UnlockedEntities.Add(entitySettings.Guid))
                return false;

            Debug.Log($"New entity unlocked: {entitySettings.EntityName}");
            OnNewEntityUnlocked?.Invoke(entitySettings.Guid);
            return true;
        }
    }
}