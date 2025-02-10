namespace ChasBWare.SpotLight.Domain.Enums
{
    public enum ConnectionStatus
    {
        NotConnected,
        Unauthorised,
        Authorising,
        Authorised,
        Connected,
        TokenExpired,
        Faulted
    }
}