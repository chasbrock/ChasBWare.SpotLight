namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IHatedItemsRepository
    {
        Task<HashSet<string>> GetItems();
        Task<int> SetHated(string itemId, bool isHated);
    }
}