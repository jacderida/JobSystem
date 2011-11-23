using System;

namespace JobSystem.Framework.Configuration
{
	/// <summary>
	/// <see cref="NamedHostNameProvider"/> provides a hostname provider with an already known hostname.
	/// </summary>
	public class NamedHostNameProvider : IHostNameProvider
	{
		#region Fields

		private readonly string _hostname;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NamedHostNameProvider"/> class.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		public NamedHostNameProvider(string hostname)
		{
			Check.That<ArgumentNullException>(String.IsNullOrEmpty(hostname), "The hostname must have a value");
			_hostname = hostname;
		}

		#endregion

		#region Implementation of IHostNameProvider

		/// <summary>
		/// Gets the host name that has been used to access the application.
		/// </summary>
		/// <returns>The current hostname.</returns>
		public string GetHostName()
		{
			return _hostname;
		}

		#endregion
	}
}