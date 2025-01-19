namespace FoxCultGames.Gameplay
{
    using Camera;
    using Core;
    using Enclosures;
    using Entities;
    using Input;
    using Loot;
    using Money;
    using Progression;
    using UI;

    public interface IGameContext
    {
        public bool IsNewGame { get; }
        
        public GameSave GameSave { get; }
        public GameInputActions InputActions { get; }
        
        public IEntityManager EntityManager { get; }
        public ICameraManager CameraManager { get; }
        public IMoneyManager MoneyManager { get; }
        public IUiManager UiManager { get; }
        public ILootManager LootManager { get; }
        public IProgressionManager ProgressionManager { get; }
        public IEnclosureManager EnclosureManager { get; }
    }
}