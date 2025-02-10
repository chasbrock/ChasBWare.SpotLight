using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyArtistRepository(IServiceProvider _serviceProvider,
                                         ISpotifyActionManager _actionManager) 
               : ISpotifyArtistRepository
    {
        public  async Task<List<IArtistViewModel>> SearchForArtists(string searchText)
        {
            var fullArtists = await _actionManager.SearchForArtists(searchText);

            List<IArtistViewModel> artists = []; 
            foreach (var artist in fullArtists.Where(t => t != null).Select(fa => fa.CopyToArtist()))
            {
                var artistViewModel = _serviceProvider.GetRequiredService<IArtistViewModel>();
                artistViewModel.Model = artist;
                artists.Add(artistViewModel);
            }

            return artists;
        }

        public async Task<List<RecentPlaylist>> LoadArtistAlbums(string artistId)
        {
            var savedAlbums = await _actionManager.GetArtistAlbums(artistId);
            if (savedAlbums != null) 
            {
                return savedAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
            }
            return [];
        }

        public async Task<Artist?> FindArtist(string artistId)
        {
            var found = await _actionManager.FindArtist(artistId);
            if (found != null)
            {
                return found.CopyToArtist();
            }
            return null;
        }
    }
}
