using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ILibraryRepository 
    {
        int AddPlaylists(List<Playlist> playlists);
        Playlist? FindPlaylist(string playlistId);
        HashSet<string> GetPlaylistIds();
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        bool RemovePlaylist(string playlistId);
        bool TransferPlaylistToLibrary(Playlist playlist, bool save);
        void UpdateLastAccessed(Playlist playlist);
    }
}