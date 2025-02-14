using System.Linq;
using System.Runtime.Intrinsics.Arm;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class TransferToLibraryTask (IDispatcher _dispatcher,
                                    ILibraryViewModel _library,
                                    ILibraryRepository _libraryRepo)
           : ITransferToLibraryTask
{
    public void Execute(IPlaylistViewModel viewModel, bool save)
    {
       
        Task.Run(() => RunTask(viewModel, save));
    }

    private void RunTask(IPlaylistViewModel viewModel, bool save)
    {
        if (_libraryRepo.TransferPlaylistToLibrary(viewModel.Model, save))
        {
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
}
