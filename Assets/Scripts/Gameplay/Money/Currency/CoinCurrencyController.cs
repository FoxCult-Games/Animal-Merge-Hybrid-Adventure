namespace FoxCultGames.Gameplay.Money.Currency
{
    public class CoinCurrencyController : ICurrencyController
    {
        public int Amount => gameContext.GameSave.Money.Currencies[Type];
        public CurrencyType Type => CurrencyType.Coins;

        private readonly IGameContext gameContext;
        
        public CoinCurrencyController(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }
        
        public void AddAmount(int amount)
        {
            gameContext.GameSave.Money.Currencies[Type] += amount;
        }

        public void SpendAmount(int amount)
        {
            gameContext.GameSave.Money.Currencies[Type] -= amount;
        }

        public bool CanSpendAmount(int amount)
        {
            return Amount >= amount;
        }
    }
}