using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class CurrentTrackArgs(string trackId, string albumName, TrackStatus trackStatus)
    {
        public string TrackId { get; } = trackId;
        public string AlbumName { get; } = albumName;
        public TrackStatus State { get; } = trackStatus;
    }

    public class CurrentTrackChangedMessage(string trackId, string albumName, TrackStatus trackStatus)
        : Message<CurrentTrackArgs>(new CurrentTrackArgs(trackId, albumName, trackStatus))
    {
    }
}
