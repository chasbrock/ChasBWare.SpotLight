using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Users;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Users;

public class FindUserTask(IServiceProvider _serviceProvider,
                            IDispatcher _dispatcher,
                            ISpotifyUserRepository _spotifyUserRepo,
                            IUserRepository _userRepo)
           : IFindUserTask
{
    public void Execute(IRecentUserViewModel viewModel, string UserId)
    {
        Task.Run(() => RunTask(viewModel, UserId));
    }

    private void RunTask(IRecentUserViewModel viewModel, string UserId)
    {
        if (string.IsNullOrWhiteSpace(UserId))
        {
            return;
        }

        var UserViewModel = viewModel.Items.FirstOrDefault(a => a.Model.Id == UserId);
        User? User = UserViewModel?.Model;
        if (User == null)
        {
            User = _userRepo.FindUser(UserId);
            if (User == null)
            {
                User = _spotifyUserRepo.FindUser(UserId);
            }
        }

        if (User == null)
        {
            return;
        }

        UserViewModel = _serviceProvider.GetRequiredService<IUserViewModel>();
        UserViewModel.Model = User;

        _dispatcher.Dispatch(() =>
        {
            viewModel.SearchViewModel.Items.Clear();
            viewModel.Items.Add(UserViewModel);
            viewModel.RefreshView();
            viewModel.SelectedItem = UserViewModel;
        });
    }
}
