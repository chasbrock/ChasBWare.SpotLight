using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyArtistRepository
    {
        Task<Artist?> FindArtist(string artistId);
       
        Task<List<IArtistViewModel>> SearchForArtists(string searchText);

        /// <summary>
        /// load allablbums that are linked to album
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Task<List<RecentPlaylist>> LoadArtistAlbums(string artistId);

    }

}