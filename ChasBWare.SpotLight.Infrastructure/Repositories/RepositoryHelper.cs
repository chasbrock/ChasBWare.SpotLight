using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using SQLite;
using System.Collections;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    internal static partial class RepositoryHelper 
    {
        internal static async Task RemovePlaylistTracks(SQLiteAsyncConnection connection,
                                                      string playlistId)
        {
            // get list of albums that are owned by albums, but not by users
            var deleteableTracks = await connection.QueryScalarsAsync<string>(GetDeleteableTracks, playlistId, playlistId);
            await connection.Table<Track>()
                            .DeleteAsync(t => t.Id != null &&
                                              deleteableTracks.Contains(t.Id));
        }

        internal static async Task RemovePlaylists(SQLiteAsyncConnection connection,
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

        internal static async Task<int> UpdateLastAccessed(SQLiteAsyncConnection connection,
                                                         string userId,
                                                         string itemId,
                                                         DateTime lastAccessed,
                                                         bool isSaved)
        {
            var recentItem = await connection.Table<RecentItem>()
                                             .FirstOrDefaultAsync(ri => ri.UserId == userId &&
                                                                        ri.ItemId == itemId);
            if (recentItem == null)
            {
                // not IsSaved is always false for artists, simply not the 
                // way spotify works
                return await connection.InsertAsync(
                new RecentItem
                {
                    UserId = userId,
                    ItemId = itemId,
                    LastAccessed = lastAccessed,
                    IsSaved = isSaved
                });
            }
            recentItem.LastAccessed = lastAccessed;
            return await connection.UpdateAsync(recentItem);
        }
    }
}