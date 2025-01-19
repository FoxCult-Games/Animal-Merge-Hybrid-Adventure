namespace FoxCultGames.Gameplay.UI.Views.HUD
{
    using System;
    using Cysharp.Text;
    using Money.Currency;
    using TMPro;
    using UnityEngine;

    public class HudUiView : UiViewBase
    {
        [SerializeField] private TextMeshProUGUI gemsText;
        [SerializeField] private TextMeshProUGUI coinsText;

        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            RefreshCurrency(CurrencyType.Coins);
            RefreshCurrency(CurrencyType.Gems);
            
            gameContext.MoneyManager.OnCurrencyChanged += RefreshCurrency;
        }
        
        private void RefreshCurrency(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Coins:
                    coinsText.text = ZString.Format("{0}",gameContext.MoneyManager.GetCurrencyAmount(CurrencyType.Coins));
                    break;
                case CurrencyType.Gems:
                    gemsText.text = ZString.Format("{0}", gameContext.MoneyManager.GetCurrencyAmount(CurrencyType.Gems));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }
    }
}