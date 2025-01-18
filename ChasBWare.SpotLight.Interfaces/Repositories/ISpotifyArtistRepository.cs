using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface ISpotifyArtistRepository
    {
        /// <summary>
        /// load allablbums that are linked to album
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Task<List<Playlist>> LoadArtistAlbums(string artistId);

    }


}