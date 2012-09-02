using System;

namespace JobSystem.Storage.Providers.S3
{
	public static class S3Utilities
	{
		private const string BucketPrefix = "jobsystem-storage";

		public static string GetBucketName(string hostname)
		{
			return String.Format("{0}-{1}", BucketPrefix, hostname).Replace('.', '-');
		}
	}
}