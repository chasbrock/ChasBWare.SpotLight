using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class LoadRecentUserTask(IServiceProvider _serviceProvider,
                                  IDispatcher _dispatcher,
                                  ISearchItemRepository _searchRepo)
            : ILoadRecentUserTask
{
    public void Execute(IRecentUserViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(IRecentUserViewModel viewModel)
    {
        var items = _searchRepo.GetUsers();
        if (items.Count > 0)
        {
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();

                foreach (var item in items)
                {
                    var UserViewModel = _serviceProvider.GetRequiredService<IUserViewModel>();
                    UserViewModel.Model = item;
                    viewModel.Items.Add(UserViewModel);
                }

                viewModel.LoadStatus = LoadState.Loaded;
                viewModel.RefreshView();
            });
        }
    }
}
