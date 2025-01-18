namespace ChasBWare.SpotLight.Definitions.Messaging
{
    public interface IMessage<T>
    {
        public T Payload { get; }
    }

}
