using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class RecentAlbumsViewModel : BaseRecentViewModel<IPlaylistViewModel>, IRecentAlbumsViewModel
    {
        private readonly IPopupService _popupService;
        
        public RecentAlbumsViewModel(IPopupService popupService,
                                     IServiceProvider serviceProvider,
                                     IPlayerControlViewModel playerControlViewModel,
                                     ISearchAlbumsViewModel searchViewModel,
                                     IMessageService<FindItemMessage> findItemMessageService,
                                     IMessageService<ActiveAlbumChangedMessage> activeAlbumChangedMessageService)

            : base(serviceProvider, searchViewModel, SorterHelper.GetPlaylistSorters())
        {
            _popupService = popupService;
            findItemMessageService.Register(OnFindItem);
            activeAlbumChangedMessageService.Register(OnSetActiveAlbum);
            PlayerControlViewModel = playerControlViewModel;
            LoadSettings();
            OpenPopupCommand = new Command(track => OpenPopup());
        }

        private void OpenPopup()
        {
            _popupService.ShowPopup<RecentAlbumPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
        }

        public ICommand OpenPopupCommand { get; }
        public IPlayerControlViewModel PlayerControlViewModel { get; }

        protected override void LoadRecentItems()
        {
            var task = _serviceProvider.GetService<ILoadRecentPlaylistTask>();
            task?.Execute(this, PlaylistType.Album);
        }

        protected override void DeleteItem()
        {
            // TODO
        }

        protected override void InitialiseSelectedItem(IPlaylistViewModel item)
        {
            item.IsExpanded = true;
        }

        private IPlaylistViewModel? AddItemToList(RecentPlaylist playlist, DateTime lastAccessed)
        {
            var viewModel = Items.FirstOrDefault(a => a.Model.Id == playlist.Id);
            if (viewModel == null)
            {
                viewModel = _serviceProvider.GetService<IPlaylistViewModel>();
                if (viewModel != null)
                {
                    viewModel.Model = playlist;
                    Items.Add(viewModel);
                }
            }
            
            if (viewModel != null)
            {
                var updateLastAccessedTask = _serviceProvider.GetService<IUpdateLastAccessedTask>();
                updateLastAccessedTask?.Execute(viewModel.Id, lastAccessed, false);
            }

            return viewModel;
        }


        private void OnFindItem(FindItemMessage message)
        {
            if (message.Payload.PageType == PageType.Albums)
            {
                var viewModel = Items.FirstOrDefault(a => a.Id == message.Payload.Id);
                if (viewModel != null)
                {
                    viewModel.LastAccessed = DateTime.Now;
                    SelectedItem = viewModel;
                    return;
                }

               // var task = _serviceProvider.GetService<IFindAlbumTask>();
               // task?.Execute(this, message.Payload.Id); 
            }
        }



        private void OnSetActiveAlbum(ActiveAlbumChangedMessage message)
        {
            SelectedItem = AddItemToList(message.Payload, DateTime.Now);
            RefreshView();
        }
    }
}
