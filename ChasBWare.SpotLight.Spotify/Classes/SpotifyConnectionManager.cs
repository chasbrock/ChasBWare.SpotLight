using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace ChasBWare.SpotLight.Spotify.Classes
{

    public class SpotifyConnectionManager(ILogger _logger,
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
            // if this is the first call try loading AccessToken from secure storage    
            if (Status == ConnectionStatus.NotConnected)
            {
                _session.ClientId = await SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.ClientId));
                _session.ClientSecret = await SecureStorage.Default.GetAsync(nameof(ISpotyConnectionSession.ClientSecret));
                _session.AccessToken = await SecureStorage.GetAsync(nameof(ISpotyConnectionSession.AccessToken));
                 Status = string.IsNullOrWhiteSpace(_session.AccessToken) ? ConnectionStatus.Unauthorised : ConnectionStatus.Connected;
            }
            switch (Status)
            {
                case ConnectionStatus.Unauthorised:
                    AuthoriseConnection();
                    break;
                case ConnectionStatus.TokenExpired:
                    _session.AccessToken = null;
                    await SecureStorage.SetAsync(nameof(ISpotyConnectionSession.AccessToken), "");
                    await GetAccessToken();
                    break;
            }

            var i = 10;
            while (Status != ConnectionStatus.Connected && i-- > 0)
            {
                Thread.Sleep(1000);
            }
            if (i > 0)
            {
                return _session.GetClient();
            }
            throw new Exception("Timed out connecting to spotify");
        }
        
        protected async Task GetAccessToken()
        {
            try
            {
                _logger.LogInformation("Getting spotify access token");
                var config = SpotifyClientConfig.CreateDefault();

                if (_session.ClientId == null || _session.ClientSecret == null)
                {
                    throw new SystemException("Big problem ClientId / Cleint Secret not set");
                }

                var request = new ClientCredentialsRequest(_session.ClientId, _session.ClientSecret);
                var response = await new OAuthClient(config).RequestToken(request);
                _session.AccessToken = response.AccessToken;
                await SecureStorage.SetAsync(nameof(ISpotyConnectionSession.AccessToken), response.AccessToken);
                _logger.LogInformation("Spotify access token updated");
                Status = ConnectionStatus.Connected;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to connect to spotify");
                Status = ConnectionStatus.Faulted;
            }
        }
        

        private async void AuthoriseConnection()
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

                        if (_session.ClientId == null || _session.ClientSecret == null)
                        {
                            throw new SystemException("Big problem ClientId / Cleint Secret not set");
                        }
                        AuthorizationCodeTokenRequest tokenRequest = new(_session.ClientId,
                                                                         _session.ClientSecret,
                                                                         response.Code,
                                                                         callBack);
                        AuthorizationCodeTokenResponse tokenResponse = await new OAuthClient(config).RequestToken(tokenRequest);
                        _session.AccessToken = tokenResponse.AccessToken;
                        if (_session.AccessToken != null)
                        {
                            await SecureStorage.SetAsync(nameof(ISpotyConnectionSession.AccessToken), _session.AccessToken);
                            Status = ConnectionStatus.Connected;
                        }
                        else
                        {
                            Status = ConnectionStatus.Unauthorised;
                        }
                    };
                await server.Start();

                if (_session.ClientId == null) 
                {
                    throw new SystemException("Big problem - not client Id set");
                }

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
                await Browser.Default.OpenAsync(loginRequest.ToUri(), BrowserLaunchMode.SystemPreferred);

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
