using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Tasks
{
    public class LoadRecentArtistTask(IArtistRepository _artistRepository,
                                      IUserRepository _userRepository,
                                      IServiceProvider _serviceProvider,
                                      IDispatcher _dispatcher)
                : ILoadRecentArtistTask
    {
        public async void Execute(IRecentArtistsViewModel viewModel)
        {
            var items = await _artistRepository.GetRecentArtists(_userRepository.CurrentUserId);
            if (items.Count == 0)
            {
                _dispatcher.Dispatch(() =>
                {
                    viewModel.Items.Clear();

                    foreach (var item in items)
                    {
                        var artistViewModel = _serviceProvider.GetService<IArtistViewModel>();
                        if (artistViewModel != null)
                        {
                            artistViewModel.Model = item.Item1;
                            artistViewModel.LastAccessed = item.Item2;
                            viewModel.Items.Add(artistViewModel);
                        }
                    }
                    viewModel.LoadStatus = LoadState.Loaded;
                });
            }
        }
    }

}
