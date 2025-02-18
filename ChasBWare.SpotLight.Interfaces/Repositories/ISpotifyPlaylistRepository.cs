using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyPlaylistRepository 
    {
        Playlist? FindPlaylist(string playlistId, PlaylistType playlistType);
        List<Playlist> SearchForAlbums(string searchText);
        List<Playlist> SearchForPlaylists(string searchText);
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        bool SetPlaylistSaveStatus(string id, PlaylistType playlistType, bool save);
    }
}