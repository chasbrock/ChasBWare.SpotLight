using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class DeviceViewModel : Notifyable, IDeviceViewModel
{
    public DeviceModel Model { get; set; } = DeviceHelper.GetLocalDevice();

    public string Name { get => Model.Name; }
    public DeviceTypes DeviceType { get => Model.DeviceType; }
    public string RawDeviceType { get => Model.RawDeviceType;  }

    public bool IsActive
    {
        get => Model.IsActive;
        set => SetField(Model, value);
    }

    public override string ToString()
    {
        return Name;
    }
}