namespace FoxCultGames.Gameplay.Progression
{
    using UnityEngine;
    
    public class ProgressionManager : MonoBehaviour, ISubManager, IProgressionManager
    {
        private IGameContext gameContext;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public void PostInitialize()
        {
        }

        public void UpdateManager()
        {
            
        }
    }
}