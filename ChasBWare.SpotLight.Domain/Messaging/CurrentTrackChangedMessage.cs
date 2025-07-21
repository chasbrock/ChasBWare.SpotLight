using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Messaging;

public class CurrentTrackChangedMessage(PlayingTrack track, TrackStatus trackStatus)
      : Message()
{
    public PlayingTrack Track { get; } = track;
    public TrackStatus State { get; } = trackStatus;
}
