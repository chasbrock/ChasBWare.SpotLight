namespace ChasBWare.SpotLight.Definitions.Tasks
{
    public interface IUpdateLastAccessedTask 
    { 
        void Execute(string itemId, DateTime lastAccessed, bool isSaved);
    }
}
