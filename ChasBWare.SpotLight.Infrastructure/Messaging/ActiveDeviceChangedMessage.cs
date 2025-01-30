using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveDeviceChangedMessage(IDeviceViewModel device)
        : Message<IDeviceViewModel>(device)
    {
    }
}
