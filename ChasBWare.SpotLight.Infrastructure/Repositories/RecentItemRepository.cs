using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class RecentItemRepository(IDbContext _dbContext,
                                      ILogger<RecentItemRepository> _logger)
               : IRecentItemRepository
    {
        public async Task<int> UpdateLastAccessed(string? userId, string itemId, DateTime lastAccessed, bool isSaved)
        {
            if (userId == null)
            { 
                return 0;
            }
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var found  = await connection.Table<RecentItem>().FirstOrDefaultAsync(ri => ri.UserId == userId &&  ri.ItemId == itemId);
                if(found == null)
                { 
                    return await connection.InsertAsync(new RecentItem
                    {
                        UserId=userId, 
                        ItemId=itemId, 
                        LastAccessed = lastAccessed, 
                        IsSaved = isSaved 
                    });
                }
                found.LastAccessed = lastAccessed;
                return await connection.UpdateAsync(found);
            }
            _logger.LogError("Could not access db connection");

            return -1;
        }
    }
}
