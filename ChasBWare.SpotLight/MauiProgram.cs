using Microsoft.Extensions.Logging;
using ChasBWare.SpotLight.DependencyInjection;
using CommunityToolkit.Maui;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Configuration;

namespace ChasBWare.SpotLight;

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
        
        return builder.Build();
    }


    private static MauiAppBuilder RegisterMyServices(this MauiAppBuilder builder)
    {
        builder.SetupLogging();

        builder.Services.RegisterViewModels()
                        .RegisterNavigator()
                        .RegisterDbContext()
                        .RegisterMessageHandlers()
                        .RegisterRepositories()
                        .RegisterServices()
                        .RegisterPopups()
                        .RegisterSpotify()
                        .RegisterTasks()
                        .RegisterViews();

        return builder;
    }


}
