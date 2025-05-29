using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

public interface ITrackListViewModel
{
    //[WriteableDataList]
    public ObservableCollection<ITrackViewModel> Items { get; }
    public ITrackViewModel? SelectedItem { get; set; }

    /// <summary>
    ///state of loading for this list
    /// </summary>
    public LoadState LoadStatus { get; set; }

    //[WriteableFileName]
    IPlaylistViewModel? Playlist { get; set; }
}
