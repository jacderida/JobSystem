using System;
using System.Collections.Generic;

namespace JobSystem.Framework.Messaging
{
    /// <summary>
    /// Describes an object that can listen to a queue and notify a consumer of any new messages
    /// </summary>
    public interface IQueueListener<T>
    {
        /// <summary>
        /// Starts the IQueueListener listening.
        /// </summary>
        /// <param name="messageProcessingCallback">
        /// The message processing callback to be called when a new message is recieved.
        /// Func[T,T]: Process(ITaskMessage msg) returns the next ITaskMessages to enqueue...
        /// ...OR..NULL if there are no further messages to process.
        /// </param>
        void StartListening(Func<T, IEnumerable<T>> messageProcessingCallback);
    }
}