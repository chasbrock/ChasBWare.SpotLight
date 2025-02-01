using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.ViewModels
{
    public interface IDeviceViewModel 
    {
        string DeviceImage { get; }
        string DeviceType { get;  }
        bool IsActive { get; set; }
        string Name { get;  }
        DeviceModel Model { get; set; }
    }
}
