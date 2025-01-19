namespace FoxCultGames.Gameplay.UI.Views.Shop
{
    using Money.Currency;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopEntryController : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button buyButton;
        
        private IGameContext gameContext;
        private ShopEntryData data;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
            
            gameContext.MoneyManager.OnCurrencyChanged += MoneyManager_OnCurrencyChanged;
        }

        public void Setup(ShopEntryData data)
        {
            this.data = data;
            
            icon.sprite = data.icon;
            nameText.text = data.name;
            priceText.text = data.price.amount.ToString();
            
            buyButton.interactable = gameContext.MoneyManager.CanSpendMoney(data.price);
            buyButton.onClick.AddListener(TryBuyingEntry);
        }

        public void Cleanup()
        {
            buyButton.onClick.RemoveAllListeners();
        }

        private void TryBuyingEntry()
        {
            if (!gameContext.MoneyManager.CanSpendMoney(data.price))
                return;
            
            gameContext.MoneyManager.SpendMoney(data.price);
            data.onBoughtCallback?.Invoke();
        }

        private void MoneyManager_OnCurrencyChanged(CurrencyType currencyType)
        {
            if (data.price.currency != currencyType)
                return;
            
            buyButton.interactable = gameContext.MoneyManager.CanSpendMoney(data.price);
        }
    }
}