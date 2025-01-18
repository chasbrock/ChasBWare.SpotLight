using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ITrackViewModel 
    {
        public IPlaylistViewModel? Playlist { get; set; }
        public Track Track { get; set; }

        //[WriteableDataIndex(4)]
        string Album { get; }
        //[WriteableDataIndex(5)]
        string Artists { get; }
        //[WriteableDataIndex(3)]
        string Duration { get; }
        //[WriteableDataIndex(1)]
        string Id { get; }
        //[WriteableDataIndex(2)]
        string Name { get; }
        //[WriteableDataIndex(0)]
        int TrackNumber { get; }

        bool IsHated { get; set; }
        bool IsSelected { get; set; }
        TrackStatus Status { get; set; }
        void PlayTrackList();
    }
}
