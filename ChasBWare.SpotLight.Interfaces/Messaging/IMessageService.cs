namespace ChasBWare.SpotLight.Definitions.Messaging
{
    public interface IMessageService<T>
    {
        public void Register(Action<T> callback);
        public void SendMessage(T message);
    }
}
