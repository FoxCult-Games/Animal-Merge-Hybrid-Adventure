namespace FoxCultGames.Gameplay.Money
{
    using System;
    using Currency;

    public interface IMoneyManager
    {
        event Action<CurrencyType> OnCurrencyChanged; 
        
        public int GetCurrencyAmount(CurrencyType currencyType);
        public void AddMoney(MoneyEntry args);
        public void SpendMoney(MoneyEntry args);
        public bool CanSpendMoney(MoneyEntry args);
    }
}