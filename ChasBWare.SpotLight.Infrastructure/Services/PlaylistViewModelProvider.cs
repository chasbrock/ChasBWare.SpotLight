using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Services;

public class PlaylistViewModelProvider(IServiceProvider _serviceProvider)
           : IPlaylistViewModelProvider
{
    public Func<string?, bool>? ExistsInlibrary { get; set; }

    public IPlaylistViewModel CreatePlaylist(Playlist playlist, bool? addToLibrary = null)
    {
        var item = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
        item.Model = playlist;
        item.LastAccessed = playlist.LastAccessed;
        if (addToLibrary != null && (bool)addToLibrary)
        {
            item.InLibrary = true;
        }
        else if (ExistsInlibrary != null)
        {
            item.InLibrary = ExistsInlibrary(playlist.Id);
        }
        return item;
    }
}
