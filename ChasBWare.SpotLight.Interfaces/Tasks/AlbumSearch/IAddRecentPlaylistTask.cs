using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;

/// <summary>
/// task to add playlist to recent playlist viewmodel, and
/// ensure that it is stored in db
/// </summary>
public interface IAddRecentPlaylistTask
{
    /// <summary>
    /// add model to viremodel and serialise to database
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="model"></param>
    void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, Playlist model);
}

