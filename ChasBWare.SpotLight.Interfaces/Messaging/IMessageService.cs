namespace ChasBWare.SpotLight.Definitions.Messaging
{
    public interface IMessageService<T>
    {
        public void Register(Action<T> callback);
        public bool SendMessage(T message);
    }
}
