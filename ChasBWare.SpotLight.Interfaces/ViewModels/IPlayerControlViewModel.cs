using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlayerControlViewModel
    {
        ICommand BackCommand { get; }
        ICommand PlayCommand { get; }
        ICommand PauseCommand { get; }
        ICommand ForwardCommand { get; }
        ICurrentDeviceViewModel CurrentDevice { get; }
        IConnectionStatusViewModel ConnectionStatusViewModel { get; }

        int ProgressPercent { get; set; }
        string ProgressText { get; set; }
        bool IsPlaying { get; set; }
        bool IsPaused { get; }
        string CurrentTrack { get; set; }
        string CurrentArtist { get; set; }
    }
}
