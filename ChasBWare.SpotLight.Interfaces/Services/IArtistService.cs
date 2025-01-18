using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Services
{
    /// <summary>
    /// provides access to artist, both from Db and from spotify
    /// </summary>
    public interface IArtistService
    {
        /// <summary>
        /// attempts to load artist from DB, otherwise creates an
        /// empty model and sets load status to NotLoaded
        /// </summary>
        /// <param name="artistId">artist id</param>
        /// <returns></returns>
        Artist GetAlbumModel(string artistId);


        void SearchForArtists(ISearchArtistsViewModel viewModel, Action? onFinished = null);



    }
}
