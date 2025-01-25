using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class RecentAlbumsViewModel : BaseRecentViewModel<IPlaylistViewModel>, IRecentAlbumsViewModel
    {
        public RecentAlbumsViewModel(IServiceProvider serviceProvider,
                                     ISearchAlbumsViewModel searchViewModel,
                                     IMessageService<ActiveAlbumChangedMessage> activeAlbumChangedMessageService)

            : base(serviceProvider, searchViewModel, SorterHelper.GetPlaylistSorters())
        {
            activeAlbumChangedMessageService.Register(OnSetActiveAlbum);
        }

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
            // TODO
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

        private void OnSetActiveAlbum(ActiveAlbumChangedMessage message)
        {
            SelectedItem = AddItemToList(message.Payload, DateTime.Now);
        }

        public void ExecuteLibrayCommand(IPlaylistViewModel? selectedItem)
        {
            
        }
    }
}
