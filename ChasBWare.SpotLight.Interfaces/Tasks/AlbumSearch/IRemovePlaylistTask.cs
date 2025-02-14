using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch
{
    public interface IRemovePlaylistTask
    {
        void Execute(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item);
        void Execute(IRecentAlbumsViewModel viewModel);

        void Execute(IRecentPlaylistsViewModel viewModel, IPlaylistViewModel item);
        void Execute(IRecentPlaylistsViewModel viewModel);

    }
}
