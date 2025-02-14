using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class RecentPlaylistsViewModel : BaseRecentViewModel<IPlaylistViewModel>, IRecentPlaylistsViewModel
    {
 
        public RecentPlaylistsViewModel(IPopupService popupService,
                                        IServiceProvider serviceProvider,
                                        IPlayerControlViewModel playerControlViewModel,
                                        ISearchPlaylistsViewModel searchViewModel,
                                        IMessageService<ActivePlaylistChangedMessage> activeAlbumChangedMessageService)
            : base(popupService, serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetPlaylistSorters())
        {
            activeAlbumChangedMessageService.Register(OnSetActivePlaylist);
        }

        protected override void LoadItems()
        {
            var task = _serviceProvider.GetRequiredService<ILoadRecentPlaylistTask>();
            task.Execute(this, Domain.Enums.PlaylistType.Playlist);
        }
           
        protected override void InitialiseSelectedItem(IPlaylistViewModel item)
        {
            item.IsExpanded = true;
        }

        protected override void OpenPopup()
        {
           // _popupService.ShowPopup<RecentAlbumPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
        }

        private IPlaylistViewModel? AddItemToList(Playlist playlist, DateTime? lastAccessed)
        {
            var viewModel = Items.FirstOrDefault(a => a.Model.Id == playlist.Id);
            if (viewModel == null)
            {
                viewModel = _serviceProvider.GetService<IPlaylistViewModel>();
                if (viewModel != null)
                {
                    viewModel.Model = playlist;

                    if (lastAccessed == null)
                    {
                        var task = _serviceProvider.GetRequiredService<IUpdateLastAccessedTask>();
                        task.Execute(viewModel.Model);
                    }
                    viewModel.LastAccessed = lastAccessed ?? DateTime.Now;
                    Items.Add(viewModel);
                }
            }
            return viewModel;
        }


        private void OnSetActivePlaylist(ActivePlaylistChangedMessage message)
        {
            SelectedItem = AddItemToList(message.Payload, null);
        }
    }
}
