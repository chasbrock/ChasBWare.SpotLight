using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class GetActiveDeviceTask(IDispatcher _dispatcher,
                                     ISpotifyDeviceRepository _spotifyDeviceRepository)
               : IGetActiveDeviceTask
    {
        public void Execute(IPlayerControlViewModel playerControlViewModel)
        {
            Task.Run(()=>RunTask(playerControlViewModel));
        }

        private async void RunTask(IPlayerControlViewModel playerControlViewModel)
        {
            var currentDevice = await _spotifyDeviceRepository.GetActiveDevice();
            _dispatcher.Dispatch(() =>
            {
                playerControlViewModel.CurrentDevice = currentDevice ?? new DeviceViewModel();
            });
        }
    }
}
