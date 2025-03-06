using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services
{
    public interface IPlaylistViewModelProvider 
    {
        Func<string?, bool>? ExistsInlibrary { get; set; }

        IPlaylistViewModel CreatePlaylist(Playlist playlist, bool? addToLibrary = null);
    }
}
