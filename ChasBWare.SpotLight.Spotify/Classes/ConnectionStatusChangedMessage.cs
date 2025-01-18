using ChasBWare.SpotLight.Definitions.Messaging;
using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Spotify.Classes
{

    public class ConnectionStatusChangedMessageArgs(ConnectionStatus status)
    {
        public ConnectionStatus ConnectionStatus { get; } = status;
    }

    public class ConnectionStatusChangedMessage: IMessage<ConnectionStatusChangedMessageArgs>
    {
        public ConnectionStatusChangedMessage(ConnectionStatus status)
        {
            Payload = new ConnectionStatusChangedMessageArgs(status);
        }

        public ConnectionStatusChangedMessageArgs Payload { get; }
    }
}
