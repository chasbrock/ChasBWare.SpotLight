using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISearchItemRepository
    {
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        List<Artist> GetArtists();
        bool AddArtist(Artist artist);
        bool RemoveArtists();
        bool RemoveArtist(string id);
        bool RemovePlaylist(string id);
        bool RemovePlaylists(PlaylistType playlistType);
        bool AddPlaylist(Playlist playlist);
    }
}