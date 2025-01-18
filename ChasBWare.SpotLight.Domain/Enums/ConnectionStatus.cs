namespace ChasBWare.SpotLight.Domain.Enums
{
    public enum ConnectionStatus
    {
        NotConnected,
        Authorising,
        Authorised,
        Connected,
        TokenExpired,
        Faulted
    }
}