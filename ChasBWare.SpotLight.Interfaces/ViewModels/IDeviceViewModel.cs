using System.Windows.Input;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IDeviceViewModel 
    {
        string DeviceImage { get; }
        string DeviceType { get;  }
        bool IsActive { get; set; }
        string IsMutedCommandImage { get;  }
        ICommand MuteCommand { get; }
        string Name { get;  }
        int VolumePercent { get; set; }
        DeviceModel Model { get; set; }
    }
}
