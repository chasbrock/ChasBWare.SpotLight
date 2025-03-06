using ChasBWare.SpotLight.Domain.Messaging;

namespace ChasBWare.SpotLight.Domain.Services;

public class MessageService<T> : IMessageService<T> where T : IMessage
{
    private List<Action<T>> _callbacks = [];


    public void Register(Action<T> callback)
    {
        if (!_callbacks.Contains(callback))
        {
            _callbacks.Add(callback);
        }
    }

    public bool SendMessage(T message)
    {
        foreach (var callback in _callbacks)
        {
            try
            {
                callback.Invoke(message);
                if (message.Completed)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _callbacks.Remove(callback);
                Console.WriteLine(ex);
            }
        }
        return false;
    }
}
