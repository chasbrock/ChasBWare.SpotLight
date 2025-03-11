using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class SearchForUserTask(IServiceProvider _serviceProvider,
                               IDispatcher _dispatcher,
                               ISpotifyUserRepository _userRepository)
     : ISearchForUserTask
{
    public void Execute(ISearchUserViewModel viewModel)
    {
        Task.Run(() => RunTask(viewModel));
    }

    private void RunTask(ISearchUserViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.SearchText))
        {
            return;
        }

        var user = _userRepository.FindUser(viewModel.SearchText);
        _dispatcher.Dispatch(() =>
        {
            viewModel.Items.Clear();
            if (user != null)
            {
                var userViewModel = _serviceProvider.GetRequiredService<IUserViewModel>();
                userViewModel.Model = user;
                viewModel.Items.Add(userViewModel);

            }
            viewModel.IsPopupOpen = viewModel.Items.Count > 0;
            viewModel.RefreshView();
        });
    }
}

