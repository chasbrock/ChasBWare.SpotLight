using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.ArtistSearch;

public class RemoveRecentArtistTask(IDispatcher _dispatcher,
                                    ISearchItemRepository _searchRepo) 
           : IRemoveRecentArtistTask
{

    public void Execute(IRecentArtistsViewModel viewModel, IArtistViewModel item)
    {
        Task.Run(() => RunTask(viewModel, item));
    }

    public void Execute(IRecentArtistsViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IRecentArtistsViewModel viewModel)
    {
        if (_searchRepo.RemoveArtists())
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }

    private void RunTask(IRecentArtistsViewModel viewModel, IArtistViewModel item)
    {
        if (_searchRepo.RemoveArtist(item.Id))
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Remove(item);
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }
}

