namespace FoxCultGames.Gameplay.Loot
{
    using Resolvers;
    using UniqueScriptableObjects;
    using UnityEngine;
    using Witchy.Attributes;

    [CreateAssetMenu(menuName = "Data/Loot/Loot Settings")]
    public class LootSettings : UniqueScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeReference, SubclassPicker] private AvailableLootResolver resolver;
        
        public Sprite Icon => icon;
        public AvailableLootResolver Resolver => resolver;
    }
}