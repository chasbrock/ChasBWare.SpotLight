using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

public class TrackRepository(IDbContext _dbContext,
                             ILogger<TrackRepository> _logger)
           : ITrackRepository
{
    public List<Track> GetPlaylistTracks(string playlistId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            var sql = RepositoryHelper.GetPlaylistTracks;
            return connection.QueryAsync<Track>(sql, playlistId).Result;

        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public int AddTracksToPlaylist(string playListId, IEnumerable<Track> tracks)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            int order = 0;
            foreach (var track in tracks)
            {
                var foundTrack = connection.Table<Track>().FirstOrDefaultAsync(t => t.Id == track.Id).Result;
                if (foundTrack == null)
                {
                    connection.InsertAsync(track);
                }

                var foundLink = connection.Table<PlaylistTrack>().FirstOrDefaultAsync(t => t.PlaylistId == playListId && t.TrackId == track.Id).Result;
                if (foundLink == null)
                {
                    var playlistTrack = new PlaylistTrack
                    {
                        TrackId = track.Id,
                        PlaylistId = playListId,
                        TrackOrder = order++
                    };
                    connection.InsertAsync(playlistTrack);
                }
            }
        }
        _logger.LogError("Could not access db connection");

        return -1;
    }

    public Track? GetTrack(string trackId)
    {
        var connection = _dbContext.GetConnection().Result;
        if (connection != null)
        {
            return connection.Table<Track>().FirstOrDefaultAsync(t=>t.Id == trackId).Result;
        }
        _logger.LogError("Could not access db connection");

        return null;
    }
}
