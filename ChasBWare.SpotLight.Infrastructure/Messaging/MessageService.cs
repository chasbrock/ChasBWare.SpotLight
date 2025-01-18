using ChasBWare.SpotLight.Definitions.Messaging;

using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class MessageService<T> : IMessageService<T>
    {
        private readonly ILogger _logger;
        private List<Action<T>> _callbacks = [];

        public MessageService(ILogger logger)
        {
            _logger = logger;
        }

        public void Register(Action<T> callback)
        {
            if (!_callbacks.Contains(callback))
            {
                _callbacks.Add(callback);
            }
        }


        public void SendMessage(T message)
        {
            if (message == null)
            {
                return;
            }

            foreach (var callback in _callbacks)
            {
                try
                {
                    callback.Invoke(message);
                }
                catch (Exception ex)
                {
                    _callbacks.Remove(callback);
                    _logger.LogError(ex, $"Removed handler from MessageService<{typeof(T)} to {callback?.Target?.GetType()}");
                }
            }
        }
    }
}
