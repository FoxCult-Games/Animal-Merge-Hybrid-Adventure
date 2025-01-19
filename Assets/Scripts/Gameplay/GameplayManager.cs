namespace FoxCultGames.Gameplay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Camera;
    using Core;
    using Enclosures;
    using Entities;
    using Input;
    using Loot;
    using Money;
    using Progression;
    using Saves;
    using UI;
    using UnityEngine;
    using Utilities;

    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private EntityManager entityManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private LootManager lootManager;
        [SerializeField] private ProgressionManager progressionManager;
        [SerializeField] private EnclosureManager enclosureManager;
        
        private List<ISubManager> subManagers;

        private IGameContext gameContext;
        private AutoSaveHandler autoSaveHandler;
        private bool isInitialized;
        
        private async void Start()
        {
            await GameData.GameData.LoadAssets();
            
            CreateInputActions();
            CreateGameContext();
            InitializeSubManagers();
            PostInitializeSubManagers();
            
            InitializeAutoSaveHandler();
            
            isInitialized = true;
        }

        private void OnDestroy()
        {
            autoSaveHandler.ForceSave();
        }

        private void OnApplicationQuit()
        {
            autoSaveHandler.ForceSave();
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
                autoSaveHandler.ForceSave();
        }

        private void OnDisable()
        {
            gameContext.InputActions.Disable();
        }

        private void Update()
        {
            if (!isInitialized)
                return;
            
            foreach (var subManager in subManagers)
            {
                subManager.UpdateManager();
            }
            
            autoSaveHandler.UpdateTimer();
        }

        private void InitializeSubManagers()
        {
            subManagers = GetComponentsInChildren<ISubManager>().ToList();

            foreach (var subManager in subManagers)
            {
                subManager.Initialize(gameContext);
            }
        }

        private void PostInitializeSubManagers()
        {
            foreach (var subManager in subManagers)
            {
                subManager.PostInitialize();
            }
        }

        private void CreateGameContext()
        {
            var isLoadedGame = GameSaveHelper.Deserialize<GameSave>(out var gameSave);
            gameContext = new GameContext
            {
                IsNewGame = !isLoadedGame,
                GameSave = gameSave,
                InputActions = CreateInputActions(),
                EntityManager = entityManager,
                CameraManager = cameraManager,
                MoneyManager = moneyManager,
                LootManager = lootManager,
                ProgressionManager = progressionManager,
                EnclosureManager = enclosureManager,
                UiManager = uiManager
            };
        }

        private void InitializeAutoSaveHandler()
        {
            autoSaveHandler = new AutoSaveHandler(gameContext);
        }
        
        private static GameInputActions CreateInputActions()
        {
            var inputActions = new GameInputActions();
            inputActions.Enable();
            return inputActions;
        }
    }
}