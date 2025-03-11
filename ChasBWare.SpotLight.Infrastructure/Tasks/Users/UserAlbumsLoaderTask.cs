using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class UserAlbumsLoaderTask(IPlaylistViewModelProvider _playlistProvider,
                                    IDispatcher _dispatcher,
                                    ISpotifyUserRepository _spotifyUserRepo,
                                    IUserRepository _userRepo)
           : IUserAlbumsLoaderTask
{
    public void Execute(IUserViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IUserViewModel viewModel)
    {
        // try to get from database
        var albums = _userRepo.LoadUserAlbums(viewModel.Id);
        if (albums.Count == 0)
        {
            albums = _spotifyUserRepo.LoadUserPlaylist(viewModel.Id);
            _userRepo.StoreUserAndAlbums(viewModel.Model, albums);
        }
        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Clear();
            foreach (var album in albums)
            {
                viewModel.Items.Add(_playlistProvider.CreatePlaylist(album));
            }
            viewModel.LoadStatus = LoadState.Loaded;
        });
    }
}

