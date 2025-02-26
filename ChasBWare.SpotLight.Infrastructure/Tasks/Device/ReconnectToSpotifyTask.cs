using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Device;

public class ReconnectToSpotifyTask(IServiceProvider _serviceProvider)                            
           : IReconnectToSpotifyTask
{

    public void Execute()
    {
        Task.Run(() => RunTask());
    }

    private void RunTask()
    {
        // wait for long enough for load events to complete
        Thread.Sleep(2000);

        // now call task to sync with spotify
        // (assumes that IPlayerControlViewModel is singleton so do not change it)
        var task = _serviceProvider.GetRequiredService<ISyncToDeviceTask>();
        var viewModel = _serviceProvider.GetRequiredService<IPlayerControlViewModel>();
        task.Execute(viewModel);
    }
}

