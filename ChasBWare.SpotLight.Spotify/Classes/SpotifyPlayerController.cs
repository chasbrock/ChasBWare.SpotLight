using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    public class SpotifyPlayerController(ILogger _logger,
                                         ISpotifyConnectionManager _spotifyConnectionManager) 
               : ISpotifyPlayerController
    {

        private async static Task<PlayingTrack?> GetCurrentlyPlaying(SpotifyClient client) 
        {
            Thread.Sleep(300);
            var currentlPlaying = await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
            return currentlPlaying?.CopyToPlayingTrack();
        }

        public async Task<PlayingTrack?> SkipNext()
        {
            SpotifyClient client = _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipNext();
                return await GetCurrentlyPlaying(client);
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to skip previous");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<PlayingTrack?> SkipPrevious()
        {
            var client =  _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipPrevious();
                return await GetCurrentlyPlaying(client);
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to skip previous");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }
        
        public async Task<PlayingTrack?> StartPlayback(string playlistUri, int trackOffset)
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                PlayerResumePlaybackRequest request = new()
                {
                    ContextUri = playlistUri,
                    OffsetParam = new PlayerResumePlaybackRequest.Offset { Position = trackOffset }

                };
                await client.Player.ResumePlayback(request);
                return await GetCurrentlyPlaying(client);
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to start playback");
                 return await GetCurrentlyPlaying(client); ;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<PlayingTrack?> ResumePlayback(string trackUri, int position)
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                PlayerResumePlaybackRequest request = new()
                {
                    ContextUri = trackUri,
                    PositionMs = position,

                };
                await client.Player.ResumePlayback(request);
                return await GetCurrentlyPlaying(client);
            }
            catch (APIException apiEx) 
            {
                _logger.LogError(apiEx, "failed to resume playback");
                return await GetCurrentlyPlaying(client);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<PlayingTrack?> PausePlayback()
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.PausePlayback();
                return await GetCurrentlyPlaying(client);
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to pause playback");
                return await GetCurrentlyPlaying(client);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<bool> TransferPlayback(IList<string> deviceIds, bool play)
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                var request = new PlayerTransferPlaybackRequest(deviceIds) { Play = play };
                return await client.Player.TransferPlayback(request);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }
               
        public async Task<Tuple<Track,List<Track>>> GetQueue()
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                var queue = await client.Player.GetQueue();
                var current = queue.CurrentlyPlaying.CopyToTrack();
                var tracks = queue.Queue.Select(pi => pi.CopyToTrack()).ToList();
                return new Tuple<Track, List<Track>>(current, tracks);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<bool> SetVolume(int volume)
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                var request = new PlayerVolumeRequest(volume);
                return await client.Player.SetVolume(request);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<PlayingTrack?> GetCurrentPlayingTrack()
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                var currentlyPlaying = await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
                return currentlyPlaying.CopyToPlayingTrack();
            }
            catch (APIUnauthorizedException)
            {
                _spotifyConnectionManager.Status = ConnectionStatus.TokenExpired;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogCritical(httpEx, "Failed to connect tto server");
                _spotifyConnectionManager.Status = ConnectionStatus.NotConnected;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
            return null;
        }

        public async Task<bool> SetDeviceAsActive(string deviceId)
        {
            var client = _spotifyConnectionManager.GetClient();
            try
            {
                var request = new PlayerTransferPlaybackRequest([deviceId]);
                return await client.Player.TransferPlayback(request);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }
    }
}
