using System.Collections.Generic;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Mappings.Mappers;
using ChasBWare.SpotLight.Spotify.Interfaces;

namespace ChasBWare.SpotLight.Spotify.Repositories
{
    public class SpotifyDeviceRepository(IServiceProvider _serviceProvider,
                                         ISpotifyActionManager _actionManager)
               : ISpotifyDeviceRepository
    {
        public async Task<IDeviceViewModel?> GetActiveDevice()
        {
            var devices = await _actionManager.GetAvailableDevices();
            if (devices != null)
            {
                var model= devices.FirstOrDefault(d => d.IsActive)?.CopyToDevice();
                if (model != null)
                {
                    var viewModel = _serviceProvider.GetService<IDeviceViewModel>();
                    if (viewModel != null)
                    {
                        viewModel.Model = model;
                        return viewModel;
                    }
                }
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
    }
}
