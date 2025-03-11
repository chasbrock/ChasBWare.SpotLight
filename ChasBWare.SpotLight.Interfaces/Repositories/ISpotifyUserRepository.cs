using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories;

public interface ISpotifyUserRepository 
{
    /// <summary>
    /// find details of User by id
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>
    User? FindUser(string UserId);

    /// <summary>
    /// load allablbums that are linked to album
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>
    List<Playlist> LoadUserPlaylist(string UserId);
}