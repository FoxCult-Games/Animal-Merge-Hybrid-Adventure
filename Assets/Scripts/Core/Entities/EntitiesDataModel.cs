namespace FoxCultGames.Core.Entities
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class EntitiesDataModel
    {
        public HashSet<Guid> UnlockedEntities { get; set; } = new();
        public DateTime LastPayoutTime { get; set; }
    }
}