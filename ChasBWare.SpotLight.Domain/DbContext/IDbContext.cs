using SQLite;

namespace ChasBWare.SpotLight.Domain.DbContext;

public interface IDbContext
{
    Task EnsureDbExists();
    Task<SQLiteAsyncConnection> GetConnection();
}
