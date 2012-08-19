using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using System.Threading;
using JobSystem.Framework.Messaging;

namespace JobSystem.Queueing.Msmq
{
	/// <summary>
	/// Encapsulates access to the MSMQ Server
	/// </summary>
	public class MsmqQueueGateway<T> : IQueueListener<T>, IQueueDispatcher<T> where T : IMessage
	{
		private readonly string _queuePath;

		private MessageQueue _queue;
		private readonly object _lockObject = new object();
		private Func<T, IEnumerable<T>> _callback;

		public MsmqQueueGateway(string queuePath)
		{
			_queuePath = queuePath;
		}

		/// <summary>
		/// Starts the System.Messaging/MSMQ MessageQueue listening.
		/// </summary>
		/// <param name="messageProcessingCallback">The message processing callback to be called when a new message is recieved.</param>
		public void StartListening(Func<T, IEnumerable<T>> messageProcessingCallback)
		{
			_callback = messageProcessingCallback;
			using (_queue = new MessageQueue(_queuePath))
			{
				_queue.Formatter = new BinaryMessageFormatter();
				_queue.MessageReadPropertyFilter.SetAll();
				_queue.ReceiveCompleted += QueueReceiveCompleted;
				_queue.BeginReceive();
				System.Diagnostics.Debug.WriteLine("Started Listening");
			}
		}

		/// <summary>
		/// <para>
		/// Enqueues the specified event.
		/// </para>
		/// <para>
		/// This method queues <paramref name="message"/> for processing. If this is the first attempt to deliver <paramref name="message"/>,
		/// i.e. <paramref name="ITaskMessage.DeliveryAttempts"/> is 0, then <see cref="ITaskMessage.Domain"/> will be set upon
		/// <paramref name="message"/> based upon the current context of the request. In this case, <see cref="ITaskMessage.Domain"/> must
		/// be null. If this is not the first attempt to delivery <paramref name="message"/>, <see cref="ITaskMessage.Domain"/> is unchanged.
		/// </para>
		/// </summary>
		/// <param name="message">the message to be queued</param>
		/// <exception cref="ArgumentException">Thrown if this is the first attempt to deliver <paramref name="message"/>, but
		/// <see cref="ITaskMessage.Domain"/> is non null.</exception>
		public void Enqueue(T @event)
		{
			@event.DeliveryAttempts++;
			var msmqMessage = new Message(@event, new BinaryMessageFormatter())
			{
				Recoverable = true,
				Label = @event.GetType().Name,
				AttachSenderId = false
			};
			if (@event.DeliveryAttempts < int.Parse(ConfigurationManager.AppSettings["MaxDeliveryAttempts"]))
			{
				using (var queue = new MessageQueue(_queuePath))
				{
					queue.Formatter = new BinaryMessageFormatter();
					queue.Send(msmqMessage);
				}
			}
		}

		private void QueueReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
		{
			lock (_lockObject)
			{
				// If this event handler was called by a known bug in System.Messaging, just continue on listening.
				if (SubjectToBug(e))
				{
					_queue.BeginReceive();
					return;
				}
				var msg = (T)e.Message.Body;
				IEnumerable<T> result = null;
				try
				{
					result = _callback.Invoke(msg);
				}
				catch (Exception)
				{
					Enqueue(msg);
					// Simple exponential backoff, pause for up to a max of 10 seconds.
					var sleepTime = (msg.DeliveryAttempts > 10 ? 10 : msg.DeliveryAttempts) * 1000;
					Thread.Sleep(sleepTime);
				}
				if (result != null)
				{
					foreach (var message in result)
					{
						try
						{
							Enqueue(message);
						}
						catch (Exception)
						{
							Enqueue(message);
							// Simple exponential backoff, pause for up to a max of 10 seconds.
							var sleepTime = (message.DeliveryAttempts > 10 ? 10 : message.DeliveryAttempts) * 1000;
							Thread.Sleep(sleepTime);
						}
					}
				}
			}
			_queue.BeginReceive();
		}

		/// <summary>
		/// Checks to see if the code is subject to a known bug in System.Messaging.
		/// </summary>
		/// <param name="e">The <see cref="System.Messaging.ReceiveCompletedEventArgs"/> instance containing the event data.</param>
		/// <returns>True if the code is subject to a known bug.</returns>
		private static bool SubjectToBug(ReceiveCompletedEventArgs e)
		{
			try
			{
				// Disables "variable message is never used" warning.
#pragma warning disable 168
				var message = e.Message;
#pragma warning restore 168
			}
			catch (MessageQueueException ex)
			{
				// BUG: in .NET framework ! --  http://connect.microsoft.com/VisualStudio/feedback/details/206533/missing-enum-value-for-msmq
				// There is a missing value in the enum for -1073741536
				if ((int)ex.MessageQueueErrorCode == -1073741536)
					return true;
			}
			return false;
		}
	}
}