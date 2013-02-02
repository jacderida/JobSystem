namespace JobSystem.Framework.Configuration
{
    public interface IConnectionStringProviderRepository
    {
        string Get(string hostname);
        void Put(string hostname, string connectionString);
    }
}