using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;


namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface ILoadRecentPlaylistTask
    {
        void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType);
    }

    public interface ILoadRemoveRecentPlaylistTask
    {
        void Execute(IRecentViewModel<IPlaylistViewModel> viewModel,string playlistId);
    }


    
}
