using System;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using JobSystem.Framework.Configuration;

namespace JobSystem.Storage.Providers.S3
{
	public class S3StorageProvider
	{
		internal const string FilenameHeader = "x-amz-meta-filename";
		internal readonly S3ClientHelper ClientHelper;
		internal readonly string BucketName;

		public S3StorageProvider(IHostNameProvider hostNameProvider)
		{
			BucketName = S3Utilities.GetBucketName(hostNameProvider.GetHostName());
			ClientHelper = new S3ClientHelper(Config.AwsKey, Config.AwsSecretKey);
		}

		protected GetObjectResponse Get(string key)
		{
			Func<AmazonS3Client, GetObjectResponse> command = c =>
			{
				var request = new GetObjectRequest().WithBucketName(BucketName).WithKey(key);
				return c.GetObject(request);
			};
			var response = ClientHelper.TryCommand(command);
			return response;
		}

		protected int GetListCount(string path)
		{
			Func<AmazonS3Client, ListObjectsResponse> command = c =>
			{
				var request = new ListObjectsRequest().WithBucketName(BucketName).WithPrefix(path);
				return c.ListObjects(request);
			};
			return ClientHelper.TryCommand(command).S3Objects.Count;
		}

		protected string GetUrl(string objectId)
		{
			Func<AmazonS3Client, string> command = c =>
			{
				GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
					.WithBucketName(BucketName)
					.WithKey(objectId)
					.WithProtocol(Protocol.HTTP)
					.WithExpires(DateTime.UtcNow.AddDays(1));
				return c.GetPreSignedURL(request);
			};
			return ClientHelper.TryCommand(command);
		}

		protected void Put(string key, string filename, string contentType, Stream content)
		{
			CreateBucket(BucketName);
			Func<AmazonS3Client, PutObjectResponse> command = c =>
			{
				var request = new PutObjectRequest().WithKey(key).WithContentType(contentType).WithBucketName(BucketName).WithMetaData("filename", filename);
				request.InputStream = content;
				return c.PutObject(request);
			};
			var response = ClientHelper.TryCommand(command);
			response.Dispose();
		}

		protected void Delete(string objectId)
		{
			Func<AmazonS3Client, DeleteObjectResponse> command = c =>
			{
				var request = new DeleteObjectRequest().WithKey(objectId).WithBucketName(BucketName);
				return c.DeleteObject(request);
			};
			var response = ClientHelper.TryCommand(command);
			response.Dispose();
		}

		protected AttachmentData ToAttachmentData(GetObjectResponse response, Guid objectId)
		{
			// todo: using a memory stream will mean reading the content into memory, perhaps a temp file would be better
			var memStream = new MemoryStream();
			response.ResponseStream.CopyTo(memStream);
			memStream.Position = 0;
			var attachment = new AttachmentData
			{
				Id = objectId,
				Content = memStream,
				ContentType = response.ContentType,
				Filename = response.Headers[FilenameHeader]
			};
			return attachment;
		}

		protected void CreateBucket(string name)
		{
			if (!BucketExists(name))
			{
				Func<AmazonS3Client, PutBucketResponse> command = c =>
				{
					var request = new PutBucketRequest().WithBucketRegion(S3Region.EU).WithBucketName(name);
					return c.PutBucket(request);
				};
				var response = ClientHelper.TryCommand(command);
				response.Dispose();
			}
		}

		protected bool BucketExists(string name)
		{
			Func<AmazonS3Client, ListBucketsResponse> command = c =>
			{
				var request = new ListBucketsRequest();
				return c.ListBuckets(request);
			};

			var response = ClientHelper.TryCommand(command);
			var result = response.Buckets.Any(b => b.BucketName == name);
			response.Dispose();
			return result;
		}
	}
}