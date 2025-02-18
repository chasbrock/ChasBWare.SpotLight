using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ILibraryRepository 
    {
        int AddPlaylists(List<Playlist> playlists);
        Playlist? FindPlaylist(string playlistId);
        bool RemovePlaylist(string playlistId);
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        void UpdateLastAccessed(Playlist playlist);
        bool TransferPlaylistToLibrary(Playlist playlist, bool save);
    }
}