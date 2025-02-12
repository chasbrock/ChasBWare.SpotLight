using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    internal static class SpotifyErrorCatcher
    {
        public static TReturn Execute<TReturn>(ISpotifyConnectionManager spotifyConnectionManager,
                                               Func<SpotifyClient, TReturn> body)
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    var client = spotifyConnectionManager.GetClient();
                    return body(client);
                }
                catch (Exception ex) 
                {
                    if (ex.Message.Contains("The access token expired"))
                    {
                        spotifyConnectionManager.Status = ConnectionStatus.TokenExpired;
                        continue;
                    }
                    retry = false;
                }
            }
            throw new Exception("Unexpectedly failed to run Spotify command!");
        }
    }
}
