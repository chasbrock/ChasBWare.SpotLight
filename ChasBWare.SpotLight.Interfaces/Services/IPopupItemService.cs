using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Infrastructure.Popups;

namespace ChasBWare.SpotLight.Infrastructure.Interfaces.Services;

public interface IPopupItemService
{
    void AddMenuItem(IPopupMenuViewModel popup, IPlaylistViewModel playlist, PopupActivity action);
    void AddMenuItem(IPopupMenuViewModel popup, ITrackViewModel track, PopupActivity action);
    void AddMenuItem(IPopupMenuViewModel popup, ILibraryViewModel library, PopupActivity action);
}
