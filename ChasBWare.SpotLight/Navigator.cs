using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight
{
    public class Navigator() : INavigator
    {
        private AppShell? _shell;
        private readonly Dictionary<string, INavigationClient> _clients = [];

        internal void SetShell(AppShell appShell) 
        {
            _shell = appShell;
            appShell.Navigator = this;
        }

        public void NavigateTo(string uri)
        {
            if (_shell != null)
            {
                _shell.GoToAsync(uri);
                if (_clients.TryGetValue(uri, out var client) )
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
                if (_clients.TryGetValue(uri.ToString(), out var client))
                {
                    client.OnNavigationRecieved(LastPage);
                }
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
            _clients[client.Path] = client;
        }

        public void UnregisterOnNavigate(INavigationClient client)
        {
            _clients.Remove(client.Path);
        }

        internal Uri? LastPage { get; set; }

    }
}
