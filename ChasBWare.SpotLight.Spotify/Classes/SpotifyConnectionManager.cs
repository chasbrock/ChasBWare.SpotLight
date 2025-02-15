using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Domain.Enums;
using ChasBWare.SpotLight.Spotify.Interfaces;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;

namespace ChasBWare.SpotLight.Spotify.Classes
{

    public class SpotifyConnectionManager
               : ISpotifyConnectionManager
    {
        private readonly ILogger _logger;
        private readonly ISpotyConnectionSession _session;
        private readonly IMessageService<ConnectionStatusChangedMessage> _messageService;
        private readonly EmbedIOAuthServer _server;
        private ConnectionStatus _status = ConnectionStatus.NotConnected;
        
        
        public SpotifyConnectionManager(ILogger logger,
                                          ISpotyConnectionSession session,
                                          IMessageService<ConnectionStatusChangedMessage> messageService)
        {
            _logger = logger;
            _session = session;
            _messageService = messageService;
            _server = new EmbedIOAuthServer(new Uri(_session.RedirectUrl), _session.RedirectPort);
            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _server.ErrorReceived += OnErrorReceived;
        }

     
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

        public SpotifyClient GetClient()
        {
            // if this is the first call try loading AccessToken from secure storage    
            if (Status == ConnectionStatus.NotConnected)
            {
                Status = _session.RestoreTokens();
            }

            switch (Status)
            {
                case ConnectionStatus.Unauthorised:
                    AuthoriseConnection();
                    var i = 10;
                    while (Status != ConnectionStatus.Connected && i-- > 0)
                    {
                        Thread.Sleep(1000);
                    }
                    
                    break;
                case ConnectionStatus.TokenExpired:
                    RefreshAccessToken();      
                break;
            }
         
            return _session.GetClient();
        }

        private async void RefreshAccessToken()
        {
            if (!string.IsNullOrWhiteSpace(_session.RefreshToken))
            {
                try
                {
                    var request = new AuthorizationCodeRefreshRequest(_session.ClientId, _session.ClientSecret, _session.RefreshToken);
                    var reply = await new OAuthClient().RequestToken(request);
                    Status = _session.UpdateToken(reply.AccessToken);
                }
                catch (Exception ex) 
                {
                    _logger.LogError("Error refreshing token: {error}", ex.Message);
                }
            }
        }
            
        private async void AuthoriseConnection()
        {
            try
            {
                _logger.LogInformation("Getting spotify authorising connection");
                Status = ConnectionStatus.Authorising;

                await _server.Start();

                if (_session.ClientId == null)
                {
                    throw new SystemException("Big problem - not client Id set");
                }

                var loginRequest = new LoginRequest(_server.BaseUri,
                                                    _session.ClientId,
                                                    LoginRequest.ResponseType.Code)
                {
                    Scope = _session.GetScopes()
                };

                await Browser.Default.OpenAsync(loginRequest.ToUri(), BrowserLaunchMode.SystemPreferred);

                _logger.LogInformation("Getting spotify authorising connection");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to spotify");
                Status = ConnectionStatus.Unauthorised;
                throw;
            }
        }

        private async Task OnErrorReceived(object sender, string error, string? state)
        {
            _logger.LogError("Aborting authorization, error received: {error}", error);
            await _server.Stop();
        }

        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var request = new AuthorizationCodeTokenRequest(_session.ClientId,
                                                             _session.ClientSecret,
                                                             response.Code,
                                                             new Uri(_session.RedirectUrl));

            var reply = await new OAuthClient(config).RequestToken(request);
             _session.UpdateTokens(reply.AccessToken, reply.RefreshToken);

            if (!string.IsNullOrWhiteSpace(_session.AccessToken))
            {
                Status = ConnectionStatus.Connected;
            }
            else
            {
                Status = ConnectionStatus.Unauthorised;
            }
        }

    }
}
