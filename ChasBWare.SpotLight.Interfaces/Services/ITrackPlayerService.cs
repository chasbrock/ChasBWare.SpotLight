using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

public interface ITrackPlayerService
{
    event EventHandler<PlayingTrack>? OnTrackProgress;

    void Pause();
    void Resume();
    void SkipForward();
    void SkipBackward();
    void StartPlaylist(Playlist playlist, int trackNumber);
    void UpdateNowPlaying(PlayingTrack nowPlaying);
}
