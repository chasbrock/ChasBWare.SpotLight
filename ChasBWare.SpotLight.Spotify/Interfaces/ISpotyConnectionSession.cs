using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Interfaces
{
    public interface ISpotyConnectionSession
    {  /// <summary>
       /// default details for connecting to spotify
       /// </summary>
       /// <see cref="https://johnnycrazy.github.io/SpotifyAPI-NET/docs/configuration"/>
        public SpotifyClientConfig GetDefaultConfig { get; }

        /// <summary>
        /// unique app id from spotify
        /// </summary>
        /// <see cref="https://developer.spotify.com/dashboard"/>
        public string ClientId { get; set; }

        /// <summary>
        /// unique id created for me by spotify
        /// </summary>
        /// <see cref="https://developer.spotify.com/dashboard"/>
        public string ClientSecret { get; set; }

        public int RedirectPort { get; set; }

        public string RedirectUrl { get; set; }

        string AccessToken { get; set; }

        SpotifyClient GetClient();

    }
}