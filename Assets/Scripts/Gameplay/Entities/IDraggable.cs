namespace FoxCultGames.Gameplay.Entities
{
    using System;
    using UnityEngine;

    public interface IDraggable
    {
        event Action OnStartedDragging;
        event Action OnStoppedDragging;
        
        void StartDragging();
        void UpdateDragging(Vector2 screenPosition);
        void StopDragging();
    }
}