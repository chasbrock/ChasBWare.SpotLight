using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories;

public class HatedItemsRepository(IDbContext _dbContext,
                                  ILogger<HatedItemsRepository> _logger)
           : IHatedItemsRepository
{
    public async Task<HashSet<string>> GetItems()
    {
        var connection = await _dbContext.GetConnection();
        if (connection != null)
        {
            var sql = "select ItemId from HatedItem";
            var found = await connection.QueryScalarsAsync<string>(sql);
            if (found != null)
            {
                return new HashSet<string>(found);
            }
            return [];

        }
        _logger.LogError("Could not access db connection");

        return [];
    }

    public async Task<int> SetHated(string itemId, bool isHated)
    {
        var connection = await _dbContext.GetConnection();
        if (connection != null)
        {
            if (isHated)
            {
                var found = await connection.Table<HatedItem>()
                                            .FirstOrDefaultAsync(hi => hi.ItemId == itemId);
                if (found == null)
                {
                    return await connection.InsertAsync(new HatedItem { ItemId = itemId });
                }
                return 0;
            }
            else
            {
                return await connection.Table<HatedItem>().DeleteAsync(hi => hi.ItemId == itemId);
            }
        }

        _logger.LogError("Could not access db connection");
        return -1;
    }
}
