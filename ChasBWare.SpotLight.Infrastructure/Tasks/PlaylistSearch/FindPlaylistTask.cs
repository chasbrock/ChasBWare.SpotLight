using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.PlaylistSearch;

public class FindPlaylistTask(IServiceProvider _serviceProvider,
                            IDispatcher _dispatcher,
                            ISpotifyPlaylistRepository _spotifyRepo,
                            ILibraryRepository _libraryRepo)
           : IFindPlaylistTask
{
   
    public void Execute(IRecentViewModel<IPlaylistViewModel> viewModel, string playlistId, PlaylistType playlistType)
    {
        Task.Run(() => RunTask(viewModel, playlistId, playlistType));
    }

    private void RunTask(IRecentViewModel<IPlaylistViewModel> viewModel, string playlistId, PlaylistType playlistType)
    {
        if (string.IsNullOrWhiteSpace(playlistId))
        { 
            return; 
        }

        var playlistViewModel = viewModel.Items.FirstOrDefault(a => a.Model.Id == playlistId);
        Playlist? playlists = playlistViewModel?.Model;
        if (playlists == null)
        {
            playlists = _libraryRepo.FindPlaylist(playlistId);
            if (playlists == null)
            {
                playlists = _spotifyRepo.FindPlaylist(playlistId, playlistType);
            }
        }

        if (playlists == null)
        {
            return;
        }

        playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
        playlistViewModel.Model = playlists;

        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Add(playlistViewModel);
            viewModel.RefreshView();
            viewModel.SelectedItem = playlistViewModel;
        });
    }
}
