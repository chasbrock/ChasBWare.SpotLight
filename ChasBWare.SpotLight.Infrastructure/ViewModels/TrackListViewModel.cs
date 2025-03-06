using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class TrackListViewModel : Notifyable, ITrackListViewModel
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly INavigator _navigator;

    private ITrackViewModel? _selectedItem = null;
    private LoadState _loadStatus = LoadState.NotLoaded;
    private bool _showHatedTracks = false;

    public TrackListViewModel(IServiceProvider serviceProvider,
                              INavigator navigator,
                              IPopupService popupService)
    {
        _serviceProvider = serviceProvider;
        _navigator = navigator;
     
        OpenPopupCommand = new Command<ITrackViewModel>(track => popupService.ShowPopup<TrackPopupViewModel>(onPresenting: vm => vm.SetTrack(Playlist, track)));
        OpenArtistCommand = new Command<string>(id => NavigateToArtist(id));
    }

    public ICommand OpenPopupCommand { get; }
    public ICommand OpenArtistCommand { get; }

    public ObservableCollection<ITrackViewModel> Items { get; } = [];
    public IPlaylistViewModel? Playlist { get; set; }
     
    public ITrackViewModel? SelectedItem
    {
        get => _selectedItem;
        set => SetField(ref _selectedItem, value);
    }

    public bool ShowHatedTracks
    {
        get => _showHatedTracks;
        set => SetField(ref _showHatedTracks, value);  
    }

    public LoadState LoadStatus
    {
        get => _loadStatus;
        set => SetField(ref _loadStatus, value);
    }

    private void NavigateToArtist(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }

        var messageService = _serviceProvider.GetRequiredService<IMessageService<FindItemMessage>>();
        messageService.SendMessage(new FindItemMessage(PageType.Artists, id));

        _navigator.NavigateTo(PageType.Artists);
    }

}
