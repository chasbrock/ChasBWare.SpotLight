using System.Collections.Generic;
using System.Reflection;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories;


public class SpotifyDeviceRepository(IServiceProvider _serviceProvider,
                                     ISpotifyActionManager _actionManager)
           : ISpotifyDeviceRepository
{
    public CurrentContext? GetCurrentContext()
    {
        var context = _actionManager.GetCurrentContext();
        if (context != null)
        {
            return new CurrentContext
            {
                Device = context.Device.CopyToDevice(),
                Track = context.CopyToPlayingTrack()
            };
        }
        else
        {
            
        }
            return null;
    }

    public List<IDeviceViewModel> GetAvailableDevices()
    {
        var devices = _actionManager.GetAvailableDevices();

        List<IDeviceViewModel> viewModels = [];
        if (devices != null)
        {
            foreach (var model in devices.Select(d => d.CopyToDevice()))
            {
                var viewModel = _serviceProvider.GetService<IDeviceViewModel>();
                if (viewModel != null) 
                { 
                    viewModel.Model = model;
                    viewModels.Add(viewModel);
                }
            }
        }

        return viewModels;
    }

    public void SetDeviceVolume(int volumePercent)
    {
        _actionManager.SetCurrentDeviceVolume(volumePercent);
    }

    public bool SetDeviceAsActive(string deviceId)
    {
        return  _actionManager.SetDeviceAsActive(deviceId);
    }
}
