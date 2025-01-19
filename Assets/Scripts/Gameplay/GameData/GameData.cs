namespace FoxCultGames.Gameplay.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Settings;
    using Loot;
    using UnityEngine;

    public static partial class GameData
    {
        public static Dictionary<Guid, EntitySettings> EntitiesSettings { get; private set; }
        public static Dictionary<Guid, ChestLootSettings> LootSettings { get; private set; }
        
        public static EntitySettings GetNextEntitySettings(Guid firstEntityId, Guid secondEntityId)
        {
            foreach (var settings in EntitiesSettings.Values)
            {
                if (IsProperEntity(settings))
                    return settings;
            }
            
            Debug.Log($"[GameData] No next entity settings found for {EntitiesSettings[firstEntityId].EntityName} and {EntitiesSettings[secondEntityId].EntityName}");
            return null;

            bool IsProperEntity(EntitySettings settings)
            {
                return settings.RequiredEntitySettings.Contains(EntitiesSettings[firstEntityId]) && settings.RequiredEntitySettings.Contains(EntitiesSettings[secondEntityId]);
            }
        }
    }
}