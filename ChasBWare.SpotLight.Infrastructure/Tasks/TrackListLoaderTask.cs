using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class TrackListLoaderTask(IServiceProvider _serviceProvider,
                                     IDispatcher _dispatcher,
                                     ITrackRepository _trackRepository,
                                     ISpotifyTrackRepository _spotifyTrackRepository,
                                     IHatedService _hatedService)
               : ITrackListLoaderTask
    {
        public void Execute(IPlaylistViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private void RunTask(IPlaylistViewModel viewModel)
        {
            if (!_hatedService.Initialised)
            {
                _hatedService.Refresh();
            }

            //try to get from db first
            var tracks = _trackRepository.GetPlaylistTracks(viewModel.Id);
            if (tracks.Count == 0)
            {
                tracks = _spotifyTrackRepository.GetPlaylistTracks(viewModel.Id, viewModel.PlaylistType);
                _trackRepository.AddTracksToPlaylist(viewModel.Id, tracks);
            }

            _dispatcher.Dispatch(() =>
            {
                viewModel.TracksViewModel.Items.Clear();
                if (tracks != null)
                {
                    foreach (var track in tracks)
                    {
                        var trackViewModel = _serviceProvider.GetRequiredService<ITrackViewModel>();
                        trackViewModel.Playlist = viewModel;
                        trackViewModel.Track = track;
                        trackViewModel.IsHated = _hatedService.GetIsHated(track.Id);
                        viewModel.TracksViewModel.Items.Add(trackViewModel);
                    }
                    
                    viewModel.TracksViewModel.LoadStatus = LoadState.Loaded;
                }
            });
        }
    }
}
