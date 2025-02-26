using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks.AlbumSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.Tasks.PlaylistSearch;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class RecentPlaylistsViewModel 
           : BaseRecentViewModel<IPlaylistViewModel>, 
             IRecentPlaylistsViewModel
{
    private readonly ILibraryViewModel _library;

    public RecentPlaylistsViewModel(IPopupService popupService,
                                    IServiceProvider serviceProvider,
                                    IPlayerControlViewModel playerControlViewModel,
                                    ISearchPlaylistsViewModel searchViewModel,
                                    IMessageService<FindItemMessage> findItemMessageService,
                                    IMessageService<ActiveItemChangedMessage> activeAlbumChangedMessageService,
                                    ILibraryViewModel library)
        : base(popupService, serviceProvider, playerControlViewModel, searchViewModel, SorterHelper.GetPlaylistSorters())
    {
        _library = library;
        findItemMessageService.Register(OnFindItem);
        activeAlbumChangedMessageService.Register(OnSetActivePlaylist);
    }

    public override PageType PageType => PageType.Playlists;

    protected override void LoadItems()
    {
         var task = _serviceProvider.GetRequiredService<ILoadRecentPlaylistTask>();
        task.Execute(this, PlaylistType.Playlist);
    }

    protected override void SelectedItemChanged(IPlaylistViewModel? oldItem, IPlaylistViewModel? newItem)
    {
        if (oldItem != null)
        {
            oldItem.IsSelected = false;
        }

        if (newItem != null)
        {
            newItem.IsSelected = true;
            newItem.IsExpanded = true;
        }
    }

    protected override void OpenPopup()
    {
        _popupService.ShowPopup<RecentPlaylistPopupViewModel>(onPresenting: vm => vm.SetItem(this, SelectedItem));
    }

    private IPlaylistViewModel? AddItemToList(Playlist playlist)
    {
        var viewModel = Items.FirstOrDefault(a => a.Model.Id == playlist.Id);
        if (viewModel == null)
        {
            var task = _serviceProvider.GetRequiredService<IAddRecentPlaylistTask>();
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

    private Continue OnFindItem(FindItemMessage message)
    {
        if (message.Payload.PageType == PageType.Playlists)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Payload.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
            }
            else
            {
                var task = _serviceProvider.GetRequiredService<IFindPlaylistTask>();
                task.Execute(this, message.Payload.Id, PlaylistType.Playlist);
            }
            return Continue.No;
        }
        return Continue.Yes;
    }

    private Continue OnSetActivePlaylist(ActiveItemChangedMessage message)
    {
        if (message.Payload.PageType == PageType.Playlists)
        {
            if (message.Payload.Model is Playlist playlist)
            {
                SelectedItem = AddItemToList(playlist);
            }
            else
            {
                SelectedItem = null;
            }
            RefreshView();
        }
        return Continue.Yes;
    }
}
