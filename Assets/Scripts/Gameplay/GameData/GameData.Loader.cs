namespace FoxCultGames.Gameplay.GameData
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities.Settings;
    using Loot;
    using UnityEngine.AddressableAssets;

    public static partial class GameData
    {
        public static async Task LoadAssets()
        {
            await LoadEntitiesSettings();
            await LoadLootSettings();
        }

        private static async Task LoadEntitiesSettings()
        {
            var entitiesSettings = await Addressables.LoadAssetsAsync<EntitySettings>(EntitiesSettingsKey, null).Task;
            EntitiesSettings = entitiesSettings.ToDictionary(x => x.Guid);
        }

        private static async Task LoadLootSettings()
        {
            var lootSettings = await Addressables.LoadAssetsAsync<LootSettings>(LootSettingsKey, null).Task;
            LootSettings = lootSettings.OfType<ChestLootSettings>().ToDictionary(x => x.Guid);
        }
    }
}