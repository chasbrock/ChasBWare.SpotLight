using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{

    public class SpotifyActionManager(ILogger _logger,
                                      ISpotifyConnectionManager _spotifyConnectionManager)
               : ISpotifyActionManager
    {
  
        public async Task<PrivateUser> GetUserDetails()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var currentUser = await client.UserProfile.Current();
 
                return currentUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return new PrivateUser();
            }
        }
        
        public async Task<IEnumerable<FullPlaylist>> GetCurrentUsersPlaylists()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var fullPlaylists = new List<FullPlaylist>();
                var page = await client.Playlists.CurrentUsers();
                await foreach (var playlist in client.Paginate(page))
                {
                    fullPlaylists.Add(playlist);
                }
                return fullPlaylists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<IEnumerable<SavedAlbum>> GetCurrentUsersAlbums()
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var savedAlbums = new List<SavedAlbum>();
                var page = await client.Library.GetAlbums();
                await foreach (var savedAlbum in client.Paginate(page))
                {
                    savedAlbums.Add(savedAlbum);
                }
                return savedAlbums;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<FullTrack>> GetPlaylistTracks(string playlistId)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var playableTracks = new List<PlaylistTrack<IPlayableItem>>();
                var page = await client.Playlists.GetItems(playlistId);
                await foreach (var playable in client.Paginate(page))
                {
                    playableTracks.Add(playable);
                }
                return playableTracks.Select(t => t.Track).Cast<FullTrack>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<SimpleTrack>> GetAlbumTracks(string albumId)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var simpleTracks = new List<SimpleTrack>();
                var page = await client.Albums.GetTracks(albumId);
                await foreach (var simpleTrack in client.Paginate(page))
                {
                    simpleTracks.Add(simpleTrack);
                }
                return simpleTracks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<SimpleAlbum>> GetArtistAlbums(string artistId)
        {
            var client = await _spotifyConnectionManager.GetClient();
            var request = new ArtistsAlbumsRequest { IncludeGroupsParam = ArtistsAlbumsRequest.IncludeGroups.Album };
            try
            {
                var simpleAlbums = new List<SimpleAlbum>();
                var page = await client.Artists.GetAlbums(artistId, request);
                await foreach (var simpleAlbum in client.Paginate(page))
                {
                    simpleAlbums.Add(simpleAlbum);
                }
                return simpleAlbums;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<Paging<SimpleAlbum>> GetArtistAlbums(string artistId, int offset)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                return await client.Artists.GetAlbums(artistId, new ArtistsAlbumsRequest { Offset = offset });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                throw;
             }
        }

        public async Task<List<FullTrack>> GetArtistTopTracks(string artistId, string userCountry)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var topTracks = await client.Artists.GetTopTracks(artistId, new ArtistsTopTracksRequest(userCountry));
                return topTracks.Tracks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<FullArtist>> FindArtist(string artistName)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Artist, artistName) { Offset = 0, Limit = 50 };
                var search = await client.Search.Item(searchRequest);
                return search.Artists?.Items?.Where(a => a.Name != null && 
                                                         a.Name.Contains(artistName, StringComparison.CurrentCultureIgnoreCase))
                                             .ToList() ?? [];

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<SimpleAlbum>> FindAlbums(string albumName)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Album, albumName) { Offset = 0, Limit = 50 };
                var search = await client.Search.Item(searchRequest);
                return search.Albums?.Items?.Where(a => a != null && a.Name.Contains(albumName, StringComparison.CurrentCultureIgnoreCase)).ToList() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

        public async Task<List<FullPlaylist>> FindPlaylists(string playlistName)
        {
            var client = await _spotifyConnectionManager.GetClient();
            try
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Playlist, playlistName) { Offset = 0, Limit = 50 };
                var search = await client.Search.Item(searchRequest);
                var fullPlaylists = new List<FullPlaylist>();
                if (search.Playlists != null && search.Playlists.Items != null)
                {
                    foreach (var fp in search.Playlists.Items)
                    {
                        if (fp != null && fp.Name != null && fp.Name.Contains(playlistName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            fullPlaylists.Add(fp);
                        }
                    }
                }
                return fullPlaylists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to access user");
                return [];
            }
        }

       
    }
}
