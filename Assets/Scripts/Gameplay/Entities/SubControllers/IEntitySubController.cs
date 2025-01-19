namespace FoxCultGames.Gameplay.Entities.SubControllers
{
    using Core.Entities.SubControllers;

    public interface IEntitySubController
    {
        void Initialize(IGameContext gameContext, EntityController entityController, SubControllerDataModel dataModel);
        void UpdateController();
    }
}