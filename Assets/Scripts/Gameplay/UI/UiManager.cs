namespace FoxCultGames.Gameplay.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Views;

    public class UiManager : MonoBehaviour, ISubManager, IUiManager
    {
        [SerializeField] private GameObject canvas;
        
        private IGameContext gameContext;

        private List<UiViewBase> views;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;

            views = GetComponentsInChildren<UiViewBase>().ToList();
        }

        public void PostInitialize()
        {
            canvas.SetActive(true);
            foreach (var view in views)
            {
                view.Initialize(gameContext);
            }
        }

        public void UpdateManager()
        {
            foreach (var view in views)
            {
                view.UpdateView();
            }
        }

        public T GetView<T>() where T : UiViewBase
        {
            return views.OfType<T>().FirstOrDefault();
        }
    }
}