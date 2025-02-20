namespace ChasBWare.SpotLight.Definitions.Messaging
{
    public enum Continue
    {
        Yes,
        No
    }

    /// <summary>
    /// simple delegated method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageService<T>
    {
       
        /// <summary>
        /// register message callback client.
        /// </summary>
        /// <param name="callback">func</param>
        public void Register(Func<T, Continue> callback);

        /// <summary>
        /// broadcast message to clients.
        /// if the call returns No it signifies that the clients has dealt with
        /// the message and no more clients will be called.
        /// if call returns Yes then subsequent clients will be called
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true is a client processed the message successfully. </returns>
        public Continue SendMessage(T message);
    }
}
