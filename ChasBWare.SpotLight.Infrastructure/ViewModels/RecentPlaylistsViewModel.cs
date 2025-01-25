using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class RecentPlaylistsViewModel : BaseRecentViewModel<IPlaylistViewModel>, IRecentPlaylistsViewModel
    {
 
        public RecentPlaylistsViewModel(IServiceProvider serviceProvider,
                                        ISearchPlaylistsViewModel searchViewModel,
                                        IMessageService<ActivePlaylistChangedMessage> activeAlbumChangedMessageService)
            : base(serviceProvider, searchViewModel, SorterHelper.GetPlaylistSorters())
        {
            activeAlbumChangedMessageService.Register(OnSetActivePlaylist);
        }


        protected override void LoadRecentItems()
        {
            var task = _serviceProvider.GetService<ILoadRecentPlaylistTask>();
            task?.Execute(this, Domain.Enums.PlaylistType.Playlist);
        }

        protected override void DeleteItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                var task = _serviceProvider.GetService<ILoadRemoveRecentPlaylistTask>();
                task?.Execute(this, SelectedItem.Id);
            }
        }

        protected override void InitialiseSelectedItem(IPlaylistViewModel item)
        {
            // TODO
        }

        private IPlaylistViewModel? AddItemToList(RecentPlaylist playlist, DateTime? lastAccessed)
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
                        var task = _serviceProvider.GetService<IUpdateLastAccessedTask>();
                        task?.Execute(viewModel.Id, DateTime.Now, true);
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

        public void ExecuteLibrayCommand(IPlaylistViewModel? selectedItem)
        {
            throw new NotImplementedException();
        }
    }
}
