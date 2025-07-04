﻿using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Domain.Services;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Repositories;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Infrastructure.Tasks.Device;
using ChasBWare.SpotLight.Infrastructure.Tasks.Library;
using ChasBWare.SpotLight.Infrastructure.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Infrastructure.Tasks.Users;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using ChasBWare.SpotLight.Install;
using ChasBWare.SpotLight.Pages;
using ChasBWare.SpotLight.Popups;
using ChasBWare.SpotLight.Spotify.Classes;
using ChasBWare.SpotLight.Spotify.Interfaces;
using ChasBWare.SpotLight.Spotify.Repositories;
using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.DependencyInjection;

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

    public static void SetupLogging(this MauiAppBuilder builder)
    {
        builder.Logging.SetMinimumLevel(LogLevel.Trace) // IMPORTANT: set your minimum log level, here Trace
                       .AddTraceLogger(
                           options =>
                           {
                               options.MinLevel = LogLevel.Debug;
                               options.MaxLevel = LogLevel.Critical;
                           }) // Will write to the Debug Output
                       .AddConsoleLogger(
                           options =>
                            {
                                options.MinLevel = LogLevel.Information;
                                options.MaxLevel = LogLevel.Critical;
                            }) // Will write to the Console Output (logcat for android)
                       .AddInMemoryLogger(
                            options =>
                            {
                                options.MaxLines = 1024;
                                options.MinLevel = LogLevel.Debug;
                                options.MaxLevel = LogLevel.Critical;
                            })
                       .AddStreamingFileLogger(
                           options =>
                            {
                                options.RetainDays = 2;
                                options.FolderPath = Path.Combine(
                                    FileSystem.CacheDirectory,
                                    "MetroLogs");
                            });
        builder.Services.AddSingleton(LogOperatorRetriever.Instance);
    }

    public static IServiceCollection RegisterMessageHandlers(this IServiceCollection services)
    {
        return services.AddSingleton<IMessageService<ActiveItemChangedMessage>, MessageService<ActiveItemChangedMessage>>()
                       .AddSingleton<IMessageService<AppActivationChanged>, MessageService<AppActivationChanged>>()
                       .AddSingleton<IMessageService<CurrentTrackChangedMessage>, MessageService<CurrentTrackChangedMessage>>()
                       .AddSingleton<IMessageService<FindItemMessage>, MessageService<FindItemMessage>>()
                       .AddSingleton<IMessageService<PlayPlaylistMessage>, MessageService<PlayPlaylistMessage>>()
                       .AddSingleton<IMessageService<ConnectionStatusChangedMessage>, MessageService<ConnectionStatusChangedMessage>>();
    }

    public static IServiceCollection RegisterNavigator(this IServiceCollection services)
    {
        // find all singletons that implement INavigationClient
        // when navigation is initialised it will link them up
        var navigatable = services.Where(sd => sd.Lifetime == ServiceLifetime.Singleton &&
                                               typeof(INavigationClient).IsAssignableFrom(sd.ServiceType))
                                  .Select(sd => sd.ServiceType)
                                  .ToList();

        return services.AddSingleton<INavigator>(new Navigator(navigatable));
    }

    public static IServiceCollection RegisterPopups(this IServiceCollection services)
    {
        return services.AddTransientPopup<InstallPopup, InstallViewModel>()
                       .AddTransientPopup<LibraryPopupMenu, LibraryPopupViewModel>()
                       .AddTransientPopup<RecentArtistPopupMenu, RecentArtistPopupViewModel>()
                       .AddTransientPopup<RecentAlbumPopupMenu, RecentPlaylistPopupViewModel>()
                       .AddTransientPopup<RecentUserPopupMenu, RecentUserPopupViewModel>()
                       .AddTransientPopup<DevicesPopupMenu, DevicePopupViewModel>()
                       .AddTransientPopup<TrackPopupMenu, TrackPopupViewModel>();
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services.AddTransient<IAppSettingsRepository, AppSettingsRepository>()
                       .AddTransient<IArtistRepository, ArtistRepository>()
                       .AddTransient<IHatedItemsRepository, HatedItemsRepository>()
                       .AddTransient<ILibraryRepository, LibraryRepository>()
                       .AddTransient<ISearchItemRepository, SearchItemRepository>()
                       .AddTransient<ISpotifyArtistRepository, SpotifyArtistRepository>()
                       .AddTransient<ISpotifyDeviceRepository, SpotifyDeviceRepository>()
                       .AddTransient<ISpotifyPlaylistRepository, SpotifyPlaylistRepository>()
                       .AddTransient<ISpotifyTrackRepository, SpotifyTrackRepository>()
                       .AddTransient<ISpotifyUserRepository, SpotifyUserRepository>()
                       .AddTransient<ITrackRepository, TrackRepository>()
                       .AddTransient<IUserRepository, UserRepository>();
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services.AddSingleton<IAlertService, AlertService>()
                       .AddSingleton<IDeviceService, DeviceService>()
                       .AddSingleton<IHatedService, HatedService>()
                       .AddSingleton<IPlaylistViewModelProvider, PlaylistViewModelProvider>()
                       .AddSingleton<IPopupItemService, PopupItemService>()
                       .AddSingleton<ITrackPlayerService, TrackPlayerService>();
    }

    public static IServiceCollection RegisterSpotify(this IServiceCollection services)
    {
        return services.AddSingleton<ISpotifyActionManager, SpotifyActionManager>()
                       .AddSingleton<ISpotifyConnectionManager, SpotifyConnectionManager>()
                       .AddSingleton<ISpotifyPlayerController, SpotifyPlayerController>()
                       .AddSingleton<ISpotyConnectionSession, SpotifyConnectionSession>();
    }

    public static IServiceCollection RegisterTasks(this IServiceCollection services)
    {
        return services.AddTransient<IAddRecentPlaylistTask, AddRecentPlaylistTask>()
                       .AddTransient<IAddRecentArtistTask, AddRecentArtistTask>()
                       .AddTransient<IAddRecentUserTask, AddRecentUserTask>()
                       .AddTransient<IArtistAlbumsLoaderTask, ArtistAlbumsLoaderTask>()
                       .AddTransient<IUserAlbumsLoaderTask, UserAlbumsLoaderTask>()
                       .AddTransient<IChangeActiveDeviceTask, ChangeActiveDeviceTask>()
                       .AddTransient<IFindArtistTask, FindArtistTask>()
                       .AddTransient<IFindPlaylistTask, FindPlaylistTask>()
                       .AddTransient<IFindUserTask, FindUserTask>()
                       .AddTransient<ILibraryLoaderTask, LibraryLoaderTask>()
                       .AddTransient<ILoadAvailableDevicesTask, LoadAvailableDevicesTask>()
                       .AddTransient<ILoadRecentArtistTask, LoadRecentArtistTask>()
                       .AddTransient<ILoadRecentPlaylistTask, LoadRecentPlaylistTask>()
                       .AddTransient<ILoadRecentUserTask, LoadRecentUserTask>()
                       .AddTransient<IReconnectToSpotifyTask, ReconnectToSpotifyTask>()
                       .AddTransient<IRemovePlaylistTask, RemoveRecentPlaylistTask>()
                       .AddTransient<IRemoveRecentArtistTask, RemoveRecentArtistTask>()
                       .AddTransient<IRemoveRecentUserTask, RemoveRecentUserTask>()
                       .AddTransient<ITransferToLibraryTask, TransferToLibraryTask>()
                       .AddTransient<ISearchForAlbumTask, SearchForAlbumTask>()
                       .AddTransient<ISearchForArtistTask, SearchForArtistTask>()
                       .AddTransient<ISearchForPlaylistTask, SearchForPlaylistTask>()
                       .AddTransient<ISearchForUserTask, SearchForUserTask>()
                       .AddTransient<ISearchLibraryTask, SearchLibraryTask>()
                       .AddTransient<ISetHatedTrackTask, SetHatedTrackTask>()
                       .AddTransient<ISyncToDeviceTask, SyncToDeviceTask>()
                       .AddTransient<ITrackListLoaderTask, TrackListLoaderTask>()
                       .AddTransient<IUpdateLastAccessedTask, UpdateLastAccessedTask>();
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        return services.AddTransient<IArtistViewModel, ArtistViewModel>()
                       .AddTransient<ICurrentDeviceViewModel, CurrentDeviceViewModel>()
                       .AddSingleton<IDeviceListViewModel, DeviceListViewModel>()
                       .AddTransient<IDeviceViewModel, DeviceViewModel>()
                       .AddSingleton<ILibraryViewModel, LibraryViewModel>()
                       .AddSingleton<IPlayerControlViewModel, PlayerControlViewModel>()
                       .AddTransient<IPlaylistListViewModel, PlaylistListViewModel>()
                       .AddTransient<IPlaylistViewModel, PlaylistViewModel>()
                       .AddSingleton<IRecentAlbumsViewModel, RecentAlbumsViewModel>()
                       .AddSingleton<IRecentArtistsViewModel, RecentArtistsViewModel>()
                       .AddSingleton<IRecentPlaylistsViewModel, RecentPlaylistsViewModel>()
                       .AddSingleton<IRecentUserViewModel, RecentUserViewModel>()
                       .AddSingleton<ISearchUserViewModel, SearchUserViewModel>()
                       .AddSingleton<ISearchAlbumsViewModel, SearchAlbumsViewModel>()
                       .AddSingleton<ISearchArtistsViewModel, SearchArtistsViewModel>()
                       .AddSingleton<ISearchPlaylistsViewModel, SearchPlaylistsViewModel>()
                       .AddSingleton<ISearchLibraryViewModel, SearchLibraryViewModel>()
                       .AddTransient<ITrackViewModel, TrackViewModel>()
                       .AddTransient<ITrackListViewModel, TrackListViewModel>()
                       .AddTransient<IUserViewModel, UserViewModel>();
    }

    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        return services.AddSingleton<ArtistPage>()
                       .AddSingleton<AlbumPage>()
                       .AddSingleton<PlaylistPage>()
                       .AddSingleton<LibraryPage>()
                       .AddSingleton<ArtistPage>();
    }
}

