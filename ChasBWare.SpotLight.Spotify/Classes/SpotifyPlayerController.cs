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
        public async Task<PlayingTrack?> SkipNext()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipNext();
                Thread.Sleep(100);
                var currentlPlaying = await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
                
                return currentlPlaying?.CopyToPlayingTrack();
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
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipPrevious();
                Thread.Sleep(100);
                var currentlPlaying = await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
                return currentlPlaying?.CopyToPlayingTrack();
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
        
        public async Task<bool> StartPlayback(string playlistUri, int trackOffset)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                PlayerResumePlaybackRequest request = new()
                {
                    ContextUri = playlistUri,
                    OffsetParam = new PlayerResumePlaybackRequest.Offset { Position = trackOffset }

                };
                return await client.Player.ResumePlayback(request);
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to start playback");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<bool> ResumePlayback(string trackUri, int position)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                PlayerResumePlaybackRequest request = new()
                {
                    ContextUri = trackUri,
                    PositionMs = position,

                };
                return await client.Player.ResumePlayback(request);
            }
            catch (APIException apiEx) 
            {
                _logger.LogError(apiEx, "failed to resume playback");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<bool> PausePlayback()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                return await client.Player.PausePlayback();
            }
            catch (APIException apiEx)
            {
                _logger.LogError(apiEx, "failed to pause playback");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<bool> TransferPlayback(IList<string> deviceIds, bool play)
        {
            var client = await _spotifyConnectionManager.GetClient();
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
            var client = await _spotifyConnectionManager.GetClient();
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
            var client = await _spotifyConnectionManager.GetClient();
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
            var attempts = 3;
            while (attempts -- > 0)
            {
                var client = await _spotifyConnectionManager.GetClient();
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
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Failed to connect to access user");
                    throw;
                }
            }
            throw new Exception("failed to access spotify");
        }

    
    }
}
