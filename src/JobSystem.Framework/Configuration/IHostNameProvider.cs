namespace JobSystem.Framework.Configuration
{
    public interface IHostNameProvider
    {
        /// <summary>
        /// Gets the host name that has been used to access the application.
        /// </summary>
        /// <returns>The current hostname.</returns>
        string GetHostName();
    }
}