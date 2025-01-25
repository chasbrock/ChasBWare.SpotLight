using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Interfaces
{
    public interface ISpotifyPlayerController
    {
        /// <summary>
        /// get list of available devices
        /// </summary>
        /// <returns></returns>
        Task<List<SpotifyAPI.Web.Device>> GetAvailableDevices();

        /// <summary>
        /// make this the active device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>true if success</returns>
        Task<bool> SetDeviceAsActive(string deviceId);
       
        /// <summary>
        /// retreave the currently playing track from spotify
        /// </summary>
        /// <returns></returns>
        Task<CurrentlyPlaying> GetCurrentPlayingTrack();
        
        /// <summary>
        /// get the spotify queue
        /// </summary>
        /// <returns></returns>
        Task<QueueResponse> GetQueue();
        
        /// <summary>
        /// pause currently playing track
        /// </summary>
        /// <returns></returns>
        Task<bool> PausePlayback();

        /// <summary>
        /// start playing album / playlist at specific track
        /// </summary>
        /// <param name="playlistUri"></param>
        /// <param name="trackOffset"></param>
        /// <returns></returns>
        Task<bool> StartPlayback(string playlistUri, int trackOffset);

        /// <summary>
        /// resume track that was paused at given offset in ms
        /// </summary>
        /// <param name="trackUri"></param>
        /// <param name="position"></param>
        /// <returns>the track id that is actually playing</returns>
        Task<bool> ResumePlayback(string trackUri, int position);
       
        /// <summary>
        /// set volume for current device
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        Task<bool> SetVolume(int volume);
        
        /// <summary>
        /// skip forward
        /// </summary>
        /// <returns>the track id that is actually playing</returns>
        Task<CurrentlyPlaying> SkipNext();

        /// <summary>
        /// skip forward
        /// </summary>
        /// <returns>the track id that is actually playing</returns>
        Task<CurrentlyPlaying> SkipPrevious();
       
    }
}