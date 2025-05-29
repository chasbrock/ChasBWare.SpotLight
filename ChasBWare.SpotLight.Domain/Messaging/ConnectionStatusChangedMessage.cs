using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Messaging;

public class ConnectionStatusChangedMessage(ConnectionStatus status)
           : Message()
{
    public ConnectionStatus ConnectionStatus { get; } = status;

}
