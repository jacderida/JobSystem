using System;

namespace JobSystem.Framework
{
	/// <summary>
	/// Utility class hat provides methods for  
	/// </summary>
	public static class Check
	{
		/// <summary>
		/// Check that an argument is not null
		/// </summary>
		/// <param name="obj">the object to check </param>
		/// <param name="message">the message to pass into the exception</param>
		public static void NotNull(object obj, string message)
		{
			That<ArgumentNullException>(obj == null, message);
		}

		/// <summary>
		/// Check an assertion and throws exception if it is meet.
		/// </summary>
		/// <typeparam name="TException">The exception to throw</typeparam>
		/// <param name="assertion">The assertion that we are checking</param>
		/// <param name="message">the message to pass to the exception</param>
		public static void That<TException>(bool assertion, string message) where TException : Exception
		{
			if (assertion)
			{
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}

		/// <summary>
		/// Check an assertion and throws exception if it is meet.
		/// </summary>
		/// <typeparam name="TException">The exception to throw</typeparam>
		/// <param name="assertion">The assertion that we are checking</param>
		/// <param name="message">the message to pass to the exception</param>
		public static void That<TException>(Func<bool> assertion, string message) where TException : Exception
		{
			if (assertion())
			{
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}
	}
}