using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Popups;
using ChasBWare.SpotLight.Infrastructure.Utility;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class TrackListViewModel : Notifyable, ITrackListViewModel
    {
        private readonly IPopupService _popupService;

        private ITrackViewModel? _selectedItem = null;
        private LoadState _loadStatus = LoadState.NotLoaded;
        private string _name = string.Empty;
        private bool _showHatedTracks = false;
       
        public TrackListViewModel(IPopupService popupService) 
        {
            _popupService = popupService;

            OpenTrackPopupCommand = new Command<ITrackViewModel>(t=>OpenTrackPopupMenu(t));
        }

        public ICommand OpenTrackPopupCommand { get; }
        public ObservableCollection<ITrackViewModel> Items { get; } = [];
                
        public ITrackViewModel? SelectedItem
        {
            get => _selectedItem;
            set => SetField(ref _selectedItem, value);
        }

        public bool ShowHatedTracks
        {
            get => _showHatedTracks;
            set { SetField(ref _showHatedTracks, value); } 
        }

        public LoadState LoadStatus
        {
            get => _loadStatus;
            set => SetField(ref _loadStatus, value);
        }

        public string PlaylistName
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private void OpenTrackPopupMenu(ITrackViewModel track)
        {
            _popupService.ShowPopup<TrackMenuViewModel>(onPresenting: vm => vm.SetTrack(track));
        }


    }
}
