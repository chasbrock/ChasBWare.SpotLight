using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class RecentArtistsViewModel 
               : BaseRecentViewModel<IArtistViewModel>, 
                 IRecentArtistsViewModel
    {
      
        public RecentArtistsViewModel(IServiceProvider serviceProvider,
                                      ISearchArtistsViewModel searchViewModel,
                                      IPlayerControlViewModel playerControlViewModel,
                                      IMessageService<FindItemMessage> findItemMessageService,
                                      IMessageService<ActiveArtistChangedMessage> activeArtistChangedMessageService)
                    : base(serviceProvider, searchViewModel, SorterHelper.GetArtistSorters())
        {
            findItemMessageService.Register(OnFindItem);
            activeArtistChangedMessageService.Register(OnSetActiveArtist);
            PlayerControlViewModel = playerControlViewModel;
            LoadSettings();
        }

    
        public IPlayerControlViewModel PlayerControlViewModel { get; }

        public bool ShowResults 
        {
            get => SelectedItem != null ;
        }

        protected override void SelectedItemChanged(IArtistViewModel? selectedItem) 
        {
            base.SelectedItemChanged(selectedItem);
            Notify(nameof(ShowResults));
        }

        protected override void LoadRecentItems()
        {
            var task = _serviceProvider.GetService<ILoadRecentArtistTask>();
            task?.Execute(this);
        }

        protected override void DeleteItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                var task = _serviceProvider.GetService<IRemoveArtistTask>();
                task?.Execute( this,  SelectedItem.Id);
            }
        }

        protected override void InitialiseSelectedItem(IArtistViewModel item)
        {
            LoadAlbums(item);
        }

        private IArtistViewModel? AddItemToList(Artist artist, DateTime lastAccessed)
        {
            var viewModel = Items.FirstOrDefault(a => a.Model.Id == artist.Id);
            if (viewModel == null)
            {
                viewModel = _serviceProvider.GetService<IArtistViewModel>();
                if (viewModel != null)
                {
                    viewModel.Model = artist;
                    Items.Add(viewModel);
                }
             }

            return viewModel;
        }

        private void LoadAlbums(IArtistViewModel item)
        {
            if (item!.LoadStatus == LoadState.NotLoaded)
            {
                item!.LoadStatus = LoadState.Loading;
                var task = _serviceProvider.GetService<IArtistAlbumsLoaderTask>();
                task?.Execute(item);
            }
        }

        private void OnFindItem(FindItemMessage message)
        {
            if (message.Payload.ItemType == PageType.Albums)
            {
                var albumViewModel = Items.FirstOrDefault(a => a.Id == message.Payload.Id);
                if (albumViewModel != null)
                {
                    albumViewModel.LastAccessed = DateTime.Now;
                    SelectedItem = albumViewModel;
                    return;
                }

//                var task = _serviceProvider.GetService<IFindAlbumTask>();
//                task?.Execute(this, message.Payload.Id); 
             }
        }      

        private void OnSetActiveArtist(ActiveArtistChangedMessage message)
        {
            SelectedItem = AddItemToList(message.Payload, DateTime.Now);
            UpdateSorting();
        }
    }
}
