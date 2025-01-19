namespace FoxCultGames.Gameplay.UI
{
    using Views;

    public interface IUiManager
    {
        T GetView<T>() where T : UiViewBase;
    }
}