using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class HatedItemsRepository(IDbContext _dbContext,
                                 ILogger _logger)
               : IHatedItemsRepository
    {
        public async Task<HashSet<string>> GetItems(string userId)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetPlaylistTracks;
                var found = await connection.QueryScalarsAsync<string>(sql, userId);
                if (found != null)
                {
                    return new HashSet<string>(found);
                }
                return [];

            }
            _logger.LogError("Could not access db connection");

            return [];
        }

        public async Task<int> SetHated(string userId, string itemId, bool isHated)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                if (isHated)
                {
                    var found = await connection.Table<HatedItem>()
                                                .FirstOrDefaultAsync(hi => hi.UserId == userId &&
                                                                           hi.ItemId == itemId);
                    if (found == null)
                    {
                        return await connection.InsertAsync(new HatedItem { ItemId = itemId, UserId = userId });
                    }
                    return 0;
                }
                else
                {
                    return await connection.Table<HatedItem>().DeleteAsync(hi => hi.UserId == userId && hi.ItemId == itemId);
                }
            }

            _logger.LogError("Could not access db connection");
            return -1;
        }
    }
}
