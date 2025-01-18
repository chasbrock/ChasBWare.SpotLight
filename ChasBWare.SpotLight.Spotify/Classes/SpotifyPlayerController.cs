using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    public class SpotifyPlayerController(ILogger _logger,
                                         ISpotifyConnectionManager _spotifyConnectionManager) 
               : ISpotifyPlayerController
    {
        public async Task<CurrentlyPlaying> SkipNext()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipNext();
                Thread.Sleep(100);
                return await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
               
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<CurrentlyPlaying> SkipPrevious()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                await client.Player.SkipPrevious();
                Thread.Sleep(100);
                return await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
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

        public async Task<List<Device>> GetAvailableDevices()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var devicesResponse = await client.Player.GetAvailableDevices();
                return devicesResponse != null ? devicesResponse.Devices : [];
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<CurrentlyPlayingContext> GetCurrentPlayback()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                return await client.Player.GetCurrentPlayback();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to access user");
                throw;
            }
        }

        public async Task<QueueResponse> GetQueue()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                return await client.Player.GetQueue();
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

        public async Task<CurrentlyPlaying> GetCurrentPlayingTrack()
        {
            var attempts = 3;
            while (attempts -- > 0)
            {
                var client = await _spotifyConnectionManager.GetClient();
                try
                {
                    return await client.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
                }
                catch (APIUnauthorizedException)
                {
                    _spotifyConnectionManager.Status = ConnectionStatus.TokenExpired;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Failed to connect to access user");
                    throw;
                }
            }
            throw new Exception("failed to access spotify");
        }

        public async Task<bool> SetDeviceAsActive(string deviceId)
        {
            var client = await _spotifyConnectionManager.GetClient();
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
