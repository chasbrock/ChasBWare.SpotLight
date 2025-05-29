using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Device;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Device;

public class SyncToDeviceTask(IDispatcher _dispatcher,
                              ILibraryRepository _libraryRepository,
                              ISpotifyDeviceRepository _spotifyDeviceRepository,
                              IMessageService<FindItemMessage> _findItemMessageService)
           : ISyncToDeviceTask
{

    public void Execute(IPlayerControlViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IPlayerControlViewModel viewModel)
    {
        var context = _spotifyDeviceRepository.GetCurrentContext();

        if (context != null)
        {
            viewModel.CurrentDevice.Device = context.Device;
            viewModel.TrackPlayerService.UpdateNowPlaying(context.Track);

            // check to see if the playlist has changed externally
            var playlistId = _libraryRepository.FindAllPlaylistsForTrack(viewModel.TrackPlayerService.CurrentTrack?.Id,
                                                                         viewModel.TrackPlayerService.PriorTrack?.Id);
            if (playlistId != viewModel.TrackPlayerService.CurrentPlaylistId)
            {
                // open the playlist in library
                viewModel.TrackPlayerService.CurrentPlaylistId = playlistId;
                _dispatcher.Dispatch(() =>
                {
                    _findItemMessageService.SendMessage(new FindItemMessage(PageType.Library, playlistId));
                });
            }
            else
            {
                viewModel.CurrentDevice.Device.IsActive = false;
            }
            viewModel.IsSyncing = false;
        }

    }
}

