using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Install;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public App(IServiceProvider serviceProvider,
                   ILogger<App> logger) : base()
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                _logger.LogInformation("Creating window");
                var appShell = new AppShell();
                var navigator = _serviceProvider.GetService<INavigator>() as Navigator;
                navigator?.SetShell(appShell, _serviceProvider);

                // SecureStorage.Default.RemoveAll();

                Task.Run(() => CheckForInitialisation());
                return new Window(appShell);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to create main window");
                throw;
            }
        }

        private async void CheckForInitialisation()
        {
            // make sure secure storeage is pouplatd
            _logger.LogInformation("Starting CheckForInitialisation"); 
            Thread.Sleep(1000);
            try
            {
                var val = await SecureStorage.Default.GetAsync(InstallViewModel.Initialised);
                if (val != true.ToString())
                {
                    var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
                    dispatcher.Dispatch(() =>
                    {
                        var popupService = _serviceProvider.GetRequiredService<IPopupService>();
                        popupService.ShowPopupAsync<InstallViewModel>();
                    });
                }
                else
                {
                    var task = _serviceProvider.GetRequiredService<IReconnectToSpotifyTask>();
                    task.Execute();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "CheckForInitialisation failed");
            }
        }


    }
}