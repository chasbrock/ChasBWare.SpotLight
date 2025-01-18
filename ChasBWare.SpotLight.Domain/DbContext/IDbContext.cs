using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace ChasBWare.SpotLight.Domain.DbContext
{
    public interface IDbContext
    {
        Task EnsureDbExists();
        Task<SQLiteAsyncConnection> GetConnection();
    }
}
