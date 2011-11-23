using System;
using System.Linq;
using Microsoft.Build;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace MSBuild.Community.Tasks.Aws
{
	public class CreateAmiImageFromInstance : Task
	{
		[Required] public string AwsAccessKey { get; set; }
		[Required] public string AwsSecretKey { get; set; }
		[Required] public string InstanceId { get; set; }
		[Required] public string InstanceName { get; set; }
		[Output] public string AmiId { get; set; }

		public override bool Execute()
		{
			using (var ec2Client = AWSClientFactory.CreateAmazonEC2Client(
				AwsAccessKey, AwsSecretKey, new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com")))
			{
				var createImageResponse = ec2Client.CreateImage(new CreateImageRequest()
					.WithInstanceId(InstanceId)
					.WithName(InstanceName)
					.WithNoReboot(false));
				AmiId = createImageResponse.CreateImageResult.ImageId;
			}
			return true;
		}
	}
}