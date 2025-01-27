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
        public  async Task<List<IArtistViewModel>> FindArtists(string searchText)
        {
            var fullArtists = await _actionManager.FindArtist(searchText);

            List<IArtistViewModel> artists = []; 
            foreach (var fullArtist in fullArtists.Where(t => t != null))
            {
                var artistViewModel = _serviceProvider.GetService<IArtistViewModel>();
                if (artistViewModel != null)
                {
                    artistViewModel.Id = fullArtist.Id;
                    artistViewModel.Name = fullArtist.Name;
                    artistViewModel.Image = fullArtist.Images.GetSmallImage();
                    artists.Add(artistViewModel);
                }
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
    }
}
