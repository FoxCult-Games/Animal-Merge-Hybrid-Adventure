namespace FoxCultGames.Gameplay.Money
{
    using System;
    using Currency;

    [Serializable]
    public struct MoneyEntry
    {
        public CurrencyType currency;
        public int amount;
    }
}