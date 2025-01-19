namespace FoxCultGames.Gameplay.Entities.Settings
{
    using System;
    using System.Collections.Generic;
    using SubControllers;
    using UnityEngine;

    [Serializable]
    public class MexicanCockroachControllerSettings : EntitySubControllerSettings
    {
        [SerializeField] private List<Sprite> sprites = new();
        [SerializeField] private float speed = 0.2f;
        [SerializeField] private float appearanceChance = 0.5f;

        public List<Sprite> Sprites => sprites;
        public float Speed => speed;
        public float AppearanceChance => appearanceChance;
    }
}