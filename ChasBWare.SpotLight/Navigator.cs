using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight;

public class Navigator : INavigator
{
    private AppShell? _shell;
    private readonly List<Type> _navigatable;
    private readonly Dictionary<PageType, INavigationClient> _clients = [];

    public Navigator(List<Type> navigatable)
    {
        _navigatable = navigatable;
    }

    internal void SetShell(AppShell appShell, IServiceProvider serviceProvider) 
    {
        _shell = appShell;
        appShell.Navigator = this;
        foreach (var type in _navigatable)
        {
            var instance = serviceProvider.GetService(type);
            if (instance is INavigationClient navigationClient)
            {
                _clients.TryAdd(navigationClient.PageType, navigationClient);
            }
        }

    }
          
    public void NavigateTo(PageType pageType)
    {
        if (_shell != null)
        {
            _shell.GoToAsync($"//{pageType}");
            if (_clients.TryGetValue(pageType, out var client))
            {
                client.OnNavigationRecieved(LastPage);
            }
        }
    }

    public void NavigateTo(Uri uri)
    {
        if (_shell != null)
        {
            _shell.GoToAsync(uri);
        }
    }

    public void PopLastNavigation()
    {
        if (_shell != null && LastPage != null)
        {
            _shell.GoToAsync(LastPage);

        }
    }

    public void RegisterOnNavigate(INavigationClient client)
    {
        _clients[client.PageType] = client;
    }

    public void UnregisterOnNavigate(INavigationClient client)
    {
        _clients.Remove(client.PageType);
    }

    internal Uri? LastPage { get; set; }

}
