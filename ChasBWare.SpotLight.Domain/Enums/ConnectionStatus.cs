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

public static class ConnectionStatusHelper
{
    public static bool IsActiveState(this ConnectionStatus status)
    {
        return status == ConnectionStatus.Connected || status == ConnectionStatus.TokenExpired;
    }
}