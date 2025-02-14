using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;

public interface IAddRecentAlbumTask
{
    void Execute(IRecentAlbumsViewModel viewModel, Playlist model);
}
