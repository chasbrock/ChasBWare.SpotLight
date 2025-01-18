using ChasBWare.SpotLight.Definitions.Messaging;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
   public class Message<T>(T payload) : IMessage<T>
    {
        public T Payload { get; } = payload;
    }
}
