using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyPlaylistRepository 
    {
        List<Playlist> FindAlbums(string searchText);
        List<Playlist> FindPlaylists(string searchText);
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        bool SetPlaylistSaveStatus(string id, PlaylistType playlistType, bool save);
    }
}