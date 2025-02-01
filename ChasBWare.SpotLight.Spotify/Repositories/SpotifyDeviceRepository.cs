using System.Collections.Generic;
using System.Reflection;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
  
    public class SpotifyDeviceRepository(IServiceProvider _serviceProvider,
                                         ISpotifyActionManager _actionManager)
               : ISpotifyDeviceRepository
    {
        public async Task<CurrentContext?> GetCurrentContext()
        {
            var context = await _actionManager.GetCurrentContext();
            if (context != null) 
            {
                return new CurrentContext
                {
                    Device = context.Device.CopyToDevice(),
                    Track = context.CopyToPlayingTrack()
                };
            }
            return null;
        }

        public async Task<List<IDeviceViewModel>> GetAvailableDevices()
        {
            var devices = await _actionManager.GetAvailableDevices();

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

        public async void SetDeviceVolume(int volumePercent)
        {
            await _actionManager.SetCurrentDeviceVolume(volumePercent);
        }
    }
}
