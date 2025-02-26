using ChasBWare.SpotLight.Definitions.ViewModels.Tracks;
using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface ISpotifyPlayerController
    {
        /// <summary>
        /// cleare queue, then add tracks, and start playling
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        Task<PlayingTrack?> Enqueue(IEnumerable<ITrackViewModel> tracks);

        /// <summary>
        /// retreave the currently playing track from spotify
        /// </summary>
        /// <returns></returns>
        Task<PlayingTrack?> GetCurrentPlayingTrack();

        /// <summary>
        /// get the spotify queue
        /// </summary>
        /// <returns></returns>
        Task<List<Track>> GetQueue();

        /// <summary>
        /// pause currently playing track
        /// </summary>
        /// <returns></returns>
        Task<PlayingTrack?> PausePlayback();

        /// <summary>
        /// start playing album / playlist at specific track
        /// </summary>
        /// <param name="playlistUri"></param>
        /// <param name="trackOffset"></param>
        /// <returns></returns>
        Task<PlayingTrack?> StartPlayback(string playlistUri, int trackOffset);

        /// <summary>
        /// resume track that was paused at given offset in ms
        /// </summary>
        /// <param name="trackUri"></param>
        /// <param name="position"></param>
        /// <returns>the track id that is actually playing</returns>
        Task<PlayingTrack?> ResumePlayback(string trackUri, int position);

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