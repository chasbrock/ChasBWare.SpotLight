using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface ISpotifyPlayerController
    {
    
        /// <summary>
        /// retreave the currently playing track from spotify
        /// </summary>
        /// <returns></returns>
        Task<PlayingTrack?> GetCurrentPlayingTrack();

        /// <summary>
        /// get the spotify queue
        /// </summary>
        /// <returns></returns>
        Task<Tuple<Track, List<Track>>> GetQueue();

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
        Task<PlayingTrack?> SkipNext();

        /// <summary>
        /// skip forward
        /// </summary>
        /// <returns>the track id that is actually playing</returns>
        Task<PlayingTrack?> SkipPrevious();

    }
}