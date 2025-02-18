using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Definitions.Messaging
{
    public class ConnectionStatusChangedMessageArgs(ConnectionStatus status, string? message = null)
    {
        public ConnectionStatus ConnectionStatus { get; } = status; 
        public string? Message { get; } = message;

    }

    public class ConnectionStatusChangedMessage : IMessage<ConnectionStatusChangedMessageArgs>
    {
        public ConnectionStatusChangedMessage(ConnectionStatus status, string? message = null)
        {
            Payload = new ConnectionStatusChangedMessageArgs(status, message);
        }

        public ConnectionStatusChangedMessageArgs Payload { get; }
        public bool Success { get; set; } = true;
    }
}
