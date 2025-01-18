using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Infrastructure.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChasBWare.SpotLight.Infrastructure.ViewModels
{
    public class TrackListViewModel : Notifyable, ITrackListViewModel
    {
        private ITrackViewModel? _selectedItem = null;
        private LoadState _loadStatus = LoadState.NotLoaded;
        private string _name = string.Empty;
        private bool _showHatedTracks = false;

        public ICommand ShowHatedTracksCommand { get; set; } = new Command<TrackListViewModel>(o => o.ShowHatedTracks = !o.ShowHatedTracks);

       // [WriteableDataList]
        public ObservableCollection<ITrackViewModel> Items { get; } = [];
                
        public ITrackViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetField(ref _selectedItem, value))
                {
                    foreach (var track in Items)
                    {
                        track.IsSelected = track == _selectedItem;
                    }
                }
            }
        }

        public bool ShowHatedTracks
        {
            get => _showHatedTracks;
            set { SetField(ref _showHatedTracks, value); } //TODO add filtering
        }

        public LoadState LoadStatus
        {
            get => _loadStatus;
            set => SetField(ref _loadStatus, value);
        }

        //[WriteableFileName]
        public string PlaylistName
        {
            get => _name;
            set => SetField(ref _name, value);
        }
    }
}
