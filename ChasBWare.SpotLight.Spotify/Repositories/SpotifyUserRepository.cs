using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories;

public class SpotifyUserRepository(ISpotifyActionManager _actionManager)
           : ISpotifyUserRepository
{
    public User? FindUser(string userId)
    {
        if (_actionManager.Status != ConnectionStatus.Connected)
        {
            return null;
        }

        var found = _actionManager.FindUser(userId);
        if (found != null)
        {
            return found.CopyToUser();
        }
        return null;
    }

    public List<Playlist> LoadUserPlaylist(string userId)
    {
        if (_actionManager.Status != ConnectionStatus.Connected)
        {
            return [];
        }

        List<Playlist> playlists = [];
        var fullPlaylists = _actionManager.GetUserPlaylists(userId);
        if (fullPlaylists != null)
        {
            foreach (var playlist in fullPlaylists.Select(sa => sa.CopyToPlaylist()))
            {
                if (playlist != null)
                {
                    playlists.Add(playlist);
                }
            }
        }

        return playlists;
    }


}