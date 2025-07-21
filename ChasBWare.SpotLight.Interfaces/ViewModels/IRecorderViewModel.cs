using System.Windows.Input;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.ViewModels;

/// <summary>
/// view model for playlist recorder screen
/// </summary>
public interface IRecorderViewModel : IPlaylistViewModel
{
    void AddTracks(List<Track> list);
}
