using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class RecentAlbumsViewModel 
           : BaseRecentViewModel<IPlaylistViewModel>, 
             IRecentAlbumsViewModel
{
    private readonly ILibraryViewModel _library;
    public RecentAlbumsViewModel(IPopupService popupService,
                                 IServiceProvider serviceProvider,
                                 IPlayerControlViewModel playerControlViewModel,
                                 ISearchAlbumsViewModel searchViewModel,
                                 IMessageService<FindItemMessage> findItemMessageService,
                                 IMessageService<ActiveAlbumChangedMessage> activeAlbumChangedMessageService,
                                 ILibraryViewModel library)
         : base(popupService,serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetPlaylistSorters())
    {
        _library = library;
        findItemMessageService.Register(OnFindItem);
        activeAlbumChangedMessageService.Register(OnSetActiveAlbum);
    }

    protected override void LoadItems()
    {
        var task = _serviceProvider.GetService<ILoadRecentPlaylistTask>();
        task?.Execute(this, PlaylistType.Album);
    }
        
    protected override void InitialiseSelectedItem(IPlaylistViewModel item)
    {
        item.IsExpanded = true;
        LoadItem(item);
    }
    
    protected override void OpenPopup()
    {
        _popupService.ShowPopup<RecentAlbumPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
    }

    private IPlaylistViewModel? AddItemToList(Playlist playlist)
    {
        var viewModel = Items.FirstOrDefault(a => a.Model.Id == playlist.Id);
        if (viewModel == null)
        {
            var task = _serviceProvider.GetRequiredService<IAddRecentAlbumTask>();
            task.Execute(this, playlist);
        }
        else 
        {
            var task = _serviceProvider.GetRequiredService<IUpdateLastAccessedTask>();
            task.Execute(viewModel.Model);
            viewModel.InLibrary = _library.Exists(viewModel.Id);
        }
        return viewModel;
    }

    private void LoadItem(IPlaylistViewModel item)
    {
        if (item!.TracksViewModel.LoadStatus == LoadState.NotLoaded)
        {
            var task = _serviceProvider.GetService<IUpdateLastAccessedTask>();
            task?.Execute(item.Model);
        }
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
        SelectedItem = AddItemToList(message.Payload);
        RefreshView();
    }
}
