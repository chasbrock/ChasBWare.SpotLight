using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Services;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class TrackListLoaderTask(ITrackRepository _trackRepository,
                                     ISpotifyTrackRepository _spotifyTrackRepository,
                                     IServiceProvider _serviceProvider,
                                     IHatedService _hatedService,
                                     IDispatcher _dispatcher)
               : ITrackListLoaderTask
    {
        public async void Execute(IPlaylistViewModel viewModel)
        {
            //try to get from db first
            var tracks = await _trackRepository.GetPlaylistTracks(viewModel.Id);
            if (tracks.Count == 0)
            {
                tracks = await _spotifyTrackRepository.GetPlaylistTracks(viewModel.Id, viewModel.PlaylistType);
                await _trackRepository.AddTracksToPlaylist(viewModel.Id, tracks);
            }

            if (tracks != null)
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.TracksViewModel.Items.Clear();

                    foreach (var track in tracks)
                    {
                        var trackViewModel = _serviceProvider.GetService<ITrackViewModel>();
                        if (trackViewModel != null)
                        {   
                            trackViewModel.Playlist = viewModel;
                            trackViewModel.Track = track;
                            trackViewModel.IsHated = _hatedService.IsHated(track.Id);
                            viewModel.TracksViewModel.Items.Add(trackViewModel); 
                        }
                    }
                    viewModel.TracksViewModel.LoadStatus = LoadState.Loaded;
                });
            }
        }
    }
}
