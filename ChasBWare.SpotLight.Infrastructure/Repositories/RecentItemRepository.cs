using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class RecentItemRepository(IDbContext _dbContext,
                                      ILogger _logger)
               : IRecentItemRepository
    {
        public int UpdateLastAccessed(string? userId, string itemId, DateTime lastAccessed, bool isSaved)
        {
            if (userId == null)
            { 
                return 0;
            }
            var connection = _dbContext.GetConnection().Result;
            if (connection != null)
            {
                return RepositoryHelper.UpdateLastAccessed(connection, userId, itemId, lastAccessed, isSaved);
            }
            _logger.LogError("Could not access db connection");

            return -1;
        }
    }
}
