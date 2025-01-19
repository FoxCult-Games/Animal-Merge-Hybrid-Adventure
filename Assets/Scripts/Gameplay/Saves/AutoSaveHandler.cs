namespace FoxCultGames.Gameplay.Saves
{
    using UnityEngine;
    using Utilities;

    public class AutoSaveHandler
    {
        private readonly IGameContext gameContext;
        
        private float autoSaveTimer;
        
        private const float AutoSaveInterval = 60f;
        
        public AutoSaveHandler(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public void UpdateTimer()
        {
            autoSaveTimer += Time.deltaTime;

            if (autoSaveTimer < AutoSaveInterval) 
                return;
            
            autoSaveTimer = 0f;
            Save();
        }
        
        public void ForceSave()
        {
            Save();
        }
        
        private void Save()
        {
            Debug.Log("[AutoSaveHandler] Saving game...");
            GameSaveHelper.Serialize(gameContext.GameSave);
        }
    }
}