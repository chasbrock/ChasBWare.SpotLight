using System.Windows.Input;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IPlayerControlViewModel
    {
        ICommand BackCommand { get; }
        ICommand PlayCommand { get; }
        ICommand PauseCommand { get; }
        ICommand ForwardCommand { get; }
        IDeviceViewModel CurrentDevice { get; set; }

        double ProgressPercent { get; set; }
        string ProgressText { get; set; }
        bool IsPlaying { get; set; }
        bool IsPaused { get; }
        string CurrentTrack { get; set; }
        string CurrentArtist { get; set; }
    }
}
