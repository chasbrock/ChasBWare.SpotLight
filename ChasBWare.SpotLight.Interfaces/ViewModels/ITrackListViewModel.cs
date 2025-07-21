using System.Collections.ObjectModel;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels.Tracks;

public interface ITrackListViewModel
{
    //[WriteableDataList]
    ObservableCollection<ITrackViewModel> Items { get; }
    ITrackViewModel? SelectedItem { get; set; }

    /// <summary>
    ///state of loading for this list
    /// </summary>
    LoadState LoadStatus { get; set; }

    //[WriteableFileName]
    IPlaylistViewModel? Playlist { get; set; }

    void DeleteSelectedItem();
    void MoveSelectedTrackDown();
    void MoveSelectedTrackUp();
}
