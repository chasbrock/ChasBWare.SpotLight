using ChasBWare.SpotLight.Spotify.Interfaces;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Spotify.Classes
{
    /// <summary>
    /// class to hold session based info for spotify
    /// </summary>
    public class SpotyConnectionSession : ISpotyConnectionSession
    {
        private const int defaultPort = 8888;

        private string _accessToken = string.Empty;

        /// <summary>
        /// default details for connecting to spotify
        /// </summary>
        public SpotifyClientConfig GetDefaultConfig { get; set; } = SpotifyClientConfig.CreateDefault();

        /// <summary>
        /// unique app id from spotify
        /// </summary>
        public string ClientId { get; set; } = "f8eb5326c4b3445a9dcd079ee11812a0";

        /// <summary>
        /// unique id created for me by spotify
        /// </summary>
        public string ClientSecret { get; set; } = "bae2baff23fa4204b50efb260df14fc2";

        public int RedirectPort { get; set; } = defaultPort;

        public string RedirectUrl { get; set; } = $"http://localhost:{defaultPort}/callback";

        /// <summary>
        /// token created by OAuth
        /// </summary>
        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                GetDefaultConfig = SpotifyClientConfig.CreateDefault(_accessToken);
            }
        }

        /// <summary>
        /// get a new spotify client
        /// </summary>
        /// <returns>spotify client</returns>
        /// <exception cref="InvalidDataException">thrown if access token not set</exception>
        public SpotifyClient GetClient()
        {
            return string.IsNullOrEmpty(_accessToken)
                ? throw new InvalidDataException("access token has not been set")
                : new SpotifyClient(GetDefaultConfig);
        }

    }
}
