namespace FoxCultGames.Gameplay.UI.Views.AnimalPedia
{
    using System;
    using Gameplay.Entities.Settings;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimalPediaEntryController : MonoBehaviour
    {
        [SerializeField] private Image animalImage;
        [SerializeField] private Button goToEnclosureButton;
        [SerializeField] private Sprite lockedSprite;

        private IGameContext gameContext;
        private EntitySettings entitySettings;

        public void Initialize(IGameContext gameContext, Action<EntitySettings> onClickCallback)
        {
            this.gameContext = gameContext;
            goToEnclosureButton.onClick.AddListener(() => onClickCallback?.Invoke(entitySettings));
        }
        
        public void SetEntityData(EntitySettings entitySettings)
        {
            this.entitySettings = entitySettings;
            var isUnlocked = gameContext.GameSave.Entities.UnlockedEntities.Contains(entitySettings.Guid);
            goToEnclosureButton.interactable = isUnlocked;
            animalImage.sprite = isUnlocked ? entitySettings.Sprite : lockedSprite;
        }
    }
}