namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface IHatedService 
    { 
        bool IsHated(string? itemId);
        void Refresh();
    }
}
