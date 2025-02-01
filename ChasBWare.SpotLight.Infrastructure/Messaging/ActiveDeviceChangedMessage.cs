using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveDeviceChangedMessage(DeviceModel device)
        : Message<DeviceModel>(device)
    {
    }
}
