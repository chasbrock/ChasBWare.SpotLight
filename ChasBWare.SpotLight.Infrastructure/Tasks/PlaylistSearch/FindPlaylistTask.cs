using System.Reflection;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.PlaylistSearch;

public class FindPlaylistTask(IPlaylistViewModelProvider _playlistProvider,
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
        Playlist? playlist = playlistViewModel?.Model;
        if (playlist == null)
        {
            playlist = _libraryRepo.FindPlaylist(playlistId);
            if (playlist == null)
            {
                playlist = _spotifyRepo.FindPlaylist(playlistId, playlistType);
            }
        }

        if (playlist == null)
        {
            return;
        }

        playlistViewModel = _playlistProvider.CreatePlaylist(playlist);
       
        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Add(playlistViewModel);
            viewModel.RefreshView();
            viewModel.SelectedItem = playlistViewModel;
        });
    }
}
