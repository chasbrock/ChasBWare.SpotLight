using System.Linq;
using System.Runtime.Intrinsics.Arm;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks;

public class SetPlaylistSavedStatus (IDispatcher _dispatcher,
                                     ILibraryViewModel _library,
                                     IRecentItemRepository _recentItemsRepo,
                                     IUserRepository _userRepo)
           : ISetPlaylistSavedStatus
{
    public void Execute(IPlaylistViewModel viewModel, bool save)
    {
       
        Task.Run(() => RunTask(viewModel, save));
    }

    private void RunTask(IPlaylistViewModel viewModel, bool save)
    {
        _recentItemsRepo.UpdateLastAccessed(_userRepo.CurrentUserId, viewModel.Id, DateTime.Now, save);
        viewModel.IsSaved = save;
        _dispatcher.Dispatch(() =>
        {
            if (save)
            {
                if (!_library.Items.Any(pl => pl.Id == viewModel.Id))
                {
                    _library.Items.Add(viewModel);
                }
            }
            else 
            {
                _library.Items.Remove(viewModel);
            }
            _library.RefreshView();
        });
    }
}
