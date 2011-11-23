namespace JobSystem.Framework.Configuration
{
	/// <summary>
	/// <see cref="IHostNameProvider"/> provides an interface that is used to resolve the hostname that has been used to 
	/// </summary>
	public interface IHostNameProvider
	{
		/// <summary>
		/// Gets the host name that has been used to access the application.
		/// </summary>
		/// <returns>The current hostname.</returns>
		string GetHostName();
	}
}