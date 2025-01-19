namespace FoxCultGames.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using SubControllers;

    [Serializable]
    public class EntityDataModel
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid SettingsId { get; set; }
        
        public List<SubControllerDataModel> SubControllers { get; set; } = new();
        
        public EntityDataModel(Guid settingsId)
        {
            SettingsId = settingsId;
        }
    }
}