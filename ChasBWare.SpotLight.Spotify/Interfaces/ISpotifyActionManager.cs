using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Interfaces
{

    /// <summary>
    /// class that managed calls to spotify api
    /// </summary>
    public interface ISpotifyActionManager
    {
        string UserCountry { get; set; }

        /// <summary>
        /// get details of current user on local machine
        /// </summary>
        /// <returns></returns>
        Task<PrivateUser> GetUserDetails();

        /// <summary>
        /// async load playlist list for current user
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<FullPlaylist>> GetCurrentUsersPlaylists();

        /// <summary>
        /// async load playlist list for current user
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SavedAlbum>> GetCurrentUsersAlbums();

        /// <summary>
        /// async load tracks for playlist list
        /// </summary>
        /// <returns></returns>
        Task<List<FullTrack>> GetPlaylistTracks(string playlistId);

        /// <summary>
        /// async load tracks for album list
        /// </summary>
        /// <returns></returns>
        Task<List<SimpleTrack>> GetAlbumTracks(string albumId);

        /// <summary>
        /// async load albums  list for artist
        /// </summary>
        /// <returns></returns>
        Task<List<SimpleAlbum>> GetArtistAlbums(string artistId);

        /// <summary>
        /// async find all albums for artist
        /// </summary>
        /// <returns></returns>
        Task<List<FullArtist>> FindArtist(string artistName);

        /// <summary>
        /// async find all albums with this name
        /// </summary>
        /// <returns></returns>
        Task<List<SimpleAlbum>> FindAlbums(string albumName);
     
        /// <summary>
        /// async find all playlists with this name
        /// </summary>
        /// <returns></returns>
        Task<List<FullPlaylist>> FindPlaylists(string playlistName);

        /// <summary>
        /// gets all albums for artist
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<Paging<SimpleAlbum>> GetArtistAlbums(string artistId, int offset);

        /// <summary>
        /// gets all albums for artist
        /// </summary>
        /// <returns></returns>
        Task<List<FullTrack>> GetArtistTopTracks(string artistId);


     /*   
        /// <summary>
        /// remove album from saved lists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveSavedAlbum(string id);

        /// <summary>
        /// remove playlis from saved lists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveSavedPlaylist(string id);
     */
    }
}