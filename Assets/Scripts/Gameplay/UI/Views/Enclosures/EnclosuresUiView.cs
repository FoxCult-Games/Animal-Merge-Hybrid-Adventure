namespace FoxCultGames.Gameplay.UI.Views.Enclosures
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    public class EnclosuresUiView : UiViewBase
    {
        [SerializeField] private RectTransform enclosuresParent;
        [SerializeField] private RectTransform enclosurePrefab;
        [SerializeField] private Button setAsDefaultButton;

        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            setAsDefaultButton.onClick.AddListener(() => gameContext.EnclosureManager.SetCurrentAsDefault());
        }

        public void SpawnEnclosure(Action<RectTransform> onDoneCallback)
        {
            var enclosure = Instantiate(enclosurePrefab, enclosuresParent);
            enclosure.GetComponent<Image>().color = Random.ColorHSV();
            onDoneCallback?.Invoke(enclosure); 
        }
    }
}