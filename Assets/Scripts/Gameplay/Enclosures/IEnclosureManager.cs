namespace FoxCultGames.Gameplay.Enclosures
{
    using System;

    public interface IEnclosureManager
    {
        EnclosureController CurrentEnclosure { get; }
        
        void SwipeEnclosure(int direction);
        void SetCurrentAsDefault();
        void ChangeEnclosure(Guid entitySettingsGuid);
    }
}