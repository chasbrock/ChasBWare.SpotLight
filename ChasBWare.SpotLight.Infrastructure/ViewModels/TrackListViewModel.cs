using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels;

public class TrackListViewModel : Notifyable, ITrackListViewModel
{

    private ITrackViewModel? _selectedItem = null;
    private LoadState _loadStatus = LoadState.NotLoaded;
    private bool _showHatedTracks = false;

    public TrackListViewModel(IPopupService popupService)
    {
        OpenPopupCommand = new Command<ITrackViewModel>(track => popupService.ShowPopup<TrackPopupViewModel>(onPresenting: vm => vm.SetTrack(Playlist, track)));
    }

    public ICommand OpenPopupCommand { get; }
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

}
