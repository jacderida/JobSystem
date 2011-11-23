using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Amazon;
using Amazon.AutoScaling;
using Amazon.AutoScaling.Model;

namespace MSBuild.Community.Tasks.Aws
{
	public class CreateAutoScalingLaunchConfiguration : Task
	{
		[Required] public string AwsAccessKey { get; set; }
		[Required] public string AwsSecretKey { get; set; }
		[Required] public string AmiId { get; set; }
		[Required] public string LaunchConfigurationName { get; set; }
		[Required] public string InstanceType { get; set; }
		[Required] public string KeyName { get; set; }
		[Required] public string SecurityGroup { get; set; }

		public override bool Execute()
		{
			using (var autoScalingClient = AWSClientFactory.CreateAmazonAutoScalingClient(
				AwsAccessKey, AwsSecretKey, new AmazonAutoScalingConfig().WithServiceURL("https://eu-west-1.autoscaling.amazonaws.com")))
			{
				var response = autoScalingClient.CreateLaunchConfiguration(
					new CreateLaunchConfigurationRequest()
						.WithImageId(AmiId)
						.WithKeyName(KeyName)
						.WithSecurityGroups(SecurityGroup)
						.WithInstanceType(InstanceType)
						.WithLaunchConfigurationName(LaunchConfigurationName));
			}
			return true;
		}
	}
}