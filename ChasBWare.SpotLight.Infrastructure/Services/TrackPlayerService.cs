using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class TrackPlayerService : ITrackPlayerService
    {
        const int TimerIntervalMs = 1000;
       
        private DateTime _playBackStarted = DateTime.Now;
        private DateTime _playbackPaused = DateTime.Now;
        private PlayingTrack? _nowPlaying = null;
        private IServiceProvider _servicProvider;
        private readonly IMessageService<CurrentTrackChangedMessage> _currentTrackMessageService;
        private readonly IDispatcherTimer _timer;
  
        public TrackPlayerService(IServiceProvider serviceProvider,
                                  IDispatcher dispatcher,
                                  IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
        {
            _servicProvider = serviceProvider;
            _currentTrackMessageService = currentTrackChangedMessage;
            _timer = dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TimerIntervalMs);
            _timer.Tick += OnTimerTick;
        }

        public event EventHandler<TrackProgressMessageArgs>? OnTrackProgress;

        public async void StartPlaylist(IPlaylistViewModel playlist, int trackNumber)
        {
         	var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                await playerController.StartPlayback(playlist.Uri, trackNumber);
                _playBackStarted = DateTime.Now;

                var track = playlist.TracksViewModel.Items[trackNumber].Track;
                if (track.Id == null) 
                {
                    return;
                }

                var nowPlaying = new PlayingTrack { TrackId = track.Id, Track = track, ProgressMs = 0 };
                ChangePlayingTrack(nowPlaying);
            }
        }

        public async void Pause()
        {
            _timer.Stop();
            _playbackPaused = DateTime.Now;
            if (_nowPlaying != null)
            {
                _nowPlaying.Track.Status = TrackStatus.Paused;
            }

            var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                await playerController.PausePlayback();
            }
        }

        public async void Resume()
        {
            if (_nowPlaying == null) 
            {
                await UpdateNowPlaying();
            }

            if (_nowPlaying != null)
            {
                var progress = (int)(_playbackPaused - _playBackStarted).TotalMilliseconds;
                _playBackStarted = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(progress));
                _timer.Start();

                var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
                _nowPlaying.Track.Status = TrackStatus.Playing;
                if (playerController != null)
                {
                    await playerController.ResumePlayback(_nowPlaying.Track.Uri, progress);
                }
            }
        }

        public async void SkipForward()
        {
            var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                var nowPlaying = await playerController.SkipNext();
                if (nowPlaying != null)
                {
                    UpdateNowPlaying(nowPlaying);
                }
            }
        }

        public async void SkipBackward()
        {
            var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                var nowPlaying = await playerController.SkipPrevious();
                if (nowPlaying != null)
                {
                    UpdateNowPlaying(nowPlaying);
                }
            }
        }

        public async void SyncToDevice()
        {
            await UpdateNowPlaying();
        }

        private async void OnTimerTick(object? sender, EventArgs e)
        {
            if (_nowPlaying != null)
            {
                var progress = (int)(DateTime.Now - _playBackStarted).TotalMilliseconds;

                //have we finished playing track?
                if (progress > _nowPlaying.Track.Duration)
                {
                    await UpdateNowPlaying();
                }
                else
                {
                    UpdateTrackProgress(_nowPlaying, progress);
                }
            }
            else
            {
                _timer.Stop();
            }
        }

        private void UpdateNowPlaying(PlayingTrack nowPlaying)
        {
            _playBackStarted = DateTime.Now.AddMilliseconds(-nowPlaying.ProgressMs);
            if (nowPlaying.Track.Status == TrackStatus.Playing)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }

            ChangePlayingTrack(nowPlaying);
            UpdateTrackProgress(nowPlaying, nowPlaying.ProgressMs);
        }
   		
   		private async Task UpdateNowPlaying()
        {
            var playerController = _servicProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                var nowPlaying = await playerController.GetCurrentPlayingTrack();
                if (nowPlaying != null)
                {
                    UpdateNowPlaying(nowPlaying);
                }
            }
        }

        // this is called on ui thread because we are using DispatchTimer
        private void UpdateTrackProgress(PlayingTrack nowPlaying, int progress)
        {
            int progressPercent = 100 * progress / nowPlaying.Track.Duration;
            var progressText = $"{progress.MSecsToMinsSecs()} / {nowPlaying.Track.Duration.MSecsToMinsSecs()}";
            OnTrackProgress?.Invoke(this, new TrackProgressMessageArgs(nowPlaying.Track.Name, nowPlaying.Track.Artists, 
                                                                       progressPercent, progressText, nowPlaying.Track.Status));
        }

        private void ChangePlayingTrack(PlayingTrack nowPlaying)
        {
            // clean up last track
            if (_nowPlaying != null)
            {
                _currentTrackMessageService.SendMessage(new CurrentTrackChangedMessage(_nowPlaying.TrackId, _nowPlaying.Track.Album, TrackStatus.NotPlaying));
            }

            _nowPlaying = nowPlaying;
            if (_nowPlaying != null)
            {
                
                _nowPlaying.Track.Status = TrackStatus.Playing;
                _currentTrackMessageService.SendMessage(new CurrentTrackChangedMessage(_nowPlaying.TrackId, _nowPlaying.Track.Album, TrackStatus.Playing));
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
