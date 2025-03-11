using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

public interface IDeviceViewModel 
{
    DeviceTypes DeviceType { get;  }
    bool IsActive { get; set; }
    string Name { get;  }
    DeviceModel Model { get; set; }
    string RawDeviceType { get; }
}
