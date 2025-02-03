using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlaylistViewModel
    {
        public RecentPlaylist Model { get; set; }

        public string Description { get; }
        public string Id { get; }
        public string Name { get; }
        public string Owner { get; }
        public PlaylistType PlaylistType { get; }
        public string Uri { get; }
        public string? Image { get;  }
        public bool IsExpanded { get; set; }
        public DateTime ReleaseDate { get;  }
        public DateTime LastAccessed { get; set; }

        /// <summary>
        /// list of tracks, tends to be lazy loaded on selection
        /// </summary>        
        public ITrackListViewModel TracksViewModel { get; }
        ICommand PlayTracklistCommand { get; }
    }
}
