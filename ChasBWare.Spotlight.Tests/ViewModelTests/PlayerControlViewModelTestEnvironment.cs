using System.Reflection;
using ChasBWare.Spotlight.Tests.Dummies;
using ChasBWare.Spotlight.Tests.Dummies.Services;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Models;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Services;
using ChasBWare.SpotLight.Infrastructure.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChasBWare.Spotlight.Tests.ViewModelTests
{
    internal class PlayerControlViewModelTestEnvironment : BaseTestEnvironment 
    {

        internal IPlayerControlViewModel GetPlayerControlViewModel()
        {
            var viewModel = _serviceProvider.GetService<IPlayerControlViewModel>();
            Assert.NotNull(viewModel);
            return viewModel;
        }

        internal DummyTrackPlayerService GetTrackPlayerService()
        {
            var service = _serviceProvider.GetService<ITrackPlayerService>() as DummyTrackPlayerService;
            Assert.NotNull(service);
            return service;
        }

        internal IDeviceViewModel GetDeviceViewModel(int id)
        {
            var viewModel = _serviceProvider.GetService<IDeviceViewModel>();
            Assert.NotNull(viewModel);
            viewModel.Model = new DeviceModel { Id = $"Test{id}", Name = $"Test{id}", DeviceType = "Test" };
            return viewModel;
        }

        internal void SendPlayTracklistMessage(IPlaylistViewModel playlist, int offset)
        {
            var messageService = _serviceProvider.GetService<MessageService<PlayTracklistMessage>>();
            Assert.NotNull(messageService);
            messageService.SendMessage(new PlayTracklistMessage(playlist, offset));
        }

        internal void SendActiveDeviceChangedMessage(IDeviceViewModel device)
        {
            var messageService = _serviceProvider.GetService<MessageService<ActiveDeviceChangedMessage>>();
            Assert.NotNull(messageService);
            messageService.SendMessage(new ActiveDeviceChangedMessage(device.Model));
        }

        internal void SendConnectionStatusChangedMessage(ConnectionStatus status)
        {
            var messageService = _serviceProvider.GetService<MessageService<ConnectionStatusChangedMessage>>();
            Assert.NotNull(messageService);
            messageService.SendMessage(new ConnectionStatusChangedMessage(status));
        }


        protected override void InitialiseDI(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<INavigator, DummyNavigator>()
                             .AddSingleton<ITrackPlayerService, DummyTrackPlayerService>()
                             .AddSingleton<IMessageService<PlayTracklistMessage>, MessageService<PlayTracklistMessage>>()
                             .AddSingleton<IMessageService<ActiveDeviceChangedMessage>, MessageService<ActiveDeviceChangedMessage>>()
                             .AddSingleton<IMessageService<ConnectionStatusChangedMessage>, MessageService<ConnectionStatusChangedMessage>>()
                             .AddTransient<IDeviceViewModel, DeviceViewModel>()
              ;
        }

    }
}
