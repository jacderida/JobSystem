
namespace JobSystem.Framework.Messaging
{
    public class NullQueueDispatcher : IQueueDispatcher<IMessage>
    {
        public void Enqueue(IMessage message)
        {
        }
    }
}