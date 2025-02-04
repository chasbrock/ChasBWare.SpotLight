using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;

namespace ChasBWare.SpotLight.Infrastructure.Utility;

public static class DeviceHelper
{
    public static DeviceModel GetLocalDevice()
    {
        return new DeviceModel
        {
            Name = $"Local {DeviceInfo.Current.Idiom}",
            DeviceType = DeviceInfo.Current.Idiom.ToDeviceTypes(),
            Id = "LOCAL" 
         };
    }

    public static DeviceTypes ToDeviceTypes(this DeviceIdiom idiom)
    {
        if (idiom == DeviceIdiom.Desktop)
        {
            return DeviceTypes.Computer;
        }
        if (idiom == DeviceIdiom.Phone)
        {
            return DeviceTypes.Smartphone;
        }
        if (idiom == DeviceIdiom.Tablet)
        {
            return DeviceTypes.Tablet;
        }
        return DeviceTypes.Unknown;
    }
}
