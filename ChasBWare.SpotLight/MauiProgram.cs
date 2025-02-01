using Microsoft.Extensions.Logging;
using ChasBWare.SpotLight.DependencyInjection;
using CommunityToolkit.Maui;

namespace ChasBWare.SpotLight
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                   .UseMauiCommunityToolkit()
                   .RegisterMyServices()
                   .ConfigureFonts(fonts =>
                   {
                       fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                       fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                   });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterMyServices(this MauiAppBuilder builder)
        {
            builder.Services.RegisterDbContext()
                            .RegisterLogging()
                            .RegisterMessageHandlers()
                            .RegisterRepositories()
                            .RegisterServices()
                            .RegisterNavigator()
                            .RegisterSpotify()
                            .RegisterTasks()
                            .RegisterViewModels()
                            .RegisterViews();

            return builder;
        }


    }
}
