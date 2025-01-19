namespace FoxCultGames.Gameplay.Loot
{
    using System;
    using UnityEngine;

    public class LootManager : MonoBehaviour, ISubManager, ILootManager
    {
        [SerializeField] private LootSettings testingLoot;
        
        public event Action<LootSettings> OnNewLootUnlocked;

        public void Initialize(IGameContext gameContext)
        {
            
        }

        public void PostInitialize()
        {
            
        }

        public void UpdateManager()
        {
            
        }
        
        public void BuyLoot(ChestLootSettings chest)
        {
            OnNewLootUnlocked?.Invoke(chest);
        }
    }
}