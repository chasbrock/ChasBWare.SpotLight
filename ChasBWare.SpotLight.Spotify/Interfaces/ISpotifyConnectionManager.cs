using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Interfaces;

public interface ISpotifyConnectionManager
{
    /// <summary>
    /// gets or sets status of connection manager
    /// </summary>
    ConnectionStatus Status { get; set; }

    /// <summary>
    /// ensure that client is authorised before returning client
    /// </summary>
    /// <returns></returns>        
    SpotifyClient GetClient();
}