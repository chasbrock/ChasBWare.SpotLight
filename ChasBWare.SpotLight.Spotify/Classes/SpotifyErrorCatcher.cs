using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    internal class SpotifyErrorCatcher<TReturn>(ISpotifyConnectionManager _spotifyConnectionManager)
    {
        private const int MaxTryCount = 2;

        public Task<TReturn> Execute(Func<SpotifyClient, Task<TReturn>> body)
        {
            var tryCount = 0;
            while (tryCount++ < MaxTryCount)
            {
                var client = _spotifyConnectionManager.GetClient().Result;
                try
                {
                    return body.Invoke(client);
                }
                catch (SpotifyAPI.Web.APIUnauthorizedException spex)
                {
                    if (spex.Message == "The access token expired")
                    {
                        _spotifyConnectionManager.Status = Domain.Enums.ConnectionStatus.TokenExpired;
                        continue;
                    }
                }
                catch (Exception ex) 
                {
                    throw;
                }
            }
            throw new Exception("Shit!");
        }

        public async Task<TReturn> Execute<TParam>(Func<TParam, Task<TReturn>> body, TParam value) 
        {
            var tryCount = 0;
            while (tryCount++ < MaxTryCount)
            {
                var client = await _spotifyConnectionManager.GetClient();
                try
                {
                    return await body.Invoke(value);
                }
                catch (SpotifyAPI.Web.APIUnauthorizedException spex)
                {
                    if (spex.Message == "Token Expired")
                    {
                        _spotifyConnectionManager.Status = Domain.Enums.ConnectionStatus.TokenExpired;
                        continue;
                    }
                }
            }
            throw new Exception("Shit!");
        }
    }
}
