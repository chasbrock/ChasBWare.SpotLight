using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class SearchForAlbumTask(IServiceProvider _serviceProvider, 
                                    IDispatcher _dispatcher, 
                                    ISpotifyPlaylistRepository _playlistRepository)
               : ISearchForAlbumTask
    {
        public void Execute(ISearchAlbumsViewModel viewModel)
        {
            Task.Run(() => RunTask(viewModel));
        }

        private  void RunTask(ISearchAlbumsViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SearchText))
            {
                return;
            }

            var items = _playlistRepository.FindAlbums(viewModel.SearchText);
          
            _dispatcher.Dispatch(() =>
            {
                viewModel.Items.Clear();
                foreach (var item in items)
                {
                    var playlistViewModel = _serviceProvider.GetRequiredService<IPlaylistViewModel>();
                    playlistViewModel.Model = item;
                    playlistViewModel.IsSaved = false;
                    viewModel.Items.Add(playlistViewModel);
                }

                viewModel.IsPopupOpen = viewModel.Items.Count > 0;
            });
        }
    }
}
