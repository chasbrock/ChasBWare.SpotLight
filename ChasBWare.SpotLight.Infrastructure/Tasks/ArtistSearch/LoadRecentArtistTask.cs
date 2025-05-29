using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

public class LoadRecentArtistTask(IServiceProvider _serviceProvider,
                                  IDispatcher _dispatcher,
                                  ISearchItemRepository _searchRepo)
            : ILoadRecentArtistTask
{
    public void Execute(IRecentArtistsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IRecentArtistsViewModel viewModel)
    {
        var items = _searchRepo.GetArtists();
        if (items.Count > 0)
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();

                foreach (var item in items)
                {
                    var artistViewModel = _serviceProvider.GetRequiredService<IArtistViewModel>();
                    artistViewModel.Model = item;
                    viewModel.Items.Add(artistViewModel);
                }

                viewModel.LoadStatus = LoadState.Loaded;
                viewModel.RefreshView();
            });
        }
    }
}
