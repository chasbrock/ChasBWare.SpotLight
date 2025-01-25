namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IHatedItemsRepository
    {
        Task<HashSet<string>> GetItems(string userId);
        Task<int> SetHated(string userId, string itemId, bool isHated);
    }
}