using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.Configuration
{
	public class ConnectionStringNotFoundException : Exception
	{
		/// <summary>
		/// <see cref="ConfigurationItemNotFoundException"/> is thrown when a configuration item was not found for a tennant.
		/// </summary>
		/// <param name="hostname">The hostname of the tennant.</param>
		/// <param name="configItem">The request configuration item.</param>
		public ConnectionStringNotFoundException(string hostname, string connectionStringName)
			: base(String.Format("Could not find the configuration item '{0}' for host '{1}.", connectionStringName, hostname))
		{
		}
	}
}