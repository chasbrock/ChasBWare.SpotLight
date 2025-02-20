using ChasBWare.SpotLight.Definitions.Messaging;

using Microsoft.Extensions.Logging;

namespace ChasBWare.SpotLight.Infrastructure.Services
{
    public class MessageService<T> : IMessageService<T> 
    {
        private readonly ILogger<MessageService<T>> _logger;
        private List<Func<T, Continue>> _callbacks = [];

        public MessageService(ILogger<MessageService<T>> logger)
        {
            _logger = logger;
        }

        public void Register(Func<T, Continue> callback)
        {
            if (!_callbacks.Contains(callback))
            {
                _callbacks.Add(callback);
            }
        }


        public Continue SendMessage(T message)
        {
            if (message == null)
            {
                return Continue.Yes;
            }

            foreach (var callback in _callbacks)
            {
                try
                {
                    if  (callback.Invoke(message) == Continue.No) 
                    {
                        return Continue.No;
                    }
                }
                catch (Exception ex)
                {
                    _callbacks.Remove(callback);
                    _logger.LogError(ex, "Removed handler from MessageService<{serviceType} to {callbackType}", typeof(T), callback?.Target?.GetType());
                }
            }

            return Continue.Yes;
        }
    }
}
