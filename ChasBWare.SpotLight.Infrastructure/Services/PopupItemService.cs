using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Messaging;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Popups;

namespace ChasBWare.SpotLight.Infrastructure.Services;

public class PopupItemService(IServiceProvider _serviceProvider)
           : IPopupItemService
{
    public void AddMenuItem(IPopupMenuViewModel popup, IPlaylistViewModel playlist, PopupActivity activity)
    {
        switch (activity)
        {
            case PopupActivity.Save:
                var library = _serviceProvider.GetRequiredService<ILibraryViewModel>();
                var save = !library.Exists(playlist.Id);
                var caption = save ? $"Add  '{playlist.Name}'" : $"Remove '{playlist.Name}'";
                var toolTip = save ? $"Adds current {playlist.PlaylistType} to your library, but not to you Spotify profile"
                                   : $"Removes current {playlist.PlaylistType} from your library, but not to you Spotify profile";

                popup.AddItem(PopupGroup.Library,
                              activity,
                              caption: caption,
                              toolTip: toolTip,
                              action: (t) =>
                              {
                                  var task = _serviceProvider.GetRequiredService<ITransferToLibraryTask>();
                                  task.Execute(playlist, save);
                                  popup.Close();
                              });
                break;



            case PopupActivity.Play:
                popup.AddItem(PopupGroup.Playlist,
                              activity,
                              caption: $"Play '{playlist.Name}'",
                              toolTip: $"Start playing current {playlist.PlaylistType}",
                              action: (t) =>
                              {
                                  var messageService = _serviceProvider.GetRequiredService<IMessageService<PlayPlaylistMessage>>();
                                  messageService.SendMessage(new PlayPlaylistMessage(playlist.Model, 0));
                                  popup.Close();
                              });
                break;
           
                case PopupActivity.Copy:
                popup.AddItem(PopupGroup.Track,
                           activity,
                           caption: $"Copy {playlist.Name}",
                           toolTip: $"Copy playlist to clipboard",
                           action: (t) =>
                           {
                               if (playlist != null)
                               {
                                   var task = _serviceProvider.GetRequiredService<IExportPlaylistTask>();
                                   task.ExportPlaylist(playlist.Id);
                               }
                               popup.Close();
                           });
                break;

            default:
                return;
        }
    }

    public void AddMenuItem(IPopupMenuViewModel popup, ITrackViewModel track, PopupActivity activity)
    {
        switch (activity)
        {
            case PopupActivity.Play:
                popup.AddItem(PopupGroup.Track,
                              activity,
                              caption: $"Play {track.Name}",
                              toolTip: $"Start playing current track",
                              action: (t) =>
                              {
                                  if (track.Playlist != null)
                                  {
                                      var offset = track.Playlist.TracksViewModel.Items.ToList().FindIndex(tm => tm.Id == track.Id);
                                      var messageService = _serviceProvider.GetRequiredService<IMessageService<PlayPlaylistMessage>>();
                                      messageService.SendMessage(new PlayPlaylistMessage(track.Playlist.Model, offset));
                                  }
                                  popup.Close();
                              });
                break;

            case PopupActivity.Hate:
                popup.AddItem(PopupGroup.Library,
                              activity,
                              caption: track.IsHated ? $"Unhate '{track.Name}'" : $"Hate '{track.Name}'",
                              toolTip: "Hated tracks will never be played by this app, does not effect Spotify App",
                              action: (t) =>
                              {
                                  var task = _serviceProvider.GetRequiredService<ISetHatedTrackTask>();
                                  task.Execute(track);
                                  popup.Close();
                              });
                break;

            case PopupActivity.Copy:
                popup.AddItem(PopupGroup.Track,
                           activity,
                           caption: $"Copy {track.Name}",
                           toolTip: $"Copy track to clipboard",
                           action: (t) =>
                           {
                               if (track != null)
                               {
                                   var task = _serviceProvider.GetRequiredService<IExportPlaylistTask>();
                                   task.ExportTrack(track.Id);
                               }
                               popup.Close();
                           });
                break;
        }
    }

    public void AddMenuItem(IPopupMenuViewModel popup, ILibraryViewModel library, PopupActivity activity)
    {
        switch (activity)
        {
            case PopupActivity.ExpandAll:
                popup.AddItem(activity,
                              caption: "Expand All",
                              action: (t) =>
                              {
                                  library.ExpandAll();
                                  popup.Close();
                              });
                break;

            case PopupActivity.CollapseAll:
                popup.AddItem(activity,
                              caption: "Collapse All",
                              action: (t) =>
                              {
                                  library.CollapseAll();
                                  popup.Close();
                              });
                break;

            case PopupActivity.Refresh:
                popup.AddItem(PopupGroup.Library,
                              activity,
                              caption: "Refresh library",
                              action: (t) =>
                              {
                                  var task = _serviceProvider.GetRequiredService<ILibraryLoaderTask>();
                                  task.Refresh(library);
                                  popup.Close();
                              });
                break;

        }
    }
}