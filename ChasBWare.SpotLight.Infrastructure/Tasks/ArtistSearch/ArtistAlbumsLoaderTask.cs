using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

public class ArtistAlbumsLoaderTask(IServiceProvider _serviceProvider,
                                    IDispatcher _dispatcher,
                                    ISpotifyArtistRepository _spotifyArtistRepo,
                                    IArtistRepository _artistRepo)
           : IArtistAlbumsLoaderTask
{
    public void Execute(IArtistViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IArtistViewModel viewModel)
    {
        // try to get from database
        var albums = _artistRepo.LoadArtistAlbums(viewModel.Id);
        if (albums.Count == 0)
        {
            albums = _spotifyArtistRepo.LoadArtistAlbums(viewModel.Id);
            _artistRepo.StoreArtistAndAlbums(viewModel.Model, albums);
        }

        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Clear();
            foreach (var album in albums)
            {
                var item = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                item.Model = album;
                item.IsExpanded = false;
                viewModel.Items.Add(item);
            }
            viewModel.LoadStatus = LoadState.Loaded;
        });
    }
}
