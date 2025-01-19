namespace FoxCultGames.Gameplay.Money.Currency
{
    public class GemCurrencyController : ICurrencyController
    {
        public int Amount => gameContext.GameSave.Money.Currencies[Type];
        public CurrencyType Type => CurrencyType.Gems;

        private readonly IGameContext gameContext;
        
        public GemCurrencyController(IGameContext gameContext)
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