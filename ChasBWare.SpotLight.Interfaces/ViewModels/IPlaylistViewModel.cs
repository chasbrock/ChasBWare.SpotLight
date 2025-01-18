using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlaylistViewModel
    {
        public Playlist Model { get; set; }

        public string Description { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public PlaylistType PlaylistType { get; set; }
        public string Uri { get; set; }
        public string? Image { get; set; }
        public bool IsTracksExpanded { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime LastAccessed { get; set; }

        /// <summary>
        /// list of tracks, tends to be lazy loaded on selection
        /// </summary>        
        public ITrackListViewModel TracksViewModel { get; }
        ICommand PlayTracklistCommand { get; }
    }
}
