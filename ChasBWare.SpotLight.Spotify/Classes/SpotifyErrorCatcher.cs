using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    internal static class SpotifyErrorCatcher
    {
        public static TReturn? Execute<TReturn>(ISpotifyConnectionManager connectionManager,
                                               Func<SpotifyClient, TReturn?> body)
        {
            int attempt = 2;
            while (attempt-- > 0)
            {
                try
                {
                    var client = connectionManager.GetClient();
                    return body(client);
                }
                catch (Exception ex)
                {
                    if (!ProcessException(connectionManager, ex))
                    {
                        return default;
                    }
                }
            }
            // not sure what error is so just disconnect
            //  connectionManager.Status = ConnectionStatus.NotConnected;
            return default;
        }

        public static bool ProcessException(ISpotifyConnectionManager spotifyConnectionManager, Exception ex)
        {
            var apiEx = ex as APIException;
            if (apiEx == null && ex.InnerException is APIException)
            {
                apiEx = ex.InnerException as APIException;
            }

            if (apiEx != null)
            {
                switch (apiEx.Message)
                {
                    case "The access token expired":
                        spotifyConnectionManager.Status = ConnectionStatus.TokenExpired;
                        return true;
                    case "Player command failed: No active device found":
                        return false;
                    default:
                        spotifyConnectionManager.Status = ConnectionStatus.NotConnected;
                        return false;
                }
            }
            spotifyConnectionManager.Status = ConnectionStatus.NotConnected;
            return false;
        }

    }
}
