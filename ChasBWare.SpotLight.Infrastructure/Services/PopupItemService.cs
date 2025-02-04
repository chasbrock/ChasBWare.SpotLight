using ChasBWare.SpotLight.Definitions.Enums;
using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Definitions.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Interfaces.Services;
using ChasBWare.SpotLight.Infrastructure.Messaging;
using ChasBWare.SpotLight.Infrastructure.Popups;
using Microsoft.Maui.Controls;

namespace ChasBWare.SpotLight.Infrastructure.Services;

public class PopupItemService(IServiceProvider _serviceProvider) 
           : IPopupItemService
{
    public void AddMenuItem(IPopupMenuViewModel popup, IPlaylistViewModel playlist, PopupAction action)
    {
        var group = PopupGroup.Playlist;
        switch (action)
        {
            case PopupAction.Profile:
                var save = !playlist.IsSaved;
                var caption = save ? $"Add  '{playlist.Name}'" : "Remove '{playlist.Name}'";
                var toolTip = save ? $"Adds current {playlist.PlaylistType} to your library, but not to you Spotify profile"
                                   : $"Removes current {playlist.PlaylistType} from your library, but not to you Spotify profile";

                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: caption,
                              toolTip: toolTip,
                              action: (t) =>
                              {
                                  var task = _serviceProvider.GetRequiredService<ISetPlaylistSavedStatus>();
                                  task.Execute(playlist, save);
                                  popup.Close();
                              });
                break;

          

            case PopupAction.Play:
                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: $"Play '{playlist.Name}'",
                              toolTip: $"Start playing current {playlist.PlaylistType}",
                              action: (t) =>
                              {
                                  var messageService = _serviceProvider.GetRequiredService<IMessageService<PlayPlaylistMessage>>();
                                  messageService.SendMessage(new PlayPlaylistMessage(playlist, 0));
                                  popup.Close();
                              });
                break;

            case PopupAction.AddToQueue:
                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: $"Queue '{playlist.Name}'",
                              toolTip: $"Add current {playlist.PlaylistType} to the Queue",
                              action: (t) =>
                              {
                                  var messageService = _serviceProvider.GetRequiredService<IMessageService<AddToQueueMessage>>();
                                  messageService.SendMessage(new AddToQueueMessage(playlist));
                                  popup.Close();
                              });
                break;
                 
            default:
                return;
        }
    }

    public void AddMenuItem(IPopupMenuViewModel popup, ITrackViewModel track, PopupAction action)
    {
        var group = PopupGroup.Track;
        switch (action) 
        {
            case PopupAction.Play:
                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: $"Play {track.Name}",
                              toolTip: $"Start playing current track",
                              action: (t) =>
                              {
                                  if (track.Playlist != null)
                                  {
                                      var offset = track.Playlist.TracksViewModel.Items.ToList().FindIndex(tm => tm.Id == track.Id);
                                      var messageService = _serviceProvider.GetRequiredService<IMessageService<PlayPlaylistMessage>>();
                                      messageService.SendMessage(new PlayPlaylistMessage(track.Playlist, offset));
                                  }
                                  popup.Close();
                              });
                break;

            case PopupAction.AddToQueue:
                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: $"Queue '{track.Name}'",
                              toolTip: $"Add current track to queue",
                              action: (t) =>
                              {
                                   var messageService = _serviceProvider.GetRequiredService<IMessageService<AddToQueueMessage>>();
                                   messageService.SendMessage(new AddToQueueMessage(track));
                                   popup.Close();
                              });
                break;

            case PopupAction.Hate:
                popup.AddItem(group.ToString(), $"{group}_{action}",
                              caption: track.IsHated ? "Unhate" : "Hate",
                              toolTip: "Hated tracks will never be played by this app, does not effect Spotify App",
                              action: (t) =>
                              {
                                  var task = _serviceProvider.GetRequiredService<ISetHatedTrackTask>();
                                  task.Execute(track);
                                  popup.Close();
                              });
                break;
        }
    }

    public void AddMenuItem(IPopupMenuViewModel popup, ILibraryViewModel library, PopupAction action)
    {
        switch (action)
        {
            case PopupAction.ExpandAll:
                popup.AddItem($"{action}",
                              caption: $"Expand All",
                              action: (t) =>
                              {
                                  library.ExpandAll();
                                  popup.Close();
                              });
                break;

            case PopupAction.CollapseAll:
                popup.AddItem($"{action}",
                              caption: $"Collapse All",
                              action: (t) =>
                              {
                                  library.CollapseAll();
                                  popup.Close();
                              });
                break;
        }
    }
}