namespace JobSystem.Framework.Configuration
{
    /// <summary>
    /// <see cref="IConfigDomainProvider"/> provides a means to resolve a configuration domain (or config group)
    /// from which a configuration value is to be read from, at runtime.
    /// </summary>
    public interface IConfigDomainProvider
    {
        #region Methods

        /// <summary>
        /// Gets the configuration domain
        /// </summary>
        /// <returns>current configuration domain</returns>
        string GetConfigDomain();

        #endregion Methods
    }
}