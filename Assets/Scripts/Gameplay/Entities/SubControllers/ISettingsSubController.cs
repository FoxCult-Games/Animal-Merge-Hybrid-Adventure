namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System;

    public interface ISettingsSubController : IEntitySubController
    {
        Type SettingsType { get; }
        
        void AssignSettings(EntitySubControllerSettings settings);
    }
}