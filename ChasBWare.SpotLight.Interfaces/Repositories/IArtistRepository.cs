using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IArtistRepository
    {
        /// <summary>
        /// add new artist
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>records added</returns>
        Task<int> Add(Artist artist);

        /// <summary>
        /// remove existing artist for this user, this will cascade and 
        /// delete any albums that are not references anywhere else
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>success</returns>
        Task<bool> Remove(string? userId, string artistId);

        /// <summary>
        /// find the atrist that user has shown interest in 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of artist and the las time they were accessed</returns>
        Task<List<Tuple<Artist, DateTime>>> GetRecentArtists(string? userId);

        /// <summary>
        /// link an album to an artist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="playListId"></param>
        /// <returns></returns>
        Task<int> LinkAlbumToArtist(string artistId, string playListId);

        /// <summary>
        /// load allablbums that are linked to album
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Task<List<RecentPlaylist>> LoadArtistAlbums(string artistId);
        Task AddRecentArtistAndAlbums(string currentUserId, Artist artist, List<RecentPlaylist> albums);
    }
}