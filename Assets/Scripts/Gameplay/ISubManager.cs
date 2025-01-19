namespace FoxCultGames.Gameplay
{
    public interface ISubManager
    {
        void Initialize(IGameContext gameContext);
        void PostInitialize();
        void UpdateManager();
    }
}