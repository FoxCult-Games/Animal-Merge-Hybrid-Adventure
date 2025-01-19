namespace FoxCultGames.Gameplay.Loot
{
    using System;

    public interface ILootManager
    {
        event Action<LootSettings> OnNewLootUnlocked;
        void BuyLoot(ChestLootSettings chest);
    }
}