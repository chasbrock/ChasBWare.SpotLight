using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Messaging;

public class CurrentTrackChangedMessage(string trackId, string playlistName, TrackStatus trackStatus)
      : Message()
{
    public string TrackId { get; } = trackId;
    public string PlaylistName { get; } = playlistName;
    public TrackStatus State { get; } = trackStatus;
}
