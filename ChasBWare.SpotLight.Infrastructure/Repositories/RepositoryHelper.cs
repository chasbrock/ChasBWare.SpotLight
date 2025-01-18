using ChasBWare.SpotLight.Domain.Entities;
using SQLite;
using System.Collections;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public static partial class RepositoryHelper 
    {
        public static async Task RemovePlaylistTracks(SQLiteAsyncConnection connection,
                                                      string playlistId)
        {
            // get list of albums that are owned by albums, but not by users
            var deleteableTracks = await connection.QueryScalarsAsync<string>(GetDeleteableTracks, playlistId, playlistId);
            await connection.Table<Track>()
                            .DeleteAsync(t => t.Id != null &&
                                              deleteableTracks.Contains(t.Id));
        }

        public static async Task RemovePlaylists(SQLiteAsyncConnection connection,
                                                string userId, IList playlistIds)
        {
            await connection.Table<RecentItem>()
                            .DeleteAsync(ri => ri.UserId == userId &&
                                               ri.ItemId != null  &&
                                               playlistIds.Contains(ri.ItemId));

            await connection.Table<PlaylistTrack>()
                            .DeleteAsync(plt => plt.PlaylistId != null &&
                                                playlistIds.Contains(plt.PlaylistId));

            await connection.Table<Playlist>()
                            .DeleteAsync(pl => pl.Id != null &&
                                               playlistIds.Contains(pl.Id));
        }


        public static async Task RemoveArtist(SQLiteAsyncConnection connection,
                                                string userId, string artistId)
        {
            await connection.Table<RecentItem>()
                            .DeleteAsync(ri => ri.UserId == userId &&
                                               ri.ItemId == artistId);

            await connection.Table<ArtistPlaylist>()
                            .DeleteAsync(ri => ri.ArtistId == artistId);

            await connection.Table<Artist>()
                            .DeleteAsync(ri => ri.Id == artistId);
        }

    }
}