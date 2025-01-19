namespace FoxCultGames.Gameplay.Loot.Resolvers
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class AvailableLootResolver
    {
        public abstract Sprite Resolve(IGameContext gameContext, LootSettings lootSettings);
    }
}