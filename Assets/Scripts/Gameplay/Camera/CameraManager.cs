namespace FoxCultGames.Gameplay.Camera
{
    using UnityEngine;

    public class CameraManager : MonoBehaviour, ISubManager, ICameraManager
    {
        [SerializeField] private new Camera camera;

        public Camera Camera => camera;
        
        public void Initialize(IGameContext gameContext)
        {
            
        }

        public void PostInitialize()
        {
            
        }

        public void UpdateManager()
        {
            
        }
    }
}