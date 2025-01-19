namespace FoxCultGames.Gameplay.Entities.Settings
{
    using Utilities;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Entities/Entity Movement Settings")]
    public class EntityMovementSettings : ScriptableObject
    {
        [Header("Movement Settings")]
        [SerializeField] private FloatRange movementSpeed;
        [SerializeField] private FloatRange movementRange;
        [SerializeField] private float rotationChange = 4f;
        [SerializeField] private float rotationPerSideDuration = 0.5f;
        [SerializeField] private float arrivalDistance = 0.1f;
        
        [Header("Idle Settings")]
        [SerializeField] private FloatRange idleTime;
        [SerializeField] private float scaleModifier = 1.05f;
        [SerializeField] private float scaleDuration = 0.5f;
        
        public float MovementSpeed => movementSpeed.Value;
        public float MovementRange => movementRange.Value;
        public float ArrivalDistance => arrivalDistance;
        public float RotationChange => rotationChange;
        public float RotationPerSideDuration => rotationPerSideDuration;
        public float IdleTime => idleTime.Value;
        public float ScaleModifier => scaleModifier;
        public float ScaleDuration => scaleDuration;
    }
}