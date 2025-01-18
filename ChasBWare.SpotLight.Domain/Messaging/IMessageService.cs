namespace ChasBWare.SpotLight.Domain.Messaging
{
    public interface IMessageService<T>
    {
        public void Register(Action<T> callback);
        public void SendMessage(T message);
    }
}
