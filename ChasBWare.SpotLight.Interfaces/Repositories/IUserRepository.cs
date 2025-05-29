using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories;

public interface IUserRepository
{
    User? FindUser(string userId);

    /// <summary>
    /// load allablbums that are linked to album
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    List<Playlist> LoadUserAlbums(string userId);
    void StoreUserAndAlbums(User model, List<Playlist> albums);
    void UpdateLastAccessed(User user);
}
