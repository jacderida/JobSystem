using System.Messaging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.Community.Tasks.MSMQ
{
	public class CreateQueue : Task
	{
		/// <summary>
		/// Gets or sets the queue path.
		/// </summary>
		/// <value>The queue path.</value>
		[Required]
		public string QueuePath { get; set; }

		#region Overrides of Task

		/// <summary>
		/// When overridden in a derived class, executes the task.
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false.
		/// </returns>
		public override bool Execute()
		{
			var path = string.Format(@"FormatName:DIRECT=OS:{0}", QueuePath);
			MessageQueue.Create(path);
			return true;
		}

		#endregion
	}
}