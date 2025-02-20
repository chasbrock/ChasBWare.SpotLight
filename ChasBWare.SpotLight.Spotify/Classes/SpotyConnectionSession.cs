using ChasBWare.SpotLight.Domain.Enums;
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
        private  SpotifyClientConfig _config  = SpotifyClientConfig.CreateDefault();

        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public int RedirectPort { get; set; } = defaultPort;
        public string RedirectUrl { get; set; } = $"http://localhost:{defaultPort}/callback";
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }

    
        /// <summary>
        /// get a new spotify client
        /// </summary>
        /// <returns>spotify client</returns>
        /// <exception cref="InvalidDataException">thrown if access token not set</exception>
        public SpotifyClient GetClient()
        {
            if (!string.IsNullOrEmpty(AccessToken))
            {
                _config = SpotifyClientConfig.CreateDefault(AccessToken);
            }
            else
            {
                _config = SpotifyClientConfig.CreateDefault();
            }
        
            var client =  new SpotifyClient(_config);
            return client;
        }

        public ConnectionStatus UpdateTokens(string accessToken, string refreshToken)
        {
            RefreshToken = refreshToken;
            SecureStorage.SetAsync(nameof(ISpotyConnectionSession.RefreshToken), refreshToken);
            return UpdateToken(accessToken);
        }

        public ConnectionStatus UpdateToken(string accessToken)
        {
            AccessToken = accessToken;
            SecureStorage.SetAsync(nameof(ISpotyConnectionSession.AccessToken), accessToken);
      
            return string.IsNullOrWhiteSpace(AccessToken) ? ConnectionStatus.Unauthorised : ConnectionStatus.Connected;
        }

        public void ClearAccessTokens() 
        {
            AccessToken = null;
            RefreshToken = null;
        }

        public ConnectionStatus RestoreTokens()
        {
            var clientId = SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.ClientId)).Result;
            var clientSecret = SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.ClientSecret)).Result;
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new SystemException("Fatal Error ClientIs / CleitnSecret not set");
            }
            ClientId = clientId;
            ClientSecret = clientSecret;

            AccessToken = SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.AccessToken)).Result;
            RefreshToken = SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.RefreshToken)).Result;
            
            return string.IsNullOrWhiteSpace(AccessToken) ? ConnectionStatus.Unauthorised : ConnectionStatus.TokenExpired;
        }

        public ICollection<string> GetScopes()
        {
            return [Scopes.AppRemoteControl,
                    Scopes.PlaylistModifyPrivate,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistReadPrivate,
                    Scopes.UgcImageUpload,
                    Scopes.UserFollowModify,
                    Scopes.UserFollowRead,
                    Scopes.UserLibraryModify,
                    Scopes.UserLibraryRead,
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                    Scopes.UserReadPlaybackPosition,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserReadPrivate,
                    Scopes.UserReadRecentlyPlayed];
        }
    }
}
