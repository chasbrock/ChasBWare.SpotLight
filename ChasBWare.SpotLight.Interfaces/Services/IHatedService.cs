namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface IHatedService 
    {
        bool Initialised { get; }
        void SetIsHated(string itemId, bool isHated);
        bool GetIsHated(string? itemId);
        void Refresh();
    }
}
