using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class ChangeActiveDeviceTask(IDispatcher _dispatcher,
                                        ISpotifyDeviceRepository  _spotifyDeviceRepository,
                                        IMessageService<ActiveDeviceChangedMessage> _activeDeviceMessageService)
              : IChangeActiveDeviceTask
    {
        public void Execute(IDeviceViewModel selectedDevice)
        {
            Task.Run(() => RunTask(selectedDevice));
       }

        private async void RunTask(IDeviceViewModel selectedDevice)
        {
            if (await _spotifyDeviceRepository.SetDeviceAsActive(selectedDevice.Model.Id))
            {
                _dispatcher.Dispatch(() =>
                {
                    selectedDevice.Model.IsActive = true;
                    _activeDeviceMessageService.SendMessage(new ActiveDeviceChangedMessage(selectedDevice.Model));
                });
            }
        }
    }
}
