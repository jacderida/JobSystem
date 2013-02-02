using System;

namespace JobSystem.Framework.Configuration
{
    public class NamedHostNameProvider : IHostNameProvider
    {
        private readonly string _hostname;

        public NamedHostNameProvider(string hostname)
        {
            Check.That<ArgumentNullException>(String.IsNullOrEmpty(hostname), "The hostname must have a value");
            _hostname = hostname;
        }

        public string GetHostName()
        {
            return _hostname;
        }
    }
}