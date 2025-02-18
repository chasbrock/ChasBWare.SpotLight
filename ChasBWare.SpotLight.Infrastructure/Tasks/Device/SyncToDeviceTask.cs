using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Device;

public class SyncToDeviceTask(IDispatcher _dispatcher,
                              ISpotifyDeviceRepository _spotifyDeviceRepository )
           : ISyncToDeviceTask
{

    public void Execute(IPlayerControlViewModel viewModel) 
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IPlayerControlViewModel viewModel)
    {
        var context = _spotifyDeviceRepository.GetCurrentContext();
        _dispatcher.Dispatch(() =>
        {
            if (context != null)
            {
                viewModel.CurrentDevice.Device = context.Device;
                viewModel.TrackPlayerService.UpdateNowPlaying(context.Track);
            }
            else
            {
                viewModel.CurrentDevice.Device.IsActive = false;
            }
            viewModel.IsSyncing = false;
        });
        
    }

}
