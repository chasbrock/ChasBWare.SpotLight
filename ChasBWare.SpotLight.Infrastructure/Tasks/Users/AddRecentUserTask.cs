using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class AddRecentUserTask(IServiceProvider _serviceProvider,
                                 IDispatcher _dispatcher,
                                 ISearchItemRepository _searchRepo)
           : IAddRecentUserTask
{
    public void Execute(IRecentUserViewModel viewModel, User model)
    {
        Task.Run(() => RunTask(viewModel, model));
    }

    private void RunTask(IRecentUserViewModel viewModel, User model)
    {
        if (_searchRepo.AddUser(model))
        {
            _dispatcher.Dispatch(() =>
            {
                var item = _serviceProvider.GetRequiredService<IUserViewModel>();
                item.Model = model;
                viewModel.Items.Add(item);
                viewModel.SelectedItem = item;
                viewModel.RefreshView();
            });
        }
    }
}

