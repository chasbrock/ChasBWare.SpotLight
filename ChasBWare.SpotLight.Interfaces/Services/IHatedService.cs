namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface IHatedService 
    {
        bool Initialised { get; }

        bool IsHated(string? itemId);
        void Refresh();
    }
}
