namespace FoxCultGames.Utilities
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [Serializable]
    public struct FloatRange
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        
        public float Value => Random.Range(min, max);
    }
}