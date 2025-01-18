using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Classes;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyArtistRepository(SpotifyActionManager _actionManager) 
               : ISpotifyArtistRepository
    {
        public async Task<List<Playlist>> LoadArtistAlbums(string artistId)
        {
            var savedAlbums = await _actionManager.GetArtistAlbums(artistId);
            if (savedAlbums != null) 
            {
                return savedAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
            }
            return [];
        }
    }
}
