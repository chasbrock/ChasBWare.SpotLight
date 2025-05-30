namespace ChasBWare.SpotLight.Definitions.Tasks.Library;

public interface IExportPlaylistTask 
{
    void ExportPlaylist(string playlistId);
    void ExportTrack(string trackId);
}

