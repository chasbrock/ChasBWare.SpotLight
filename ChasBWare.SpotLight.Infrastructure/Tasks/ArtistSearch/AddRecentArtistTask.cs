using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

public class AddRecentArtistTask(IServiceProvider _serviceProvider,
                                 IDispatcher _dispatcher,
                                 ISearchItemRepository _searchRepo)
           : IAddRecentArtistTask
{
    public void Execute(IRecentArtistsViewModel viewModel, Artist model)
    {
        Task.Run(()=>RunTask(viewModel, model));
    }

    private void RunTask(IRecentArtistsViewModel viewModel, Artist model)
    {
        if (_searchRepo.AddArtist(model))
        {
            _dispatcher.Dispatch(() =>
            {
                var item = _serviceProvider.GetRequiredService<IArtistViewModel>();
                item.Model = model;
                viewModel.Items.Add(item);
                viewModel.RefreshView();
            });
        }
    }
}
