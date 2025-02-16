using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

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

    private void RunTask(IRecentArtistsViewModel viewModel, string artistId)
    {
        if (string.IsNullOrWhiteSpace(artistId))
        { 
            return; 
        }

        var artistViewModel = viewModel.Items.FirstOrDefault(a => a.Model.Id == artistId);
        Artist? artist = artistViewModel?.Model;
        if (artist == null)
        {
            artist = _artistRepo.FindArtist(artistId);
            if (artist == null)
            {
                artist = _spotifyArtistRepo.FindArtist(artistId);
            }
        }

        if (artist == null)
        {
            return;
        }
        
        artistViewModel = _serviceProvider.GetRequiredService<IArtistViewModel>();
        artistViewModel.Model = artist;

        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Add(artistViewModel);
            viewModel.RefreshView();
            viewModel.SelectedItem = artistViewModel;
        });
    }
}
