namespace ChasBWare.SpotLight.Domain.Messaging
{
    public class Message<T>(T payload) : IMessage<T>
    {
        public T Payload { get; } = payload;
    }

}
