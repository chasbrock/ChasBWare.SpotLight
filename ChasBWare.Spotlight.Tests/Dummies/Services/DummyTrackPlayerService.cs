using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.Spotlight.Tests.Dummies.Services
{
    public class DummyTrackPlayerService : ITrackPlayerService
    {
        public event EventHandler<PlayingTrack>? OnTrackProgress;

        public PlayingTrack PlayingTrack { get; set; } = CreatePlayingTrack(0);

        
        public static PlayingTrack CreatePlayingTrack(int seed)
        {
            return new PlayingTrack
            {
                Id = $"id_{seed}",
                Name = $"name_{seed}",
                Artists = [new IdItem{ Id=$"artist_{seed}A",Name= $"artist_{seed}A" },
                           new IdItem{ Id=$"artist_{seed}B",Name= $"artist_{seed}B" }],
                Album = $"Album {seed}",
                Image = $"image url {seed}",
                Uri = $"track uri {seed}",
            };
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void SkipForward()
        {
            throw new NotImplementedException();
        }

        public void SkipBackward()
        {
            throw new NotImplementedException();
        }

        public void StartPlaylist(IPlaylistViewModel playlist, int trackNumber)
        {
            throw new NotImplementedException();
        }

        public void UpdateNowPlaying(PlayingTrack nowPlaying)
        {
            throw new NotImplementedException();
        }
    }
}
