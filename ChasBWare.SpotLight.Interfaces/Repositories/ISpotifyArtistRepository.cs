using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyArtistRepository
    {
        /// <summary>
        /// find details of artist by id
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Artist? FindArtist(string artistId);
       
        /// <summary>
        /// search for artists with given namw
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        List<IArtistViewModel> SearchForArtists(string searchText);

        /// <summary>
        /// load allablbums that are linked to album
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        List<RecentPlaylist> LoadArtistAlbums(string artistId);

    }

}