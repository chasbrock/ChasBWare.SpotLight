using System.Windows.Input;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface ICurrentDeviceViewModel
    {
        string DeviceType { get; }
        bool IsActive { get; set; }
        ICommand MuteCommand { get; }
        string Name { get; }
        int VolumePercent { get; set; }
        DeviceModel Device { get; set; }
        ICommand VolumeUpdatedCommand { get; }
        bool IsMuted { get; set; }
    }
}
