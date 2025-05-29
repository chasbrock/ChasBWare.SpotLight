using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

public interface ITrackViewModel
{
    public IPlaylistViewModel? Playlist { get; set; }
    public Track Model { get; set; }

    string Id { get; }
    string Name { get; }
    string Album { get; }
    List<KeyValue> Artists { get; }
    string Duration { get; }
    int TrackNumber { get; }

    bool IsHated { get; set; }
    TrackStatus Status { get; set; }
    void PlayTrackList();
}
