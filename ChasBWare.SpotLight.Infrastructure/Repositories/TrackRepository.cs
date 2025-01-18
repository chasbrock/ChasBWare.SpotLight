using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class TrackRepository(IDbContext _dbContext,
                                 ILogger _logger)
               : ITrackRepository
    {
        public async Task<List<Track>> GetPlaylistTracks(string playlistId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylistTracks;
                return await connection.QueryAsync<Track>(sql, playlistId);

            }
            _logger.LogError("Could not access db connection");

            return [];
        }

        public async Task<int> AddTracksToPlaylist(string playListId, IEnumerable<Track> tracks)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                int order = 0;
                foreach (var track in tracks)
                {
                    var foundTrack = await connection.Table<Track>().FirstOrDefaultAsync(t => t.Id == track.Id);
                    if (foundTrack == null)
                    {
                        await connection.InsertAsync(track);
                    }

                    var foundLink = await connection.Table<PlaylistTrack>().FirstOrDefaultAsync(t => t.PlaylistId == playListId && t.TrackId == track.Id);
                    if (foundLink == null)
                    {
                        var playlistTrack = new PlaylistTrack
                        {
                            TrackId = track.Id,
                            PlaylistId = playListId,
                            TrackOrder = order++
                        };
                        await connection.InsertAsync(playlistTrack);
                    }
                }
            }
            _logger.LogError("Could not access db connection");

            return -1;
        }
    }
}
