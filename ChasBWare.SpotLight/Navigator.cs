using System;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.SpotLight
{
    public class Navigator() : INavigator
    {
        private AppShell? _shell;
        private readonly Dictionary<PageType, INavigationClient> _clients = [];

        internal void SetShell(AppShell appShell) 
        {
            _shell = appShell;
            appShell.Navigator = this;
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
}
