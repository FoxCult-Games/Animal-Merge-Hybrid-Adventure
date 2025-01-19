namespace FoxCultGames.Gameplay.Loot.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Settings;
    using GameData;
    using Money;
    using Money.Currency;
    using UnityEngine;
    using UnityEngine.Pool;

    [Serializable]
    public class EntityLootResolver : AvailableLootResolver
    {
        [Serializable]
        public struct EntityLootEntry
        {
            public EntityTier tier;
            public float chance;
        }
        
        [SerializeField] private List<EntityLootEntry> entries;
        
        private const float ValueReturnMultiplier = 0.8f;
        
        public override Sprite Resolve(IGameContext gameContext, LootSettings lootSettings)
        {
            DictionaryPool<EntityTier, List<Guid>>.Get(out var entitiesByTier);
            foreach (var entitySettings in GameData.EntitiesSettings.Values)
            {
                if (!entitiesByTier.ContainsKey(entitySettings.EntityTier))
                    entitiesByTier.Add(entitySettings.EntityTier, new List<Guid>());
                
                entitiesByTier[entitySettings.EntityTier].Add(entitySettings.Guid);
            }
            
            var rolledChance = UnityEngine.Random.value;
            var tierToUnlock = entries.First().tier;
            for (var i = entries.Count - 1; i >= 0; i--)
            {
                var entry = entries[i];
                if (rolledChance > entry.chance)
                    continue;

                tierToUnlock = entry.tier;
                break;
            }

            var entitiesToUnlock = entitiesByTier[tierToUnlock];
            var entityToUnlock = entitiesToUnlock[UnityEngine.Random.Range(0, entitiesToUnlock.Count)];
            if (!gameContext.EntityManager.CheckIfHasNewEntityUnlocked(GameData.EntitiesSettings[entityToUnlock]) && lootSettings is ChestLootSettings chestLootSettings)
            {
                gameContext.MoneyManager.AddMoney(new MoneyEntry
                {
                    currency = CurrencyType.Coins,
                    amount = Mathf.FloorToInt(chestLootSettings.Price * ValueReturnMultiplier)
                });
            }
            
            return GameData.EntitiesSettings[entityToUnlock].Sprite;
        }
    }
}