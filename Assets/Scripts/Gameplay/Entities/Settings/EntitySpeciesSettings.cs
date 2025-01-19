namespace FoxCultGames.Gameplay.Entities.Settings
{
    using Enclosures;
    using UnityEngine;

    public class EntitySpeciesSettings : ScriptableObject
    {
        [SerializeField] private EnclosureSettings enclosureSettings;
        
        public EnclosureSettings EnclosureSettings => enclosureSettings;
    }
}