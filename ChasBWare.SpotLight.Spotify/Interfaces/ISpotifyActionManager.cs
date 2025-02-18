using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;
using SpotifyDevice = SpotifyAPI.Web.Device;

namespace ChasBWare.SpotLight.Spotify.Interfaces
{
    /// <summary>
    /// class that managed calls to spotify api
    /// </summary>
    public interface ISpotifyActionManager
    {
        /// <summary>
        /// find album details from id
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        FullAlbum? FindAlbum(string albumId);

        /// <summary>
        /// find artist details from id
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        FullArtist? FindArtist(string artistId);

        /// <summary>
        /// find artist details from id
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        FullPlaylist? FindPlaylist(string playlistId);

        /// <summary>
        /// gets all albums for artist
        /// </summary>
        /// <returns></returns>
        List<FullTrack>? GetArtistTopTracks(string artistId, string userCountry);
 
        /// <summary>
        /// async load tracks for album list
        /// </summary>
        /// <returns></returns>
        List<SimpleTrack>? GetAlbumTracks(string albumId);

        /// <summary>
        /// async load albums  list for artist
        /// </summary>
        /// <returns></returns>
        List<SimpleAlbum>? GetArtistAlbums(string artistId);

        /// <summary>
        /// get list of available devices
        /// </summary>
        /// <returns></returns>
        List<SpotifyDevice>? GetAvailableDevices();

        /// <summary>
        /// retreave deatail of current play / playing track etc
        /// </summary>
        /// <returns></returns>
        CurrentlyPlayingContext? GetCurrentContext();

        /// <summary>
        /// async load playlist list for current user
        /// </summary>
        /// <returns></returns>
        IEnumerable<SavedAlbum>? GetCurrentUsersAlbums();

        /// <summary>
        /// async load playlist list for current user
        /// </summary>
        /// <returns></returns>
        IEnumerable<FullPlaylist>? GetCurrentUsersPlaylists();

        /// <summary>
        /// async load tracks for playlist list
        /// </summary>
        /// <returns></returns>
        List<FullTrack>? GetPlaylistTracks(string playlistId);

        /// <summary>
        /// get details of current user on local machine
        /// </summary>
        /// <returns></returns>
        PrivateUser? GetUserDetails();

        /// <summary>
        /// async find all albums for name
        /// </summary>
        /// <returns></returns>
        List<SimpleAlbum>? SearchForAlbums(string searchText);

        /// <summary>
        /// async find all albums for artist
        /// </summary>
        /// <returns></returns>
        List<FullArtist>? SearchForArtists(string searchText);

        /// <summary>
        /// async find all playlists with this name
        /// </summary>
        /// <returns></returns>
        List<FullPlaylist>? SearchForPlaylists(string searchText);


        /// <summary>
        /// set volum of currently playing device
        /// </summary>
        /// <param name="volumePercent"></param>
        /// <returns></returns>
        bool SetCurrentDeviceVolume(int volumePercent);
    
        /// <summary>
        /// make this the active device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>true if success</returns>
        bool SetDeviceAsActive(string deviceId);
        bool SetAlbumSaveStatus(string id, bool save);
        bool SetPlaylistSaveStatus(string id, bool save);
    }
}