using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChasBWare.SpotLight.Infrastructure.Utility
{
    public static class AppSettingsHelper
    {
        public static string BuildKey(this object owner, string key) 
        { 
            return $"{owner.GetType().Name}_{key}";
        }
    }
}
