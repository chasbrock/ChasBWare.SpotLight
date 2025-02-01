using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.Spotlight.Tests.Dummies.Services;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChasBWare.Spotlight.Tests.ViewModelTests
{
    public class PlayerControlViewModelTests
    {
        private readonly PlayerControlViewModelTestEnvironment _environment;
        private readonly IPlayerControlViewModel _playerControlViewModel;
        private readonly DummyTrackPlayerService _trackPlayerService;
      
        public PlayerControlViewModelTests() 
        {
            _environment = new PlayerControlViewModelTestEnvironment();
            _playerControlViewModel = _environment.GetPlayerControlViewModel();
            _trackPlayerService = _environment.GetTrackPlayerService();
        }

        [Fact]
        public void ChangeDevice() 
        {
            _trackPlayerService.PlayingTrack.IsPlaying = false;

            // manually set device
            var device = _environment.GetDeviceViewModel(1);
            _playerControlViewModel.CurrentDevice = device;
            Assert.Equal(device.Name, _playerControlViewModel.CurrentDevice.Name);
            Assert.False(_playerControlViewModel.IsPlaying);

            // send meeage to change active device
            device = _environment.GetDeviceViewModel(2);
            _environment.SendActiveDeviceChangedMessage(device);
            Assert.Equal(device.Name, _playerControlViewModel.CurrentDevice.Name);
            Assert.False(_playerControlViewModel.IsPlaying);
        }

    }
}
