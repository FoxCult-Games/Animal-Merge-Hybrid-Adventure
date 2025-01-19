namespace FoxCultGames.Gameplay.UI.Views
{
    using UnityEngine;

    public abstract class UiViewBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool isOpenByDefault;
        
        public bool IsOpen { get; private set; }

        protected IGameContext gameContext;
        
        public virtual void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
            
            if (isOpenByDefault)
                SetVisible();
            else
                SetInvisible();
        }
        
        public virtual void UpdateView() { }
        
        public virtual void Open()
        {
            IsOpen = true;
            SetVisible();
        }
        
        public virtual void Close()
        {
            IsOpen = false;
            SetInvisible();
        }
        
        protected void SetVisible()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        
        protected void SetInvisible()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}