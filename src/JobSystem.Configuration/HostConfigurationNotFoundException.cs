using System;

namespace JobSystem.Configuration
{
	[Serializable]
	public class HostConfigurationNotFoundException : Exception
	{
		/// <summary>
		/// <see cref="HostConfigurationNotFoundException"/> is thrown when no configuration exists for a host. This
		/// </summary>
		public HostConfigurationNotFoundException(string hostname) : base(String.Format("No configuration found for host '{0}'", hostname))
		{
			
		}
	}
}