using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;

public interface IFindPlaylistTask
{
    void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, 
                 string playlistId,
                 PlaylistType playlistType);
}

