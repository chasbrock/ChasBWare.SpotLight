using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;

public interface IAddRecentPlaylistTask
{
    void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, Playlist model);
}
