using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;

namespace JobSystem.Storage.Providers.S3
{
	public class S3ClientHelper
	{
		private readonly string _amazonKey;
		private readonly string _amazonPrivateKey;

		public S3ClientHelper(string amazonKey, string amazonPrivateKey)
		{
			_amazonKey = amazonKey;
			_amazonPrivateKey = amazonPrivateKey;
		}

		internal TResult TryCommand<TResult>(Func<AmazonS3Client, TResult> command)
		{
			try
			{
				using (var client = CreateAndInitializeS3ClientInstance())
				{
					var result = command.Invoke(client);
					return result;
				}
			}
			catch (AmazonS3Exception ex)
			{
				throw new StorageProviderNotAvailableException("The S3 storage is not available at the moment. Please try again later.", ex);
			}
		}

		private AmazonS3Client CreateAndInitializeS3ClientInstance()
		{
			var awsS3Config = new AmazonS3Config().WithUseSecureStringForAwsSecretKey(true);
			return new AmazonS3Client(_amazonKey, _amazonPrivateKey, awsS3Config);
		}
	}
}