namespace FoxCultGames.Core.Money
{
    using System;
    using System.Collections.Generic;
    using Gameplay.Money.Currency;

    [Serializable]
    public class MoneyDataModel 
    {
        public Dictionary<CurrencyType, int> Currencies { get; set; } = new()
        {
            {CurrencyType.Coins, 0},
            {CurrencyType.Gems, 0}
        };
    }
}