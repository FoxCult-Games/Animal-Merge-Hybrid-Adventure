namespace FoxCultGames.Gameplay.Loot
{
    using Money.Currency;

    public interface IBuyAvailableLoot
    {
        public CurrencyType CurrencyType { get; }
        public int Price { get; }
    }
}