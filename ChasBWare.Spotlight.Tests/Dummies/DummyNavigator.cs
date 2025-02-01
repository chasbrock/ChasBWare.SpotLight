using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Definitions.Utility;

namespace ChasBWare.Spotlight.Tests.Dummies
{
    internal class DummyNavigator : INavigator
    {
        public void NavigateTo(string uri)
        {
            
        }

        public void NavigateTo(Uri uri)
        {
        }

        public void PopLastNavigation()
        {
        }

        public void RegisterOnNavigate(INavigationClient client)
        {
        }

        public void UnregisterOnNavigate(INavigationClient client)
        {
        }
    }
}
