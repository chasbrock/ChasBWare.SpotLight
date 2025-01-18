using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Domain.DbContext;
using ChasBWare.SpotLight.Domain.Entities;
using System.Text.Json;

namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public class AppSettingsRepository(IDbContext _dbContext) : IAppSettingsRepository
    {
        public async Task Delete(string name)
        {
            name = name.ToUpper();
            var connection = await _dbContext.GetConnection();
            await connection.Table<AppSetting>().DeleteAsync(s => s.Name == name);
        }

        public async Task<string?> Load(string name)
        {
            name = name.ToUpper();
            var connection = await _dbContext.GetConnection();
            var found = await connection.Table<AppSetting>().FirstOrDefaultAsync(a => a.Name == name);
            return found?.Value;
        }

        public async Task<T?> Load<T>(string name)
        {
            name = name.ToUpper();
            var connection = await _dbContext.GetConnection();
            var found = await connection.Table<AppSetting>().FirstOrDefaultAsync(a => a.Name == name);
            if (found != null)
            {
                return string.IsNullOrWhiteSpace(found.Value) ? default : JsonSerializer.Deserialize<T>(found.Value);
            }
            return default;
        }

        public async Task Save(string name, string value)
        {
            name = name.ToUpper();
            var connection = await _dbContext.GetConnection();
            var setting = await connection.Table<AppSetting>().FirstOrDefaultAsync(a => a.Name == name);
            if (setting == null)
            {
                await connection.InsertAsync(new AppSetting { Name = name, Value = value });
            }
            else
            {
                setting.Value = value;
                await connection.UpdateAsync(setting);
            }
        }

        public async Task Save<T>(string name, T value, bool deferUpdate = false)
        {
            name = name.ToUpper();
            var connection = await _dbContext.GetConnection();
            var setting = await connection.Table<AppSetting>().FirstOrDefaultAsync(a => a.Name == name);
            if (setting == null)
            {
                await connection.InsertAsync(new AppSetting { Name = name, Value = JsonSerializer.Serialize(value) });
            }
            else
            {
                setting.Value = JsonSerializer.Serialize(value);
                await connection.UpdateAsync(setting);
            }
        }
    }
}
