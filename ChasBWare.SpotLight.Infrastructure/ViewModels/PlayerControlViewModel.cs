using System.Diagnostics;
using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
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
        private readonly ITrackPlayerService _trackPlayerService;
        
        private bool _isPlaying = false;
        private string _currentTrack = "";
        private string _currentArtist = "";
        private double _progressPercent = 0;
        private string _progressText = "";
        private IDeviceViewModel _currentDevice = new DeviceViewModel();
              
        public PlayerControlViewModel(IServiceProvider serviceProvider,
                                      INavigator navigator,
                                      ITrackPlayerService trackPlayerService,
                                      IMessageService<PlayTracklistMessage> playTracklistMessageService,
                                      IMessageService<ActiveDeviceChangedMessage> activeDeviceChangedMessageService,
                                      IMessageService<ConnectionStatusChangedMessage> connectionStatusService)
        {
            _serviceProvider = serviceProvider;   
            _navigator = navigator;
            _trackPlayerService = trackPlayerService;
            _trackPlayerService.OnTrackProgress += ShowProgress;
            
            BackCommand = new Command(SkipBack);
            PlayCommand = new Command(Play);
            PauseCommand = new Command(Pause);
            ForwardCommand = new Command(SkipForward);
            ResyncCommand = new Command(Resync);
            OpenDevicesPopupCommand = new Command(NavigateToDevices);
 
            activeDeviceChangedMessageService.Register(SetCurrentDevice);
            connectionStatusService.Register(ConnectionStatusChange);
            playTracklistMessageService.Register(PlayTracklist);
        }

     
        public ICommand BackCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand OpenDevicesPopupCommand { get; private set; }
        public ICommand ResyncCommand { get; private set; }

        public IDeviceViewModel CurrentDevice 
        {
            get => _currentDevice;
            set
            {
                if (SetField(ref _currentDevice, value))
                {
                    if (CurrentDevice != null && CurrentDevice.IsActive)
                    {
                        _trackPlayerService.SyncToDevice();
                    }
                    NotifyAll();
                }
            }
        } 

        public string CurrentTrack
        {
            get => _currentTrack;
            set => SetField(ref _currentTrack, value);
        }

        public string CurrentArtist
        {
            get => _currentArtist;
            set => SetField(ref _currentArtist, value);
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

        public string ProgressText
        {
            get => _progressText;
            set => SetField(ref _progressText, value);
        }

        private void PlayTracklist(PlayTracklistMessage message)
        {
            _trackPlayerService.StartPlaylist(message.Payload.Playlist,
                                              message.Payload.Offset); 
        }

        private void ConnectionStatusChange(ConnectionStatusChangedMessage message)
        {
            if (message.Payload.ConnectionStatus == ConnectionStatus.Connected)
            {
                var task = _serviceProvider.GetService<IGetActiveDeviceTask>();
                task?.Execute(this);
            }
        }


        private void SetCurrentDevice(ActiveDeviceChangedMessage message)
        {
            CurrentDevice = message.Payload;
        }

        private void SkipForward()
        {
            _trackPlayerService.SkipForward();
        }

        private void Pause()
        {
            _trackPlayerService.Pause();
            IsPlaying = false;
        }

        private void Play()
        {
           _trackPlayerService.Resume();
        }

        private void SkipBack()
        {
           _trackPlayerService.SkipBackward();
        }

        private void Resync(object obj)
        {
            _trackPlayerService.SyncToDevice();
        }

        private void NavigateToDevices()
        {
            if (String.IsNullOrEmpty(_currentDevice.Model.Id))
            {
                var task = _serviceProvider.GetService<IGetActiveDeviceTask>();
                task?.Execute(this);
            }
            else
            {
                _navigator.NavigateTo("//Devices");
            }
        }

        private void ShowProgress(object? sender, TrackProgressMessageArgs e)
        {
            CurrentTrack = e.TrackName;
            CurrentArtist = e.Artist;
            ProgressPercent = (double)e.ProgressPercent/100;
            ProgressText = e.ProgressText;
            IsPlaying = e.Status == TrackStatus.Playing;
        }

    }
}
