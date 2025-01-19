namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System.Collections;
    using Core.Entities.SubControllers;
    using Settings;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = UnityEngine.Random;

    public class MexicanCockroachEasterEggController : SerializableEntitySubController<MexicanCockroachControllerSettings, MexicanCockroachEasterEggSubControllerDataModel>
    {
        [SerializeField] private Image spriteRenderer;

        protected override MexicanCockroachEasterEggSubControllerDataModel CreateDataModel()
        {
            return new MexicanCockroachEasterEggSubControllerDataModel
            {
                IsActive = RollAppearance()
            };
        }

        protected override void Initialize(IGameContext gameContext, EntityController entityController)
        {
            if (!DataModel.IsActive)
                return;
            
            StartCoroutine(PlayAnimation());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected override void UpdateController()
        {
            
        }

        private bool RollAppearance()
        {
            return Random.value < Settings.AppearanceChance;
        }

        private IEnumerator PlayAnimation()
        {
            while (true)
            {
                spriteRenderer.sprite = Settings.Sprites[Random.Range(0, Settings.Sprites.Count)];
                yield return new WaitForSeconds(Settings.Speed);
            }
        }
    }
}