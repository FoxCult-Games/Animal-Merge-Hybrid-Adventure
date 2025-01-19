namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System;
    using UnityEngine;
    using Utilities;

    [Serializable]
    public abstract class EntitySubControllerSettings
    {
        [HideInInspector] public string name;
        
        protected EntitySubControllerSettings()
        {
            name = GetType().Name.ToHumanReadable();
        }
    }
}