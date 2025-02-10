namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services
{
    public interface IAlertService
    {
        Task ShowErrorAsync(string title, string message, string cancel);
    }
}
