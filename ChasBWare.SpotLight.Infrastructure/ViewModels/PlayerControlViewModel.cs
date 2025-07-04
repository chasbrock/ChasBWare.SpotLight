﻿using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class PlayerControlViewModel
           : Notifyable,
             IPlayerControlViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigator _navigator;
    private readonly IDispatcher _dispatcher;

    private bool _isPlaying = false;
    private string _trackName = "";
    private string _image = "";
    private string _albumId = "";
    private List<KeyValue> _artisList = [];
    private string _artists = "";
    private double _progressPercent = 0;
    private TimeSpan _playedTime = TimeSpan.Zero;
    private TimeSpan _duration = TimeSpan.Zero;

    public PlayerControlViewModel(IServiceProvider serviceProvider,
                                  INavigator navigator,
                                  IDispatcher dispatcher,
                                  ITrackPlayerService trackPlayerService,
                                  ICurrentDeviceViewModel currentDevice,
                                  IMessageService<PlayPlaylistMessage> playTracklistMessageService,
                                  IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                                  IMessageService<ConnectionStatusChangedMessage> connectionStatusService,
                                  IMessageService<AppActivationChanged> appActivationMessageService)
    {
        _serviceProvider = serviceProvider;
        _navigator = navigator;
        _dispatcher = dispatcher;
        CurrentDevice = currentDevice;

        TrackPlayerService = trackPlayerService;
        TrackPlayerService.OnTrackProgress += ShowProgress;

        BackCommand = new Command(SkipBack);
        PlayCommand = new Command(Play);
        PauseCommand = new Command(Pause);
        ForwardCommand = new Command(SkipForward);
        ResyncCommand = new Command(SyncToDevice);
        OpenDevicesPopupCommand = new Command(NavigateToDevices);
        OpenArtistCommand = new Command<string>(id => NavigateToArtist(id));
        OpenAlbumCommand = new Command<string>(id => NavigateToAlbum(id));

        activeItemChangedMessageService.Register(OnSetCurrentDevice);
        connectionStatusService.Register(OnConnectionStatusChange);
        playTracklistMessageService.Register(OnPlayTracklist);
        appActivationMessageService.Register(OnAppActivationStateChanged);
    }


    public ICommand BackCommand { get; }
    public ICommand PlayCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand ForwardCommand { get; }

    public ICommand OpenDevicesPopupCommand { get; }
    public ICommand OpenArtistCommand { get; }
    public ICommand OpenAlbumCommand { get; }
    public ICommand ResyncCommand { get; }

    public ICurrentDeviceViewModel CurrentDevice { get; }
    public ITrackPlayerService TrackPlayerService { get; }

    public bool IsSyncing { get; set; }

    public ConnectionStatus ConnectionStatus { get; private set; } = ConnectionStatus.NotInitialised;

    public string? CurrentTrackId { get; private set; }

    public TrackStatus TrackStatus
    {
        get => CurrentTrackId == null ? TrackStatus.NotPlaying : IsPlaying ? TrackStatus.Playing : TrackStatus.Paused;
    }

    public string TrackName
    {
        get => _trackName;
        set => SetField(ref _trackName, value);
    }

    public string Artists
    {
        get => _artists;
        set => SetField(ref _artists, value);
    }

    public List<KeyValue> ArtistList
    {
        get => _artisList;
        set => SetField(ref _artisList, value);
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (SetField(ref _isPlaying, value))
            {
                Notify(nameof(IsPaused));
            }
        }
    }

    public bool IsPaused
    {
        get => !_isPlaying;
    }

    public double ProgressPercent
    {
        get => _progressPercent;
        set => SetField(ref _progressPercent, value);
    }

    public TimeSpan PlayedTime
    {
        get => _playedTime;
        set => SetField(ref _playedTime, value);
    }

    public TimeSpan Duration
    {
        get => _duration;
        set => SetField(ref _duration, value);
    }

    public string Image
    {
        get => _image;
        set => SetField(ref _image, value);
    }

    public string AlbumId
    {
        get => _albumId;
        set => SetField(ref _albumId, value);
    }

    private void OnPlayTracklist(PlayPlaylistMessage message)
    {
        TrackPlayerService.StartPlaylist(message.Playlist,
                                         message.Offset);
    }

    private void OnConnectionStatusChange(ConnectionStatusChangedMessage message)
    {
        ConnectionStatus = message.ConnectionStatus;
        _dispatcher.Dispatch(() =>
        {
            if (ConnectionStatus.IsActiveState())
            {
                SyncToDevice();
            }
            else
            {
                CurrentDevice.IsActive = false;
            }
        });
    }

    private void OnSetCurrentDevice(ActiveItemChangedMessage message)
    {
        if (message.PageType == PageType.Devices)
        {
            CurrentDevice.Device = message.Model as DeviceModel ?? DeviceHelper.GetLocalDevice();
        }
    }

    private void OnAppActivationStateChanged(AppActivationChanged message)
    {
        if (message.IsActive && ConnectionStatus.IsActiveState())
        {
            SyncToDevice();
        }
    }

    private void SkipForward()
    {
        TrackPlayerService.SkipForward();
    }

    private void Pause()
    {
        TrackPlayerService.Pause();
        IsPlaying = false;
    }

    private void Play()
    {
        TrackPlayerService.Resume();
    }

    private void SkipBack()
    {
        TrackPlayerService.SkipBackward();
    }

    private void SyncToDevice()
    {
        if (!IsSyncing)
        {
            IsSyncing = true;
            var task = _serviceProvider.GetRequiredService<ISyncToDeviceTask>();
            task.Execute(this);
        }
    }

    private void NavigateToDevices()
    {
        _navigator.NavigateTo(PageType.Devices);
    }

    private void NavigateToArtist(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }

        var messageService = _serviceProvider.GetRequiredService<IMessageService<FindItemMessage>>();
        messageService.SendMessage(new FindItemMessage(PageType.Artists, id));

        _navigator.NavigateTo(PageType.Artists);
    }

    private void NavigateToAlbum(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }

        var messageService = _serviceProvider.GetRequiredService<IMessageService<FindItemMessage>>();
        if (messageService.SendMessage(new FindItemMessage(PageType.Library, id)))
        {
            _navigator.NavigateTo(PageType.Library);
        }
        else
        {
            messageService.SendMessage(new FindItemMessage(PageType.Albums, id));
            _navigator.NavigateTo(PageType.Albums);
        }
    }

    private void ShowProgress(object? sender, PlayingTrack playingTrack)
    {
        if (CurrentTrackId != playingTrack.Id)
        {
            CurrentTrackId = playingTrack.Id;
            TrackName = playingTrack.Name;
            Artists = string.Join(',', playingTrack.Artists.Select(a => a.Key));
            ArtistList = playingTrack.Artists;
            Duration = playingTrack.Duration;
            Image = playingTrack.Image ?? "";
            AlbumId = playingTrack.AlbumId;
        }

        ProgressPercent = playingTrack.Progress.TotalMilliseconds / playingTrack.Duration.TotalMilliseconds;
        PlayedTime = playingTrack.Progress;
        IsPlaying = playingTrack.IsPlaying;
    }

}
