using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ILibraryViewModel : IPlaylistSelectorViewModel
    {
        bool Exists(string? playlistId);
        void AddItems(IEnumerable<Playlist> items);
    }
}
