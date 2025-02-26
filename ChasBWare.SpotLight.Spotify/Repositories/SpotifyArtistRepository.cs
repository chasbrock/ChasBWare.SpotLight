using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories;

public class SpotifyArtistRepository(IServiceProvider _serviceProvider,
                                     ISpotifyActionManager _actionManager) 
           : ISpotifyArtistRepository
{
    public List<IArtistViewModel> SearchForArtists(string searchText)
    {
        var fullArtists =  _actionManager.SearchForArtists(searchText);

        List<IArtistViewModel> artists = [];
        if (fullArtists != null)
        {
            foreach (var artist in fullArtists.Where(t => t != null).Select(fa => fa.CopyToArtist()))
            {
                var artistViewModel = _serviceProvider.GetRequiredService<IArtistViewModel>();
                artistViewModel.Model = artist;
                artists.Add(artistViewModel);
            }
        }
        return artists;
    }

    public List<Playlist> LoadArtistAlbums(Artist artist)
    {
        if (artist.Id == null) 
        {
            return [];
        }

        var savedAlbums =  _actionManager.GetArtistAlbums(artist.Id);
        if (savedAlbums != null) 
        {
            List<Playlist> playlists = savedAlbums.Select(sa => sa.CopyToPlaylist()).ToList();
            playlists.Add(artist.CopyToTopTracksPlaylist());
            return playlists;
        }

        return [];
    }

    public Artist? FindArtist(string artistId)
    {
        var found = _actionManager.FindArtist(artistId);
        if (found != null)
        {
            return found.CopyToArtist();
        }
        return null;
    }
}
