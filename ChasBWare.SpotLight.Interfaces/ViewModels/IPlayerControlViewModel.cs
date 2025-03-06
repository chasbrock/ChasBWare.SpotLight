using System.Windows.Input;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlayerControlViewModel
    {
        ICommand BackCommand { get; }
        ICommand PlayCommand { get; }
        ICommand PauseCommand { get; }
        ICommand ForwardCommand { get; }
        ICurrentDeviceViewModel CurrentDevice { get;  }

        double ProgressPercent { get; set; }
        TimeSpan Duration { get; set; }
        TimeSpan PlayedTime { get; set; }
        bool IsPlaying { get; set; }
        bool IsPaused { get; }
        bool IsSyncing { get; set; }
        string TrackName { get; set; }
        string Artists { get; set; }
        ITrackPlayerService TrackPlayerService { get; }
        ICommand OpenArtistCommand { get; }
        ICommand OpenAlbumCommand { get; }
        ConnectionStatus ConnectionStatus { get; }
    }
}
