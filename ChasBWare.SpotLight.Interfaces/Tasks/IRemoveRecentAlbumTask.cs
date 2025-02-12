using ChasBWare.SpotLight.Definitions.ViewModels;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IRemoveRecentAlbumTask
    {
        void Execute(IRecentAlbumsViewModel viewModel, IPlaylistViewModel item);
        void Execute(IRecentAlbumsViewModel viewModel);
    }
}
