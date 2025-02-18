using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Interfaces;

public interface ISpotifyConnectionManager
{
    /// <summary>
    /// update status, message can be used to alert user of error condition
    /// </summary>
    /// <param name="status"></param>
    /// <param name="message">optional message</param>
    void SetStatus(ConnectionStatus status, string? message = null);

    /// <summary>
    /// gets status of connection manager
    /// </summary>
    ConnectionStatus Status { get;}

    /// <summary>
    /// ensure that client is authorised before returning client
    /// </summary>
    /// <returns></returns>        
    SpotifyClient GetClient();
}