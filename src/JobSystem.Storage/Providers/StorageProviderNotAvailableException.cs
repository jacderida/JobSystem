using System;

namespace JobSystem.Storage.Providers
{
	[Serializable]
	public class StorageProviderNotAvailableException : Exception
	{
		public StorageProviderNotAvailableException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}