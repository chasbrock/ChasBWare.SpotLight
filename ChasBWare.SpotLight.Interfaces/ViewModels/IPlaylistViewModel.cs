using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface IPlaylistViewModel 
{
    string Description { get; }
    string Id { get; }
    string? Image { get; }
    bool IsSelected { get; set; }
    bool IsExpanded { get; set; }
    DateTime LastAccessed { get; set; }
    Playlist Playlist { get; set; }
    string Name { get; }
    List<KeyValue> Owners { get; }
    KeyValue? Owner { get; }
    PlaylistType PlaylistType { get; }
    DateTime ReleaseDate { get; }
    ITrackListViewModel TracksViewModel { get; }
    string Uri { get; }
    bool InLibrary { get; set; }
}
