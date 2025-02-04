using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks;

public class FindArtistTask(IServiceProvider _serviceProvider,
                            IDispatcher _dispatcher,
                            ISpotifyArtistRepository _spotifyArtistRepo,
                            IArtistRepository _artistRepo)
           : IFindArtistTask
{
    public void Execute(IRecentArtistsViewModel viewModel, string artistId)
    {
        Task.Run(() => RunTask(viewModel, artistId));
    }

    private async void RunTask(IRecentArtistsViewModel viewModel, string artistId)
    {
        if (string.IsNullOrWhiteSpace(artistId))
        { 
            return; 
        }

        Artist? artist = await _artistRepo.FindArtist(artistId);
        if (artist == null)
        {
            artist = await _spotifyArtistRepo.FindArtist(artistId);
        }

        if (artist == null)
        {
            return;
        }
        var artistViewModel = _serviceProvider.GetRequiredService<IArtistViewModel>();
        artistViewModel.Model = artist;

        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Add(artistViewModel);
            viewModel.RefreshView();
            viewModel.SelectedItem = artistViewModel;
        });
    }
}
