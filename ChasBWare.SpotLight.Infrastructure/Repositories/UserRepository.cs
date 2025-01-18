using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class UserRepository(IDbContext _dbContext,
                                ILogger<UserRepository> _logger)
               : IUserRepository
    {
        public User CurrentUser { get; set; } = new User { Id = string.Empty };

        public async Task<User?> GetUser(string userId) 
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                return await connection.Table<User>().FirstOrDefaultAsync(u => u.Id == userId);
            }
            _logger.LogError("Could not access db connection");

            return null;
        }

        public async Task<User?> AddUser(string userId, string country, string name, string uri)
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var user = new User
                {
                    Id = userId,
                    Country = country,
                    Name = name,
                    Uri = uri
                };
                var found = await connection.Table<User>().FirstOrDefaultAsync(u => u.Id == userId);
                if (found == null)
                {
                    await connection.InsertAsync(user);
                }
                else
                {
                    await connection.UpdateAsync(user);
                }
                return user;
            }
            _logger.LogError("Could not access db connection");

            return null;
        }

        public async Task<bool> RemoveUser(string userId) 
        {
            var connection = await _dbContext.GetConnection();
            if (connection != null)
            {
                var sql = RepositoryHelper.GetDeleteableUserArtists;
                var deleteableAritsts = await connection.QueryScalarsAsync<string>(sql, userId);
           //     foreach(
            
            }
        
            _logger.LogError("Could not access db connection");
            return false;
        }
    }
}
