using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Install;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var appShell = new AppShell();
            var navigator = _serviceProvider.GetService<INavigator>() as Navigator;
            navigator?.SetShell(appShell);

         //   SecureStorage.Default.RemoveAll();

            Task.Run(() => CheckForInitialisation());
            return new Window(appShell);
        }

        private async void CheckForInitialisation()
        {
            Thread.Sleep(1000);
            var val = await SecureStorage.Default.GetAsync(InstallViewModel.Initialised);
            if  (val != true.ToString())
            {
                var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
                dispatcher.Dispatch(() =>
                {
                    var popupService = _serviceProvider.GetRequiredService<IPopupService>();
                    popupService.ShowPopupAsync<InstallViewModel>();
                });
            }
        }


    }
}