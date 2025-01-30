using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services
{
    public interface ITrackPlayerService
    {
        event EventHandler<TrackProgressMessageArgs>? OnTrackProgress;

        void Pause();
        void Resume();
        void SkipForward();
        void SkipBackward();
        void StartPlaylist(IPlaylistViewModel playlist, int trackNumber);
        void SyncToDevice();
       
    }
}
