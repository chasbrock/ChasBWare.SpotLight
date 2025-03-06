using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
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
        private PlayingTrack? _nowPlaying = null;
     
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
                UpdateNowPlaying(await _playerController.StartPlayback(playlist.Uri, trackNumber));
            }
        }

        public async void Pause()
        {
            _timer.Stop();
            PostPlayingTrackChange(TrackStatus.Paused);
            UpdateNowPlaying(await _playerController.PausePlayback());
        }

        public async void Resume()
        {
            // check to seee it anyone else has changed track / resumed playback
            var currentlyPlaying = await GetCurrentlyPlaying();
            if (currentlyPlaying != null && currentlyPlaying.Id == _nowPlaying?.Id)
            {
                currentlyPlaying = await _playerController.ResumePlayback(_nowPlaying.Uri, (int)_nowPlaying.Progress.TotalMilliseconds);
            }

            UpdateNowPlaying(currentlyPlaying);
        }

        public async void SkipForward()
        {
            UpdateNowPlaying(await _playerController.SkipNext());
        }

        public async void SkipBackward()
        {
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
                _timer.Stop();
                PostPlayingTrackChange(TrackStatus.NotPlaying);
                return;
            }

            // do we hate this track?
            if (_hatedService.GetIsHated(nowPlaying.Id))
            {
                SkipForward();
                return;
            }

            // alert any playlists viewing the current track
            if (_nowPlaying != nowPlaying)
            {
                PostPlayingTrackChange(TrackStatus.NotPlaying);
            }

            _nowPlaying = nowPlaying;
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

        public void AddTrackToQueue(string trackId)
        {
            // TODO
        }

        public void AddPlaylistToQueue(string playListId)
        {
            // todo
        }

        private void OnConnectionStatusChange(ConnectionStatusChangedMessage message)
        {
            if (message.ConnectionStatus != ConnectionStatus.Connected) 
            {
                _timer.Stop();
                // if we disconnect we cannot resy
                _playbackStarted = null;
            }
        }

        private async void OnTimerTick(object? sender, EventArgs e)
        {
            if (_nowPlaying != null)
            {
                var progress = _playbackStarted==null ? TimeSpan.Zero : DateTime.Now - (DateTime)_playbackStarted;

                //have we finished playing track?
                if (progress > _nowPlaying.Duration)
                {
                    UpdateNowPlaying(await GetCurrentlyPlaying());
                }
                else
                {
                    UpdateTrackProgress(_nowPlaying);
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
            if (_nowPlaying != null)
            {
                _currentTrackMessageService.SendMessage(new CurrentTrackChangedMessage(_nowPlaying.Id, _nowPlaying.Album, status));
            }
        }
    }
}
