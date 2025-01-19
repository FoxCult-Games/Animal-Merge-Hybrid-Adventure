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

    public class GameContext : IGameContext
    {
        public bool IsNewGame { get; set; }
        
        public GameSave GameSave { get; set; }
        public GameInputActions InputActions { get; set; }
            
        public IEntityManager EntityManager { get; set; }
        public ICameraManager CameraManager { get; set; }
        public IMoneyManager MoneyManager { get; set; }
        public IUiManager UiManager { get; set; }
        public ILootManager LootManager { get; set; }
        public IProgressionManager ProgressionManager { get; set; }
        public IEnclosureManager EnclosureManager { get; set; }
    }
}