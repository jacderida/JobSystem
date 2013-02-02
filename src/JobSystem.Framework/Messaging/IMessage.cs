using System;

namespace JobSystem.Framework.Messaging
{
    /// <summary>
    /// A message that accompanies a task.
    /// </summary>
    public interface IMessage
    {
        Guid Id { get; set; }
        int DeliveryAttempts { get; set; }
        string Domain { get; set; }
    }
}