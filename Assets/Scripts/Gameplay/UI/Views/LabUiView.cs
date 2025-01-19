namespace FoxCultGames.Gameplay.UI.Views
{
    using System;
    using AnimalPedia;
    using GameData;
    using Gameplay.Entities.Settings;
    using PrimeTween;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LabUiView : UiViewBase
    {
        [Serializable]
        private class EntityEntry
        {
            public Button button;
            public Image image;
            public EntitySettings EntitySettings { get; set; }
        }

        [SerializeField] private Button labButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private EntityEntry firstEntityEntry;
        [SerializeField] private EntityEntry secondEntityEntry;
        [SerializeField] private Button combineButton;
        [SerializeField] private TextMeshProUGUI combineButtonText;
        [SerializeField] private Sprite noEntitySprite;

        private EntitySettings firstEntity;
        private EntitySettings secondEntity;
        
        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            labButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);
            
            combineButton.interactable = false;
            combineButton.onClick.AddListener(CombineEntities);

            var animalPedia = gameContext.UiManager.GetView<AnimalPediaUiView>();
            firstEntityEntry.button.onClick.AddListener(() => animalPedia.Open(EntityFilter, x =>
            {
                firstEntity = x;
                SetEntityEntry(firstEntityEntry, x);
            }));
            secondEntityEntry.button.onClick.AddListener(() => animalPedia.Open(EntityFilter, x =>
            {
                secondEntity = x;
                SetEntityEntry(secondEntityEntry, x);
            }));
            
            PlayAnimation(firstEntityEntry.image.transform as RectTransform);
            PlayAnimation(secondEntityEntry.image.transform as RectTransform);

            bool EntityFilter(EntitySettings entitySettings)
            {
                return entitySettings.EntityType == EntityType.Normal && entitySettings != firstEntity && entitySettings != secondEntity;
            }

            void PlayAnimation(RectTransform rectTransform)
            {
                var initialPositionY = rectTransform.anchoredPosition.y;
                Tween.UIAnchoredPositionY(rectTransform, initialPositionY + 12f, 1.2f, Ease.InOutQuad, cycles: -1, CycleMode.Yoyo);
            }
        }

        public override void Open()
        {
            base.Open();

            SetEntityEntry(firstEntityEntry, null);
            SetEntityEntry(secondEntityEntry, null);
            
            firstEntity = null;
            secondEntity = null;
        }

        private void SetEntityEntry(EntityEntry slotToSet, EntitySettings selectedEntity)
        {
            slotToSet.EntitySettings = selectedEntity;
            slotToSet.image.sprite = selectedEntity?.Sprite ?? noEntitySprite;
            
            RefreshMergeButton();
        }

        private void RefreshMergeButton()
        {
            var hasEntitiesSelected = firstEntityEntry.EntitySettings != null && secondEntityEntry.EntitySettings != null;
            if (hasEntitiesSelected)
            {
                var entityToUnlock = GameData.GetNextEntitySettings(firstEntity.Guid, secondEntity.Guid);
                if (entityToUnlock == null)
                {
                    combineButton.interactable = false;
                    combineButtonText.text = "Not available";
                    return;
                }
                
                if (gameContext.GameSave.Entities.UnlockedEntities.Contains(entityToUnlock.Guid))
                {
                    combineButton.interactable = false;
                    combineButtonText.text = "Already unlocked";
                }
                else
                {
                    combineButton.interactable = true;
                    combineButtonText.text = "Merge";
                }
            }
            else
            {
                combineButton.interactable = false;
                combineButtonText.text = "Merge";
            }
        }
        
        private void CombineEntities()
        {
            gameContext.EntityManager.CheckIfHasNewEntityUnlocked(GameData.GetNextEntitySettings(firstEntity.Guid, secondEntity.Guid));
            RefreshMergeButton();
        }
    }
}