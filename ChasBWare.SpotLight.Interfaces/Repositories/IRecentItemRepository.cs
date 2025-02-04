using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IRecentItemRepository 
    { 
        Task<int> UpdateLastAccessed(string? userId, string itemId, DateTime lastAccessed, bool isSaved);
    }
}