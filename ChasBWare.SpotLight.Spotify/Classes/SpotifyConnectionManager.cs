using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace ChasBWare.SpotLight.Spotify.Classes
{

    public class SpotifyConnectionManager(ILogger<SpotifyConnectionManager> _logger,
                                          ISpotyConnectionSession _session,
                                          IMessageService<ConnectionStatusChangedMessage> _messageService) 
               : ISpotifyConnectionManager
    {
        private ConnectionStatus _status = ConnectionStatus.NotConnected;

        public ConnectionStatus Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    _messageService.SendMessage(new ConnectionStatusChangedMessage(_status));
                }
            }
        }

        public async Task<SpotifyClient> GetClient()
        {
            switch (Status)
            {
                case ConnectionStatus.NotConnected:
                    AuthoriseConnection();
                    break;
                case ConnectionStatus.TokenExpired:
                    await GetAccessToken();
                    break;
            }

            while (Status != ConnectionStatus.Connected)
            {
                Thread.Sleep(1000);
            }
            return _session.GetClient();

        }
        
        protected async Task GetAccessToken()
        {
            try
            {
                _logger.LogInformation("Getting spotify access token");
                var config = SpotifyClientConfig.CreateDefault();
                var request = new ClientCredentialsRequest(_session.ClientId, _session.ClientSecret);
                var response = await new OAuthClient(config).RequestToken(request);
                _session.AccessToken = response.AccessToken;
                _logger.LogInformation("Spotify access token updated");
                Status = ConnectionStatus.Connected;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to spotify");
                Status = ConnectionStatus.Faulted;
            }
        }
        

        private void AuthoriseConnection()
        {
            try
            {
                _logger.LogInformation("Getting spotify authorising connection");
                Status = ConnectionStatus.Authorising;

                var callBack = new Uri(_session.RedirectUrl);
                var config = SpotifyClientConfig.CreateDefault();
                var server = new EmbedIOAuthServer(callBack, _session.RedirectPort);
                server.AuthorizationCodeReceived += async (sender, response) =>
                    {
                        await server.Stop();
                        AuthorizationCodeTokenRequest tokenRequest = new(_session.ClientId,
                                                                         _session.ClientSecret,
                                                                         response.Code,
                                                                         callBack);
                        AuthorizationCodeTokenResponse tokenResponse = await new OAuthClient(config).RequestToken(tokenRequest);
                        _session.AccessToken = tokenResponse.AccessToken;

                        Status = ConnectionStatus.Connected;
                    };
                server.Start();

                LoginRequest loginRequest = new(server.BaseUri,
                                                _session.ClientId,
                                                LoginRequest.ResponseType.Code)
                {
                    Scope = [Scopes.AppRemoteControl,
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
                             Scopes.UserReadRecentlyPlayed]
                };
                BrowserUtil.Open(loginRequest.ToUri());

                _logger.LogInformation("Getting spotify authorising connection");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to spotify");
                Status = ConnectionStatus.TokenExpired;
                throw;
            }
        }

    }
}
