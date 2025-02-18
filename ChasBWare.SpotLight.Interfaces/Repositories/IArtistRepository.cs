using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories;

public interface IArtistRepository
{
    /// <summary>
    /// load allablbums that are linked to album
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    List<Playlist> LoadArtistAlbums(string artistId);

    /// <summary>
    /// save details for selected search result
    /// </summary>
    /// <param name="artist"></param>
    /// <param name="albums"></param>
    void StoreArtistAndAlbums(Artist artist, List<Playlist> albums);
    
    /// <summary>
    /// find artist in database
    /// </summary>
    /// <param name="artistId"></param>
    /// <returns></returns>
    Artist? FindArtist(string artistId);

    /// <summary>
    /// update last accessed time
    /// </summary>
    /// <param name="artist"></param>
    void UpdateLastAccessed(Artist artist);
    /*   

        /// <summary>
        /// add new artist
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>records added</returns>
        int Add(Artist artist);

        /// <summary>
        /// remove recent artist for this user, this will cascade and 
        /// delete any albums that are not references anywhere else
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>success</returns>
        bool RemoveUnsavedArtist(string artistId);

        /// <summary>
        /// remove all recent artist for this user, this will cascade and 
        /// delete any albums that are not references anywhere else
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>success</returns>
        bool RemoveUnsavedArtists();

        /// <summary>
        /// find the atrist that user has shown interest in 
        /// </summary>
        /// <returns>list of artist and the las time they were accessed</returns>
        List<Tuple<Artist, DateTime>> GetRecentArtists();

        /// <summary>
        /// link an album to an artist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="playListId"></param>
        /// <returns></returns>
        int LinkAlbumToArtist(string artistId, string playListId);


             */
}