namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using System;
    using Core.Entities.SubControllers;

    public interface ISerializableSubController : IEntitySubController
    {
        Type DataModelType { get; }
        
        SubControllerDataModel CreateDataModel();
        void SetDataModel(SubControllerDataModel dataModel);
    }
}