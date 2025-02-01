using System;
using System.Collections.Generic;
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
       
        private DateTime _playbackStarted = DateTime.Now;
        private PlayingTrack? _nowPlaying = null;
        private IServiceProvider _serviceProvider;
        private readonly IMessageService<CurrentTrackChangedMessage> _currentTrackMessageService;
        private readonly IDispatcherTimer _timer;
  
        public TrackPlayerService(IServiceProvider serviceProvider,
                                  IDispatcher dispatcher,
                                  IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
        {
            _serviceProvider = serviceProvider;
            _currentTrackMessageService = currentTrackChangedMessage;
            _timer = dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TimerIntervalMs);
            _timer.Tick += OnTimerTick;
        }

        public event EventHandler<PlayingTrack>? OnTrackProgress;

        public async void StartPlaylist(IPlaylistViewModel playlist, int trackNumber)
        {
         	var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                UpdateNowPlaying(await playerController.StartPlayback(playlist.Uri, trackNumber));
            }
        }

        public async void Pause()
        {
            _timer.Stop();
            PostPlayingTrackChange(TrackStatus.Paused);
            
            var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                UpdateNowPlaying(await playerController.PausePlayback());
            }
        }

        public async void Resume()
        {
            // check to seee it anyone else has changed track / resumed playback
            var currentlyPlaying = await GetCurrentlyPlaying();
            if (currentlyPlaying != null && currentlyPlaying.Id == _nowPlaying?.Id) 
            {
                var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
                if (playerController != null)
                {
                    currentlyPlaying = await playerController.ResumePlayback(_nowPlaying.Uri, (int)_nowPlaying.Progress.TotalMilliseconds);
                }
            }

            UpdateNowPlaying(currentlyPlaying);
        }

        public async void SkipForward()
        {
            var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                UpdateNowPlaying(await playerController.SkipNext());
            }
        }

        public async void SkipBackward()
        {
            var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                var nowPlaying = await playerController.SkipPrevious();
                if (nowPlaying != null)
                {
                    UpdateNowPlaying(nowPlaying);
                }
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

        private async void OnTimerTick(object? sender, EventArgs e)
        {
            if (_nowPlaying != null)
            {
                var progress = DateTime.Now - _playbackStarted;

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
            var playerController = _serviceProvider.GetService<ISpotifyPlayerController>();
            if (playerController != null)
            {
                return await playerController.GetCurrentPlayingTrack();

            }
            return null;
        }

        private void UpdateTrackProgress(PlayingTrack nowPlaying)
        {
            nowPlaying.Progress = DateTime.Now - _playbackStarted;
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
