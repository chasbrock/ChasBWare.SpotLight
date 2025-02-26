using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.ArtistSearch;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public partial class LibraryViewModel 
                   : BaseGroupedListViewModel<IPlaylistViewModel>,
                     ILibraryViewModel
{
    public LibraryViewModel(IServiceProvider serviceProvider,
                            IPopupService popupService,       
                            IPlayerControlViewModel playerControlViewModel,
                            ISearchLibraryViewModel searchViewModel,
                            IMessageService<FindItemMessage> findItemMessageService,
                            IMessageService<ActiveItemChangedMessage> activeItemChangedMessageService,
                            IMessageService<CurrentTrackChangedMessage> currentTrackChangedMessage)
         : base(serviceProvider, GrouperHelper.GetPlaylistGroupers())
    {
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
    
    public override void RefreshView()
    {
        base.RefreshView();
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
        loadPlaylistsTask.Execute(this);
    }

    private Continue OnTrackChangedMessage(CurrentTrackChangedMessage message)
    {
        // if there is no playlist selected then try and find the one playing
        if (SelectedItem == null)
        {
            SelectedItem = Items.FirstOrDefault(pl => pl.Name.Equals(message.Payload.AlbumName, StringComparison.CurrentCultureIgnoreCase));
        }

        // try to find the track
        foreach (var playList in Items.Where(pl => pl.TracksViewModel.LoadStatus == LoadState.Loaded))
        {
            var track = playList.TracksViewModel.Items.FirstOrDefault(t => t.Id == message.Payload.TrackId);
            if (track != null)
            {
                track.Status = message.Payload.State;
            }
        }
        return Continue.Yes;
    }

    private Continue OnFindItem(FindItemMessage message)
    {
        if (message.Payload.PageType == PageType.Library)
        {
            var viewModel = Items.FirstOrDefault(a => a.Id == message.Payload.Id);
            if (viewModel != null)
            {
                viewModel.LastAccessed = DateTime.Now;
                SelectedItem = viewModel;
                return Continue.No;
            }
        }
        return Continue.Yes;
    }

    private Continue OnActiveItemChanged(ActiveItemChangedMessage message) 
    {
        if (message.Payload.PageType == PageType.Library)
        {
            if (message.Payload.Model is Playlist playlist)
            {
                SelectedItem = Items.FirstOrDefault(pl => pl.Id == playlist.Id);
            }
            else
            {
                SelectedItem = null;
            }
        }
        return Continue.Yes;
    }

}
