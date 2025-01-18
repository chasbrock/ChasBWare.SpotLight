namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlaylistSelectorViewModel : IGroupedListViewModel<IPlaylistViewModel>
    {
        void ExecuteLibrayCommand(IPlaylistViewModel? selectedItem);
    }
}
