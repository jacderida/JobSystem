namespace JobSystem.Framework.Messaging
{
    /// <summary>
    /// Describes a Queue to which you can dispatch messages.
    /// </summary>
    public interface IQueueDispatcher<T>
    {
        /// <summary>
        /// Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message to enqueue.</param>
        void Enqueue(T message);
    }
}