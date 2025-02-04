using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Repositories;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.Tasks;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using ChasBWare.SpotLight.Pages;
using ChasBWare.SpotLight.Popups;
using ChasBWare.SpotLight.Popups.ViewModels;
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
                           .AddSingleton<IMessageService<ActiveDeviceChangedMessage>, MessageService<ActiveDeviceChangedMessage>>()
                           .AddSingleton<IMessageService<ActivePlaylistChangedMessage>, MessageService<ActivePlaylistChangedMessage>>()
                           .AddSingleton<IMessageService<CurrentTrackChangedMessage>, MessageService<CurrentTrackChangedMessage>>()
                           .AddSingleton<IMessageService<FindItemMessage>, MessageService<FindItemMessage>>()
                           .AddSingleton<IMessageService<PlayPlaylistMessage>, MessageService<PlayPlaylistMessage>>()
                           .AddSingleton<IMessageService<ConnectionStatusChangedMessage>, MessageService<ConnectionStatusChangedMessage>>()
                           ;
        }

        public static IServiceCollection RegisterNavigator(this IServiceCollection services)
        {
            return services.AddSingleton<INavigator,Navigator>();
        }

        public static IServiceCollection RegisterPopups(this IServiceCollection services)
        {
            return services.AddTransientPopup<TracksPopupMenu, TrackPopupViewModel>()
                           .AddTransientPopup<LibraryPopupMenu,LibraryPopupViewModel>()
            ;
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
                           .AddSingleton<IDeviceService, DeviceService>()
                           .AddSingleton<IPopupItemService, PopupItemService>()
                           .AddSingleton<ITrackPlayerService, TrackPlayerService>();
        }

        public static IServiceCollection RegisterSpotify(this IServiceCollection services)
        {
            return services.AddSingleton<ISpotifyActionManager, SpotifyActionManager>()
                           .AddSingleton<ISpotifyConnectionManager, SpotifyConnectionManager>()
                           .AddSingleton<ISpotifyPlayerController, SpotifyPlayerController>()
                           .AddSingleton<ISpotyConnectionSession, SpotyConnectionSession>()
                           ;
        }

        public static IServiceCollection RegisterTasks(this IServiceCollection services)
        {
            return services.AddTransient<IArtistAlbumsLoaderTask, ArtistAlbumsLoaderTask>()
                           .AddTransient<IFindArtistTask, FindArtistTask>()
                           .AddTransient<ILibraryLoaderTask, LibraryLoaderTask>()
                           .AddTransient<ILoadAvailableDevicesTask, LoadAvailableDevicesTask>()
                           .AddTransient<ILoadRecentArtistTask, LoadRecentArtistTask>()
                           .AddTransient<ILoadRecentPlaylistTask, LoadRecentPlaylistTask>()
                           .AddTransient<IRemoveArtistTask, RemoveArtistTask>()
                           .AddTransient<ISetPlaylistSavedStatus, SetPlaylistSavedStatus>()
                           .AddTransient<ISearchForAlbumTask, SearchForAlbumTask>()
                           .AddTransient<ISearchForArtistTask, SearchForArtistTask>()
                           .AddTransient<ISetHatedTrackTask,SetHatedTrackTask>()
                           .AddTransient<ISyncToDeviceTask, SyncToDeviceTask>()
                           .AddTransient<ITrackListLoaderTask, TrackListLoaderTask>()
                           .AddTransient<IUpdateLastAccessedTask, UpdateLastAccessedTask>()
                           ;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            return services.AddTransient<IArtistViewModel, ArtistViewModel>()
                           .AddTransient<ICurrentDeviceViewModel, CurrentDeviceViewModel>()
                           .AddTransient<IDeviceListViewModel, DeviceListViewModel>()
                           .AddTransient<IDeviceViewModel, DeviceViewModel>()
                           .AddSingleton<ILibraryViewModel, LibraryViewModel>()
                           .AddSingleton<IPlayerControlViewModel, PlayerControlViewModel>()
                           .AddTransient<IPlaylistListViewModel, PlaylistListViewModel>()
                           .AddTransient<IPlaylistViewModel, PlaylistViewModel>()
                           .AddTransient<IQueueViewModel, QueueViewModel>()
                           .AddSingleton<IRecentAlbumsViewModel, RecentAlbumsViewModel>()
                           .AddSingleton<IRecentArtistsViewModel, RecentArtistsViewModel>()
                           .AddSingleton<IRecentPlaylistsViewModel, RecentPlaylistsViewModel>()
                           .AddSingleton<ISearchArtistsViewModel, SearchArtistsViewModel>()
                           .AddSingleton<ISearchAlbumsViewModel, SearchAlbumsViewModel>()
                         //  .AddSingleton<ISearchPlaylistsViewModel, SearchPlaylistsViewModel>()
                           .AddTransient<ITrackViewModel, TrackViewModel>()
                           .AddTransient<ITrackListViewModel, TrackListViewModel>()
                           ;
        }

        public static IServiceCollection RegisterViews(this IServiceCollection services)
        {
            return services.AddSingleton<ArtistPage>()
                           .AddSingleton<LibraryPage>();
        }

    }
 }
