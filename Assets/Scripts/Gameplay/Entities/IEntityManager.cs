namespace FoxCultGames.Gameplay.Entities
{
    using System;
    using Settings;

    public interface IEntityManager
    {
        event Action<Guid> OnNewEntityUnlocked;
        
        bool CheckIfHasNewEntityUnlocked(EntitySettings entitySettings);
    }
}