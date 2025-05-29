using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class LibraryViewModel
                   : BaseGroupedListViewModel<IPlaylistViewModel>,
                     ILibraryViewModel
{
    private readonly IPlaylistViewModelProvider _playlistProvider;
   
    public LibraryViewModel(IServiceProvider serviceProvider,
                            IPopupService popupService,
                            IPlayerControlViewModel playerControlViewModel,
                            ISearchLibraryViewModel searchViewModel,
                            IPlaylistViewModelProvider playlistProvider,
                            IMessageService<FindItemMessage> findItemMessageService,
                            IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                            IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
         : base(serviceProvider, GrouperHelper.GetPlaylistGroupers())
    {
        _playlistProvider = playlistProvider;
        _playlistProvider.ExistsInlibrary = Exists;

        PlayerControlViewModel = playerControlViewModel;
        SearchViewModel = searchViewModel;

        OpenPopupCommand = new Command<ITrackViewModel>(t => popupService.ShowPopup<LibraryPopupViewModel>());

        activeItemChangedMessageService.Register(OnActiveItemChanged);
        findItemMessageService.Register(OnFindItem);
        currentTrackChangedMessage.Register(OnTrackChangedMessage);
        Initialise();

    }

    public IPlayerControlViewModel PlayerControlViewModel { get; }
    public ISearchLibraryViewModel SearchViewModel { get; }
    public ICommand OpenPopupCommand { get; }


    public bool Exists(string? playlistId)
    {
        return !string.IsNullOrEmpty(playlistId) &&
                Items.Any(pl => pl.Id == playlistId);
    }

    public void AddItems(IEnumerable<Playlist> items)
    {
        var added = false;
        foreach (var item in items.Where(m => !Items.Any(vm => vm.Model.Id == m.Id)))
        {
            Items.Add(_playlistProvider.CreatePlaylist(item, true));
            added = true;
        }

        if (added)
        {
            RefreshView();
        }
    }

    protected override void SelectedItemChanged(IPlaylistViewModel? oldItem, IPlaylistViewModel? newItem)
    {
        base.SelectedItemChanged(oldItem, newItem);
        if (oldItem != null)
        {
            oldItem.IsSelected = false;
        }

        if (newItem != null)
        {
            newItem.IsExpanded = true;
            newItem.IsSelected = true;
            newItem.LastAccessed = DateTime.Now;
            var task = _serviceProvider.GetService<IUpdateLastAccessedTask>();

            task?.Execute(newItem.Model);
        }
    }

    private void Initialise()
    {
        var loadPlaylistsTask = _serviceProvider.GetRequiredService<ILibraryLoaderTask>();
        loadPlaylistsTask.Load(this);
    }

    private void OnTrackChangedMessage(CurrentTrackChangedMessage message)
    {
        // if there is no playlist selected then try and find the one playing
        if (SelectedItem == null)
        {
            SelectedItem = Items.FirstOrDefault(pl => pl.Name.Equals(message.PlaylistName, StringComparison.CurrentCultureIgnoreCase));
        }

        // try to find the track
        foreach (var playList in Items.Where(pl => pl.TracksViewModel.LoadStatus == LoadState.Loaded))
        {
            playList.ShowPlayingTrack(message.TrackId, message.State);
        }
    }

    private void OnFindItem(FindItemMessage message)
    {
        if (message.PageType == PageType.Library)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
                message.Completed = true;
            }
        }
    }

    private void OnActiveItemChanged(ActiveItemChangedMessage message)
    {
        if (message.PageType == PageType.Library)
        {
            if (message.Model is Playlist playlist)
            {
                SelectedItem = Items.FirstOrDefault(pl => pl.Id == playlist.Id);
            }
            else
            {
                SelectedItem = null;
            }
        }
    }

}
