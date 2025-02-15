using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch
{
    public interface IRemovePlaylistTask
    {
        void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, IPlaylistViewModel item);
        void Execute(IRecentViewModel<IPlaylistViewModel> viewModel);
    }
}
