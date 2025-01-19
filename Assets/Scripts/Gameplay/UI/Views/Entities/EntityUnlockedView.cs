namespace FoxCultGames.Gameplay.UI.Views.Entities
{
    using System;
    using Coffee.UIExtensions;
    using GameData;
    using Loot;
    using PrimeTween;
    using UnityEngine;
    using UnityEngine.UI;

    public class EntityUnlockedView : UiViewBase
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private Image entityImage;
        [SerializeField] private new UIParticle particleSystem;
        [SerializeField] private Button button;
        
        [Space, SerializeField] private float showDuration = 1.6f;
        [SerializeField] private float hideDuration = 0.4f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private TweenSettings<Vector3> pulsingAnimationSettings;
        [Space, SerializeField] private ShakeSettings chestPositionShakeSettings;
        [SerializeField] private ShakeSettings chestRotationShakeSettings;
        [SerializeField] private ShakeSettings chestScaleShakeSettings;

        private Tween pulsingAnimationTween;
        private Action onClickedCallback;
        
        public override void Initialize(IGameContext gameContext)
        {
            base.Initialize(gameContext);
            
            gameContext.EntityManager.OnNewEntityUnlocked += EntityManager_OnNewEntityUnlocked;
            gameContext.LootManager.OnNewLootUnlocked += LootManager_OnNewLootUnlocked;
            
            button.onClick.AddListener(HandleClicked);
        }

        public override void UpdateView()
        {
            base.UpdateView();
            
            if (IsOpen)
                particleSystem.transform.Rotate(0f ,0f, rotationSpeed * Time.deltaTime);
        }

        public override void Close()
        {
            base.Close();
            onClickedCallback = null;
        }

        private void HandleClicked()
        {
            if (onClickedCallback == null)
            {
                AnimateOut();
                return;
            }
            
            button.interactable = false;
            onClickedCallback.Invoke();
        }

        private void AnimateOut()
        {
            particleSystem.Stop();
            StopPulsingAnimation();
            Tween.Scale(container, Vector3.zero, hideDuration, Ease.InBack, useUnscaledTime: true).OnComplete(Close);
        }

        private void AnimateIn()
        {
            particleSystem.Play();
            container.localScale = Vector3.zero;
            Tween.Scale(container, Vector3.one, showDuration, useUnscaledTime: true, ease: Ease.OutBack).OnComplete(PlayPulsingAnimation);
        }

        private void PlayPulsingAnimation()
        {
            pulsingAnimationTween = Tween.Scale(entityImage.transform, pulsingAnimationSettings.startValue , pulsingAnimationSettings.endValue, pulsingAnimationSettings.settings.duration,
                pulsingAnimationSettings.settings.ease, pulsingAnimationSettings.settings.cycles, pulsingAnimationSettings.settings.cycleMode, useUnscaledTime: pulsingAnimationSettings.settings.useUnscaledTime);
        }
        
        private void StopPulsingAnimation()
        {
            pulsingAnimationTween.Stop();
        }

        private void EntityManager_OnNewEntityUnlocked(Guid entityId)
        {
            var entitySettings = GameData.EntitiesSettings[entityId];
            entityImage.sprite = entitySettings.Sprite;
            button.interactable = true;
            Open();
            AnimateIn();
        }

        private void LootManager_OnNewLootUnlocked(LootSettings lootSettings)
        {
            entityImage.sprite = lootSettings.Icon;
            if (lootSettings is ChestLootSettings)    
                onClickedCallback += OpenChest;
            
            Open();
            AnimateIn();

            void OpenChest()
            {
                onClickedCallback = null;
                StopPulsingAnimation();
                Sequence.Create(useUnscaledTime: true)
                    .Chain(Tween.ShakeLocalRotation(entityImage.transform, chestRotationShakeSettings))
                    .Group(Tween.ShakeScale(entityImage.transform, chestScaleShakeSettings))
                    .Group(Tween.ShakeLocalPosition(entityImage.transform, chestPositionShakeSettings))
                    .OnComplete(() =>
                    {
                        entityImage.sprite = lootSettings.Resolver.Resolve(gameContext, lootSettings);
                        PlayPulsingAnimation();
                        button.interactable = true;
                    });
            }
        }
    }
}