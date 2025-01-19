namespace FoxCultGames.Gameplay.Loot
{
    using Money.Currency;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Loot/Chest Loot Settings")]
    public class ChestLootSettings : LootSettings, IBuyAvailableLoot
    {
        [SerializeField] private Sprite openedIcon;
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private int price;
        [SerializeField] private bool canBeBought;
        
        public Sprite OpenedIcon => openedIcon;
        public CurrencyType CurrencyType => currencyType;
        public int Price => price;
        public bool CanBeBought => canBeBought;
    }
}