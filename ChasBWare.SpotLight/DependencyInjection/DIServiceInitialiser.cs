using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Repositories;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.Tasks;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using ChasBWare.SpotLight.Pages;
using ChasBWare.SpotLight.Spotify.Classes;
using ChasBWare.SpotLight.Spotify.Interfaces;
using ChasBWare.SpotLight.Spotify.Repositories;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.DependencyInjection
{
    /// <summary>
    /// collection of extension methods to load entities into DI
    /// </summary>
    internal static partial class DIServiceInitialiser
    {
        public static MauiAppBuilder RegisterAllMine(this MauiAppBuilder builder) 
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

  
        public static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            return services.AddTransient<IDbContext, SpotLightDbContext>()
                           .AddTransient<IDbSettings, DefaultDbSettings>();
        }

        public static IServiceCollection RegisterLogging(this IServiceCollection services)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger("Program");

            return services.AddLogging(builder => builder.AddConsole())
                           .AddSingleton(logger);
        }

        public static IServiceCollection RegisterMessageHandlers(this IServiceCollection services)
        {
            return services.AddSingleton<IMessageService<ActiveArtistChangedMessage>, MessageService<ActiveArtistChangedMessage>>()
                           .AddSingleton<IMessageService<ActiveAlbumChangedMessage>, MessageService<ActiveAlbumChangedMessage>>()
                           .AddSingleton<IMessageService<ActivePlaylistChangedMessage>, MessageService<ActivePlaylistChangedMessage>>()
                           .AddSingleton<IMessageService<PlayTracklistMessage>, MessageService<PlayTracklistMessage>>()
                           .AddSingleton<IMessageService<CurrentTrackChangedMessage>, MessageService<CurrentTrackChangedMessage>>()
                           .AddSingleton<IMessageService<ConnectionStatusChangedMessage>, MessageService<ConnectionStatusChangedMessage>>()
                           .AddSingleton<IMessageService<ActiveDeviceChangedMessage>, MessageService<ActiveDeviceChangedMessage>>()
                           ;
        }

        public static IServiceCollection RegisterNavigator(this IServiceCollection services)
        {
            return services.AddSingleton<INavigator,Navigator>();
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddTransient<IAppSettingsRepository, AppSettingsRepository>()
                           .AddTransient<IArtistRepository, ArtistRepository>()
                           .AddTransient<IHatedItemsRepository, HatedItemsRepository>()
                           .AddTransient<IPlaylistRepository, PlaylistRepository>()
                           .AddTransient<IRecentItemRepository, RecentItemRepository>()
                           .AddTransient<ISpotifyArtistRepository, SpotifyArtistRepository>()
                           .AddTransient<ISpotifyDeviceRepository, SpotifyDeviceRepository>()
                           .AddTransient<ISpotifyPlaylistRepository, SpotifyPlaylistRepository>()
                           .AddTransient<ISpotifyTrackRepository, SpotifyTrackRepository>()
                           .AddTransient<ITrackRepository, TrackRepository>()
                           .AddTransient<IUserRepository, UserRepository>()
                           ;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddSingleton<IHatedService, HatedService>()
                           .AddSingleton<ITrackPlayerService, TrackPlayerService>();
        }

        public static IServiceCollection RegisterSpotify(this IServiceCollection services)
        {
            return services.AddSingleton<ISpotifyActionManager, SpotifyActionManager>()
                           .AddSingleton<ISpotifyConnectionManager, SpotifyConnectionManager>()
                           .AddSingleton<ISpotifyPlayerController, SpotifyPlayerController>()
                           .AddSingleton<ISpotyConnectionSession, SpotyConnectionSession>();
        }

        public static IServiceCollection RegisterTasks(this IServiceCollection services)
        {
            return services.AddTransient<IArtistAlbumsLoaderTask, ArtistAlbumsLoaderTask>()
                           .AddTransient<IGetActiveDeviceTask, GetActiveDeviceTask>()
                           .AddTransient<ILibraryLoaderTask, LibraryLoaderTask>()
                           .AddTransient<ILoadAvailableDevicesTask, LoadAvailableDevicesTask>()
                           .AddTransient<ILoadRecentArtistTask, LoadRecentArtistTask>()
                           .AddTransient<ILoadRecentPlaylistTask, LoadRecentPlaylistTask>()
                           .AddTransient<IRemoveArtistTask, RemoveArtistTask>()
                           .AddTransient<ITrackListLoaderTask, TrackListLoaderTask>()
                           .AddTransient<IUpdateLastAccessedTask, UpdateLastAccessedTask>()
                           .AddTransient<ISearchForArtistTask, SearchForArtistTask>()
                           .AddTransient<ISearchForAlbumTask, SearchForAlbumTask>()
                   ;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            return services.AddSingleton<IRecentArtistsViewModel, RecentArtistsViewModel>()
                           .AddSingleton<IRecentAlbumsViewModel, RecentAlbumsViewModel>()
                           .AddSingleton<IRecentPlaylistsViewModel, RecentPlaylistsViewModel>()
                           .AddSingleton<ILibraryViewModel, LibraryViewModel>()
                           .AddTransient<IPlaylistListViewModel, PlaylistListViewModel>()
                           .AddSingleton<ISearchArtistsViewModel, SearchArtistsViewModel>()
                           .AddSingleton<ISearchAlbumsViewModel, SearchAlbumsViewModel>()
                         //  .AddSingleton<ISearchPlaylistsViewModel, SearchPlaylistsViewModel>()
                           .AddSingleton<IPlayerControlViewModel, PlayerControlViewModel>()
                           .AddTransient<IPlaylistViewModel, PlaylistViewModel>()
                           .AddTransient<ITrackViewModel, TrackViewModel>()
                           .AddTransient<ITrackListViewModel, TrackListViewModel>()
                           .AddTransient<IArtistViewModel, ArtistViewModel>()
                           .AddTransient<IDeviceListViewModel, DeviceListViewModel>()
                           .AddTransient<IDeviceViewModel, DeviceViewModel>()
                           .AddTransient<IQueueViewModel, QueueViewModel>()
            ;
        }

        public static IServiceCollection RegisterViews(this IServiceCollection services)
        {
            return services.AddSingleton<ArtistPage>()
                           .AddSingleton<LibraryPage>();
        }

    }
 }
