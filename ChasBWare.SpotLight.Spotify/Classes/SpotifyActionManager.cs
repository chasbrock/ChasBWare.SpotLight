using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using SpotifyDevice = SpotifyAPI.Web.Device;

namespace ChasBWare.SpotLight.Spotify.Classes;

public class SpotifyActionManager(ISpotifyConnectionManager _spotifyConnectionManager)
           : ISpotifyActionManager
{
    public string? CountryCode { get; private set;}

    public FullAlbum? FindAlbum(string albumId)
    {
        return SpotifyErrorCatcher.Execute<FullAlbum>(_spotifyConnectionManager,
            client =>
            {
                return client.Albums.Get(albumId).Result;
            });
    }

    public FullArtist? FindArtist(string artistId)
    {
        return SpotifyErrorCatcher.Execute<FullArtist>(_spotifyConnectionManager,
            client =>
            {
                return client.Artists.Get(artistId).Result;
            });
    }

    public FullPlaylist? FindPlaylist(string playlistId)
    {
        return SpotifyErrorCatcher.Execute<FullPlaylist>(_spotifyConnectionManager,
            client =>
            {
                return client.Playlists.Get(playlistId).Result;
            });
    }

    public List<SimpleTrack>? GetAlbumTracks(string albumId)
    {
        return SpotifyErrorCatcher.Execute<Task<List<SimpleTrack>>>(_spotifyConnectionManager,
            async client =>
            {
                var simpleTracks = new List<SimpleTrack>();
                var page = client.Albums.GetTracks(albumId).Result;
                await foreach (var simpleTrack in client.Paginate(page))
                {
                    simpleTracks.Add(simpleTrack);
                }
                return simpleTracks;
            })?.Result;
    }

    public List<SimpleAlbum>? GetArtistAlbums(string artistId)
    {
        return SpotifyErrorCatcher.Execute<Task<List<SimpleAlbum>>>(_spotifyConnectionManager,
            async client =>
            {
                var request = new ArtistsAlbumsRequest { IncludeGroupsParam = ArtistsAlbumsRequest.IncludeGroups.Album };
                var simpleAlbums = new List<SimpleAlbum>();
                var page = await client.Artists.GetAlbums(artistId, request);
                await foreach (var simpleAlbum in client.Paginate(page))
                {
                    simpleAlbums.Add(simpleAlbum);
                }
                return simpleAlbums;
            })?.Result;
    }

    public List<SimpleAlbum>? GetArtistPlaylists(string artistId)
    {
        return SpotifyErrorCatcher.Execute<Task<List<SimpleAlbum>>>(_spotifyConnectionManager,
            async client =>
            {
                var request = new ArtistsAlbumsRequest { IncludeGroupsParam = ArtistsAlbumsRequest.IncludeGroups.Album };
                var simpleAlbums = new List<SimpleAlbum>();
                var page = await client.Artists.GetAlbums(artistId, request);
                await foreach (var simpleAlbum in client.Paginate(page))
                {
                    simpleAlbums.Add(simpleAlbum);
                }
                return simpleAlbums;
            })?.Result;
    }

    public List<FullTrack>? GetArtistTopTracks(string artistId)
    {
        return SpotifyErrorCatcher.Execute<List<FullTrack>>(_spotifyConnectionManager,
            client =>
            {
                var userCountry = GetUserCountrCode(client);
                var topTracks = client.Artists.GetTopTracks(artistId, new ArtistsTopTracksRequest(userCountry)).Result;
                return topTracks.Tracks;
            });
    }

    public List<SpotifyDevice>? GetAvailableDevices()
    {
        return SpotifyErrorCatcher.Execute<List<SpotifyDevice>?>(_spotifyConnectionManager,
            client =>
            {
                var devicesResponse = client.Player.GetAvailableDevices().Result;
                return devicesResponse != null ? devicesResponse.Devices : [];
            });
    }

    public CurrentlyPlayingContext? GetCurrentContext()
    {
        return SpotifyErrorCatcher.Execute<CurrentlyPlayingContext>(_spotifyConnectionManager,
            client =>
            {
                var context = client.Player.GetCurrentPlayback().Result;
                if (context == null)
                {
                    _spotifyConnectionManager.SetStatus(ConnectionStatus.NotConnected);
                }
                return context;
            });
    }

    public IEnumerable<SavedAlbum>? GetCurrentUsersAlbums()
    {
        return SpotifyErrorCatcher.Execute<Task<IEnumerable<SavedAlbum>>>(_spotifyConnectionManager,
             async client =>
             {
                 var savedAlbums = new List<SavedAlbum>();
                 var page = client.Library.GetAlbums().Result;
                 await foreach (var savedAlbum in client.Paginate(page))
                 {
                     savedAlbums.Add(savedAlbum);
                 }
                 return savedAlbums;
             })?.Result;
    }

    public IEnumerable<FullPlaylist>? GetCurrentUsersPlaylists()
    {
        return SpotifyErrorCatcher.Execute<Task<IEnumerable<FullPlaylist>>>(_spotifyConnectionManager,
            async client =>
            {
                var fullPlaylists = new List<FullPlaylist>();
                var page = client.Playlists.CurrentUsers().Result;
               
                await foreach (var playlist in client.Paginate(page))
                {
                    if (playlist != null)
                    {
                        fullPlaylists.Add(playlist);
                    }
                }
                return fullPlaylists;
            })?.Result;
    }

    public List<FullTrack>? GetPlaylistTracks(string playlistId)
    {
        return SpotifyErrorCatcher.Execute<Task<List<FullTrack>>>(_spotifyConnectionManager,
            async client =>
            {
                var playableTracks = new List<PlaylistTrack<IPlayableItem>>();
                var page = client.Playlists.GetItems(playlistId).Result;
                await foreach (var playable in client.Paginate(page))
                {
                    playableTracks.Add(playable);
                }
                return playableTracks.Select(t => t.Track).Cast<FullTrack>().ToList();
            })?.Result;
    }

    public PrivateUser? GetUserDetails()
    {
        return SpotifyErrorCatcher.Execute<PrivateUser>(_spotifyConnectionManager,
            client =>
            {
                return client.UserProfile.Current().Result;
            });
    }
    
    public List<SimpleAlbum>? SearchForAlbums(string searchText)
    {
        return SpotifyErrorCatcher.Execute<List<SimpleAlbum>>(_spotifyConnectionManager,
            client =>
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Album, searchText) { Offset = 0, Limit = 50 };
                var search = client.Search.Item(searchRequest).Result;
                return search.Albums?.Items?.Where(a => a != null &&
                                                        a.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                                            .ToList() ?? [];
            });
    }

    public List<FullArtist>? SearchForArtists(string artistName)
    {
        return SpotifyErrorCatcher.Execute<List<FullArtist>>(_spotifyConnectionManager,
            client =>
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Artist, artistName) { Offset = 0, Limit = 50 };
                var search = client.Search.Item(searchRequest).Result;
                return search.Artists?.Items?.Where(a => a.Name != null && a.Name.Contains(artistName, StringComparison.CurrentCultureIgnoreCase)).ToList() ?? [];
            });
    }

    public List<FullPlaylist>? SearchForPlaylists(string playlistName)
    {
        return SpotifyErrorCatcher.Execute<List<FullPlaylist>>(_spotifyConnectionManager,
            client =>
            {
                var searchRequest = new SearchRequest(SearchRequest.Types.Playlist, playlistName) { Offset = 0, Limit = 50 };
                var search = client.Search.Item(searchRequest).Result;
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
            });
    }

    public bool SetAlbumSaveStatus(string id, bool save)
    {
        return SpotifyErrorCatcher.Execute<bool>(_spotifyConnectionManager,
             client =>
             {
                 if (save)
                 {
                     var request = new LibrarySaveAlbumsRequest([id]);
                     return client.Library.SaveAlbums(request).Result;
                 }
                 else
                 {
                     var request = new LibraryRemoveAlbumsRequest([id]);
                     return client.Library.RemoveAlbums(request).Result;
                 }
             });
    }

    public bool SetCurrentDeviceVolume(int volumePercent)
    {
        return SpotifyErrorCatcher.Execute<bool>(_spotifyConnectionManager,
            client =>
            {
                var request = new PlayerVolumeRequest(volumePercent);
                return client.Player.SetVolume(request).Result;
            });
    }

    public bool SetDeviceAsActive(string deviceId)
    {
        return SpotifyErrorCatcher.Execute<bool>(_spotifyConnectionManager,
            client =>
            {
                var request = new PlayerTransferPlaybackRequest([deviceId]);
                return client.Player.TransferPlayback(request).Result;
            });
    }

    public bool SetPlaylistSaveStatus(string id, bool save)
    {
        return SpotifyErrorCatcher.Execute<bool>(_spotifyConnectionManager,
             client =>
             {
                 if (save)
                 {
                     return client.Follow.FollowPlaylist(id).Result;
                 }
                 else
                 {
                     return client.Follow.UnfollowPlaylist(id).Result;
                 }
             });
    }

    private string GetUserCountrCode(SpotifyClient client)
    {
        if (CountryCode == null)
        {
            var user = client.UserProfile.Current().Result;
            CountryCode = user.Country;
        }
        return CountryCode;
    }
}
