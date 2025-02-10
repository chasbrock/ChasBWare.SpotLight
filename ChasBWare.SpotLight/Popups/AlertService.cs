using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Popups;

public class AlertService : IAlertService
{
    public Task ShowErrorAsync(string title, string message, string cancel)
    {
        return Application.Current!.Windows[0]!.Page!.DisplayAlert(title, message, cancel);
    }
}
