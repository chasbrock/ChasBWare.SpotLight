using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface IPlaylistViewModel
{
    public RecentPlaylist Model { get; set; }

    string Id { get; }
    string Name { get; }
    string Description { get; }
    string Owner { get; }
    PlaylistType PlaylistType { get; }
    string Uri { get; }
    string? Image { get;  }
    bool IsExpanded { get; set; }
    bool IsSaved { get; set; }
    DateTime ReleaseDate { get;  }
    DateTime LastAccessed { get; set; }

    /// <summary>
    /// list of tracks, tends to be lazy loaded on selection
    /// </summary>        
    ITrackListViewModel TracksViewModel { get; }
    ICommand PlayTracklistCommand { get; }
}
