using System.Diagnostics;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class PlayerControlViewModel 
               : Notifyable, 
                 IPlayerControlViewModel 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigator _navigator;

        private string? _currentTrackId;
        private bool _isPlaying = false;
        private string _currentTrack = "";
        private string _image = "";
        private string _albumId = "";
        private List<IdItem> _artisList = [];
        private string _artists = "";
        private double _progressPercent = 0;
        private TimeSpan _playedTime = TimeSpan.Zero;
        private TimeSpan _duration = TimeSpan.Zero;
        private bool _firstCallToNavigate = true;

        public PlayerControlViewModel(IServiceProvider serviceProvider,
                                      INavigator navigator,
                                      ITrackPlayerService trackPlayerService,
                                      ICurrentDeviceViewModel currentDevice,
                                      IMessageService<PlayPlaylistMessage> playTracklistMessageService,
                                      IMessageService<ActiveDeviceChangedMessage> activeDeviceChangedMessageService,
                                      IMessageService<ConnectionStatusChangedMessage> connectionStatusService)
        {
            _serviceProvider = serviceProvider;   
            _navigator = navigator;
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

            activeDeviceChangedMessageService.Register(SetCurrentDevice);
            connectionStatusService.Register(ConnectionStatusChange);
            playTracklistMessageService.Register(PlayTracklist);
        }

        public ICommand BackCommand { get;  }
        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get;  }
        public ICommand ForwardCommand { get;  }
        
        public ICommand OpenDevicesPopupCommand { get; }
        public ICommand OpenArtistCommand { get; }
        public ICommand OpenAlbumCommand { get; }
        public ICommand ResyncCommand { get; }

        public ICurrentDeviceViewModel CurrentDevice { get; }
        public ITrackPlayerService TrackPlayerService { get; }

        public string CurrentTrack
        {
            get => _currentTrack;
            set => SetField(ref _currentTrack, value);
        }

        public string Artists
        {
            get => _artists;
            set => SetField(ref _artists, value);
        }
        
        public List<IdItem> ArtistList 
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

        private void PlayTracklist(PlayPlaylistMessage message)
        {
            TrackPlayerService.StartPlaylist(message.Payload.Playlist,
                                              message.Payload.Offset); 
        }

        private void ConnectionStatusChange(ConnectionStatusChangedMessage message)
        {
            if (message.Payload.ConnectionStatus == ConnectionStatus.Connected)
            {
                SyncToDevice();
            }
        }

        private void SetCurrentDevice(ActiveDeviceChangedMessage message)
        {
            CurrentDevice.Device = message.Payload;
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
            var task = _serviceProvider.GetService<ISyncToDeviceTask>();
            task?.Execute(this);
        }

        private void NavigateToDevices()
        {
            if (_firstCallToNavigate)
            {
                SyncToDevice();
                _firstCallToNavigate = false;
            }
            else
            {
                _navigator.NavigateTo(PageType.Devices);
            }
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
            messageService.SendMessage(new FindItemMessage(PageType.Albums, id));

            _navigator.NavigateTo(PageType.Albums);
        }

        private void ShowProgress(object? sender, PlayingTrack playingTrack)
        {
            if (_currentTrackId != playingTrack.Id) 
            {
                _currentTrackId = playingTrack.Id;
                CurrentTrack = playingTrack.Name;
                Artists = string.Join(',', playingTrack.Artists.Select(a => a.Name));
                ArtistList = playingTrack.Artists;
                Duration = playingTrack.Duration;
                Image = playingTrack.Image ?? "";
            }

            ProgressPercent = playingTrack.Progress.TotalMilliseconds / playingTrack.Duration.TotalMilliseconds ;
            PlayedTime = playingTrack.Progress;
            IsPlaying = playingTrack.IsPlaying;
        }

    }
}
