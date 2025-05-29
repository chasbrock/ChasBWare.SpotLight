using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class TrackPlayerService : ITrackPlayerService
    {
        const int TimerIntervalMs = 1000;

        private readonly IMessageService<CurrentTrackChangedMessage> _currentTrackMessageService;
        private readonly IDispatcherTimer _timer;
        private readonly IHatedService _hatedService;
        private readonly ISpotifyPlayerController _playerController;
        
        private DateTime? _playbackStarted = null;
        
        public PlayingTrack? CurrentTrack { get; private set; }
        public PlayingTrack? PriorTrack { get; private set; }
        public string? CurrentPlaylistId { get; set; }

        public TrackPlayerService(IDispatcher dispatcher,
                                  IHatedService hatedService,
                                  ISpotifyPlayerController playerController,
                                  IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage,
                                  IMessageService<ConnectionStatusChangedMessage> connectionStatusService)
        {
            _currentTrackMessageService = currentTrackChangedMessage;
            _hatedService = hatedService;
            _playerController = playerController;
            connectionStatusService.Register(OnConnectionStatusChange);

            _timer = dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TimerIntervalMs);
            _timer.Tick += OnTimerTick;
        }

        public event EventHandler<PlayingTrack>? OnTrackProgress;

        public async void StartPlaylist(Playlist playlist, int trackNumber)
        {
            if (!string.IsNullOrWhiteSpace(playlist.Uri))
            {
                PriorTrack = null;
                CurrentPlaylistId = playlist.Id;
                var nowPlaying = await _playerController.StartPlayback(playlist.Uri, trackNumber);
                UpdateNowPlaying(nowPlaying);
            }
            else
            {
                CurrentPlaylistId = null;
            }
        }

        public async void Pause()
        {
            _timer.Stop();
            PostPlayingTrackChange(TrackStatus.Paused);
            var nowPlaying = await _playerController.PausePlayback();
            UpdateNowPlaying(nowPlaying);
        }

        public async void Resume()
        {
            // check to seee it anyone else has changed track / resumed playback
            var nowPlaying = await GetCurrentlyPlaying();
            if (nowPlaying != null && nowPlaying.Id == CurrentTrack?.Id)
            {
                nowPlaying = await _playerController.ResumePlayback(CurrentTrack.Uri, (int)CurrentTrack.Progress.TotalMilliseconds);
            }

            UpdateNowPlaying(nowPlaying);
        }

        public async void SkipForward()
        {
            PriorTrack = CurrentTrack;
            var nowPlaying = await _playerController.SkipNext();
            UpdateNowPlaying(nowPlaying);
        }

        public async void SkipBackward()
        {
            PriorTrack = null;
            var nowPlaying = await _playerController.SkipPrevious();
            if (nowPlaying != null)
            {
                UpdateNowPlaying(nowPlaying);
            }
        }

        public void UpdateNowPlaying(PlayingTrack? nowPlaying)
        {
            if (nowPlaying == null)
            {
                CurrentPlaylistId = null;
                _timer.Stop();
                PostPlayingTrackChange(TrackStatus.NotPlaying);
                return;
            }

            // do we hate this track?
            if (_hatedService.GetIsHated(nowPlaying.Id))
            {
                PriorTrack = nowPlaying; 
                SkipForward();
                return;
            }

            // alert any playlists viewing the current track
            // that it has stopped
            if (CurrentTrack?.Id != nowPlaying.Id)
            {
                PriorTrack = CurrentTrack;
                PostPlayingTrackChange(TrackStatus.NotPlaying);
            }


            CurrentTrack = nowPlaying;
            _playbackStarted = DateTime.Now - nowPlaying.Progress;
            if (nowPlaying.IsPlaying)
            {
                PostPlayingTrackChange(TrackStatus.Playing);
                _timer.Start();
            }
            else
            {
                PostPlayingTrackChange(TrackStatus.Paused);
                _timer.Stop();
            }

            UpdateTrackProgress(nowPlaying);
        }

        private void OnConnectionStatusChange(ConnectionStatusChangedMessage message)
        {
            if (message.ConnectionStatus.IsActiveState())
            {
                _timer.Stop();
                // if we disconnect we cannot resy
                _playbackStarted = null;
            }
        }

        private async void OnTimerTick(object? sender, EventArgs e)
        {
            if (CurrentTrack != null)
            {
                var progress = _playbackStarted == null ? TimeSpan.Zero : DateTime.Now - (DateTime)_playbackStarted;

                //have we finished playing track?
                if (progress > CurrentTrack.Duration)
                {
                    UpdateNowPlaying(await GetCurrentlyPlaying());
                }
                else
                {
                    UpdateTrackProgress(CurrentTrack);
                }
            }
            else
            {
                _timer.Stop();
            }
        }

        private async Task<PlayingTrack?> GetCurrentlyPlaying()
        {
            return await _playerController.GetCurrentPlayingTrack();
        }

        private void UpdateTrackProgress(PlayingTrack nowPlaying)
        {
            TimeSpan progress = _playbackStarted == null ? TimeSpan.Zero : DateTime.Now - (DateTime)_playbackStarted;
            nowPlaying.Progress = progress;
            OnTrackProgress?.Invoke(this, nowPlaying);
        }

        private void PostPlayingTrackChange(TrackStatus status)
        {
            if (CurrentTrack != null)
            {
                _currentTrackMessageService.SendMessage(new CurrentTrackChangedMessage(CurrentTrack.Id, CurrentTrack.Album, status));
            }
        }
    }
}
