namespace FoxCultGames.Gameplay.Money
{
    using System;
    using System.Collections.Generic;
    using Currency;
    using UnityEngine;

    public class MoneyManager : MonoBehaviour, ISubManager, IMoneyManager
    {
        [SerializeField] private List<MoneyEntry> startingMoneyEntries = new(MoneyEntriesPreset);
        
        public event Action<CurrencyType> OnCurrencyChanged;
        
        private readonly Dictionary<CurrencyType, ICurrencyController> currencyControllers = new();
        
        private static readonly List<MoneyEntry> MoneyEntriesPreset = new()
        {
            new MoneyEntry
            {
                currency = CurrencyType.Coins,
                amount = 0
            },
            new MoneyEntry
            {
                currency = CurrencyType.Gems,
                amount = 0
            }
        };
        
        public void Initialize(IGameContext gameContext)
        {
            currencyControllers.Add(CurrencyType.Coins, new CoinCurrencyController(gameContext));
            currencyControllers.Add(CurrencyType.Gems, new GemCurrencyController(gameContext));
            
            if (!gameContext.IsNewGame)
                return;

            foreach (var moneyEntry in startingMoneyEntries)
                AddMoney(moneyEntry);
        }

        public void PostInitialize()
        {
            
        }

        public void UpdateManager()
        {
            
        }

        public int GetCurrencyAmount(CurrencyType currencyType)
        {
            return currencyControllers[currencyType].Amount;
        }

        public void AddMoney(MoneyEntry args)
        {
            currencyControllers[args.currency].AddAmount(args.amount);
            OnCurrencyChanged?.Invoke(args.currency);
        }

        public void SpendMoney(MoneyEntry args)
        {
            currencyControllers[args.currency].SpendAmount(args.amount);
            OnCurrencyChanged?.Invoke(args.currency);
        }

        public bool CanSpendMoney(MoneyEntry args)
        {
            return currencyControllers[args.currency].CanSpendAmount(args.amount);
        }
    }
}