using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISearchItemRepository
    {
        bool AddArtist(Artist artist);
        bool AddPlaylist(Playlist playlist);
        bool AddUser(User user);
        List<Playlist> GetPlaylists(PlaylistType playlistType);
        List<Artist> GetArtists();
        List<User> GetUsers();
        bool RemoveArtists();
        bool RemoveArtist(string id);
        bool RemovePlaylist(string id);
        bool RemovePlaylists(PlaylistType playlistType);
        bool RemoveUser(string id);
        bool RemoveUsers();
    }
}