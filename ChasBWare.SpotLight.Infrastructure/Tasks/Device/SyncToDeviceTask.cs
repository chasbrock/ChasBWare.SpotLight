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
        if (context != null)
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.CurrentDevice.Device = context.Device;
                viewModel.TrackPlayerService.UpdateNowPlaying(context.Track);
                viewModel.IsSyncing = false;
            });
        }
    }

}
