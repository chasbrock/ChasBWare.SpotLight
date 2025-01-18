namespace ChasBWare.SpotLight.Domain.Messaging
{
    public interface IMessage<T>
    {
        public T Payload { get; }
    }
}
