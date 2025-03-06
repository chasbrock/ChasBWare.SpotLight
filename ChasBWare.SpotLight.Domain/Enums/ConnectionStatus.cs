namespace ChasBWare.SpotLight.Domain.Enums;

public enum ConnectionStatus
{
    NotInitialised,
    NotConnected,
    Unauthorised,
    Authorising,
    Connected,
    TokenExpired
}