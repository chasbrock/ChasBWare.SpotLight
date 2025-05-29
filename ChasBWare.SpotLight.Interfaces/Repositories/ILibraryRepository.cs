using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ILibraryRepository
    {
        int AddPlaylists(List<Playlist> playlists);
        Playlist? FindPlaylist(string playlistId);
        string FindAllPlaylistsForTrack(string? trackId, string? priorTrack);
        HashSet<string> GetPlaylistIds();
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        bool RemovePlaylist(string playlistId);
        bool RemovePlaylists(IEnumerable<string> playlistIds);
        bool TransferPlaylistToLibrary(Playlist playlist, bool save);
        void UpdateLastAccessed(Playlist playlist);
    }
}