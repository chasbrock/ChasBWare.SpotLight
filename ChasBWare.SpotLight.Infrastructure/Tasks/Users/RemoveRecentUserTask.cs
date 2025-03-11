using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class RemoveRecentUserTask(IDispatcher _dispatcher,
                                 ISearchItemRepository _searchRepo)
           : IRemoveRecentUserTask
{

    public void Execute(IRecentUserViewModel viewModel, IUserViewModel item)
    {
        Task.Run(() => RunTask(viewModel, item));
    }

    public void Execute(IRecentUserViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IRecentUserViewModel viewModel)
    {
        if (_searchRepo.RemoveUsers())
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                viewModel.SelectedItem = null;
                viewModel.RefreshView();
            });
        }
    }

    private void RunTask(IRecentUserViewModel viewModel, IUserViewModel item)
    {
        if (_searchRepo.RemoveUser(item.Id))
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

