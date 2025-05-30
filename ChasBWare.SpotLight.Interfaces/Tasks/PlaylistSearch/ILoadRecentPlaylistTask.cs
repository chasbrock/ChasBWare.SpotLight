using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;


namespace ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;

public interface ILoadRecentPlaylistTask
{
    void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, PlaylistType playlistType);
}
