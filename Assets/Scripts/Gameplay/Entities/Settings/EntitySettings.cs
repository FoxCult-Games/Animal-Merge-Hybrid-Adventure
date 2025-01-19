namespace FoxCultGames.Gameplay.Entities.Settings
{
    using System.Collections.Generic;
    using Money;
    using Money.Currency;
    using SubControllers;
    using UniqueScriptableObjects;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public enum EntityType
    {
        Normal,
        Hybrid,
    }
    
    [CreateAssetMenu(menuName = "Data/Entities/Entity Settings")]
    public class EntitySettings : UniqueScriptableObject
    {
        [SerializeField] private string entityName;
        [SerializeField] private Sprite entitySprite;
        [SerializeField] private EntityType entityType;
        [SerializeField] private EntityTier entityTier;
        [SerializeField] private GameObject prefab;
        [SerializeField] private EntitySettings[] requiredEntitySettings = new EntitySettings[2];
        [SerializeField] private MoneyEntry price = new() { currency = CurrencyType.Coins };
        [SerializeField] private EntitySpeciesSettings speciesSettings;
        [SerializeField] private EntityMovementSettings entityMovementSettings;
        [SerializeReference] private List<EntitySubControllerSettings> subControllersSettings = new();
        
        public string EntityName => entityName;
        public Sprite Sprite => entitySprite;
        public EntityType EntityType => entityType;
        public EntityTier EntityTier => entityTier;
        public GameObject Prefab => prefab;
        public IReadOnlyList<EntitySettings> RequiredEntitySettings => requiredEntitySettings;
        public MoneyEntry Price => price;
        public EntitySpeciesSettings SpeciesSettings => speciesSettings;
        public EntityMovementSettings EntityMovementSettings => entityMovementSettings;
        public IEnumerable<EntitySubControllerSettings> SubControllersSettings => subControllersSettings;

        public const string ControllerSettingsFieldName = nameof(subControllersSettings);
        private const string DefaultEntitiesMovementSettingsKey = "DefaultMovementSettings";

        private void OnEnable()
        {
            if (entityMovementSettings == null)
                LoadDefaultEntityMovementSettings();
        }

        private async void LoadDefaultEntityMovementSettings()
        {
            entityMovementSettings = await Addressables.LoadAssetAsync<EntityMovementSettings>(DefaultEntitiesMovementSettingsKey).Task;
        }
    }
}