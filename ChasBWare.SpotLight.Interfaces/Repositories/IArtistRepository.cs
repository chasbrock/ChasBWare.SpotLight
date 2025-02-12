using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IArtistRepository
    {
        /// <summary>
        /// find artist in database
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Artist? FindArtist(string artistId);

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
        bool RemoveUnsavedArtist(string userId, string artistId);

        /// <summary>
        /// remove all recent artist for this user, this will cascade and 
        /// delete any albums that are not references anywhere else
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>success</returns>
        bool RemoveUnsavedArtists(string userId);

        /// <summary>
        /// find the atrist that user has shown interest in 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of artist and the las time they were accessed</returns>
        List<Tuple<Artist, DateTime>> GetRecentArtists(string userId);

        /// <summary>
        /// link an album to an artist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="playListId"></param>
        /// <returns></returns>
        int LinkAlbumToArtist(string artistId, string playListId);

        /// <summary>
        /// load allablbums that are linked to album
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        List<RecentPlaylist> LoadArtistAlbums(string artistId);

        /// <summary>
        /// save details for selected search result
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="artist"></param>
        /// <param name="albums"></param>
        void AddRecentArtistAndAlbums(string currentUserId, Artist artist, List<RecentPlaylist> albums);

    }
}