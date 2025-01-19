namespace FoxCultGames.Gameplay.Money.Currency
{
    public interface ICurrencyController
    {
        int Amount { get; }
        CurrencyType Type { get; }
        
        void AddAmount(int amount);
        void SpendAmount(int amount);
        bool CanSpendAmount(int amount);
    }
}