namespace ChasBWare.SpotLight.Definitions.Utility
{
    public interface INavigationClient 
    { 
        string Path { get; }
        void OnNavigationRecieved(Uri? callerUri);
    }

    /// <summary>
    /// encapsulates code behind on appshell
    /// </summary>
    public interface INavigator 
    {
        void NavigateTo(string uri);
        void NavigateTo(Uri uri);
        void PopLastNavigation();
        void RegisterOnNavigate(INavigationClient client);
        void UnregisterOnNavigate(INavigationClient client);
    }
}