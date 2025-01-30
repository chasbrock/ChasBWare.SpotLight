using ChasBWare.SpotLight.Definitions.Utility;

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

            return new Window(appShell);
        }
    }
}