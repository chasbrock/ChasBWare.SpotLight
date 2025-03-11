using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface ILibraryViewModel 
               : IPlaylistSelectorViewModel
{
    IPlayerControlViewModel PlayerControlViewModel { get; }

    bool Exists(string? playlistId);
    void AddItems(IEnumerable<Playlist> items);
}
