namespace FoxCultGames.Core
{
    using System;
    using Entities;
    using Money;

    [Serializable]
    public class GameSave
    {
        public EntitiesDataModel Entities { get; set; } = new();
        public MoneyDataModel Money { get; set; } = new();
        public Guid DefaultEnclosure { get; set; }
    }
}