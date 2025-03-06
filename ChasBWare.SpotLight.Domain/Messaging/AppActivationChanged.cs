namespace ChasBWare.SpotLight.Domain.Messaging
{
    public class AppActivationChanged(bool isActive)
              : Message()
    {
        public bool IsActive { get; } = isActive;
    }
}
