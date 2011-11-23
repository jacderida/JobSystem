using System;

namespace JobSystem.Configuration
{
	[Serializable]
	public class ConfigurationItemNotFoundException : Exception
	{
		#region Constructors

		/// <summary>
		/// <see cref="ConfigurationItemNotFoundException"/> is thrown when a configuration item was not found for a tennant.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <param name="configItem">The request configuration item.</param>
		public ConfigurationItemNotFoundException(string hostname, string configItem) : base (String.Format("Could not find the configuration item '{0}' for host '{1}.", configItem, hostname))
		{
		}

		#endregion
	}
}