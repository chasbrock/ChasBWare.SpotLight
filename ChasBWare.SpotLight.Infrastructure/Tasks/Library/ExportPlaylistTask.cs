using System.Text;
using ChasBWare.SpotLight.Definitions.Repositories;
using ChasBWare.SpotLight.Definitions.Tasks.Library;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Infrastructure.Tasks.Library;

public class ExportPlaylistTask(IDispatcher _dispatcher,
                                ITrackRepository _trackRepository)
            : IExportPlaylistTask
{
    public void ExportPlaylist(string playlistId)
    {
        Task.Run(() => RunExportPlaylist(playlistId));
    }

    public void ExportTrack(string trackId)
    {
        Task.Run(() => RunExportTrack(trackId));
    }

    private void RunExportPlaylist(string playlistId)
    {
        StringBuilder text = new();
        var tracks = _trackRepository.GetPlaylistTracks(playlistId);
        tracks.ForEach(t=>text.AppendLine(WriteTrack(t)));
        CopyToClipboard(text.ToString());
    }

    private  void RunExportTrack(string trackId)
    {
        var track = _trackRepository.GetTrack(trackId);
        if (track != null)
        {
            CopyToClipboard(WriteTrack(track));
        }
    }

    private static string WriteTrack(Track track)
    {
        return $"{track.Id}\t{track.Name}\t{track.Duration.MSecsToMinsSecs()}\t{track.Album}\t{track.Artists.RepackOwners(';')}\t{track.Uri}";
    }

    private void CopyToClipboard(string text)
    {
        _dispatcher.Dispatch(() =>
           {
               Clipboard.SetTextAsync(text);
           });
    }
}
