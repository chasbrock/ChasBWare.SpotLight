using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface INavigationClient
    {
        PageType PageType { get; }
        void OnNavigationRecieved(Uri? callerUri);
    }

    /// <summary>
    /// encapsulates code behind on appshell
    /// </summary>
    public interface INavigator
    {
        void NavigateTo(PageType pageType);
        void NavigateTo(Uri uri);
        void PopLastNavigation();
        void RegisterOnNavigate(INavigationClient client);
        void UnregisterOnNavigate(INavigationClient client);
    }
}