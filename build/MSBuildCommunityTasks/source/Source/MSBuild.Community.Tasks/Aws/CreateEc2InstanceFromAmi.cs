using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using System.Threading;

namespace MSBuild.Community.Tasks.Aws
{
	public class CreateEc2InstanceFromAmi : Task
	{
		[Required] public string AwsAccessKey { get; set; }
		[Required] public string AwsSecretKey { get; set; }
		[Required] public string AmiId { get; set; }
		[Required] public string SecurityGroup { get; set; }
		[Required] public string KeyPair { get; set; }
		[Required] public string InstanceType { get; set; }
		[Output] public string InstancePublicDnsName { get; set; }
		[Output] public string InstanceId { get; set; }

		public override bool Execute()
		{
			using (var ec2Client = AWSClientFactory.CreateAmazonEC2Client(
				AwsAccessKey, AwsSecretKey, new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com")))
			{
				var runInstanceResponse = ec2Client.RunInstances(
					new RunInstancesRequest()
						.WithImageId(AmiId)
						.WithMinCount(1)
						.WithMaxCount(1)
						.WithInstanceType(InstanceType)
						.WithSecurityGroup(SecurityGroup)
						.WithKeyName(KeyPair));
				var instanceId = runInstanceResponse.RunInstancesResult.Reservation.RunningInstance[0].InstanceId;
				while (IsInstancePending(ec2Client, instanceId))
				{
					Log.LogMessage("Instance starting up...");
					Thread.Sleep(5000);
				}

				var instancePublicAddress = GetPublicDnsName(ec2Client, instanceId);
				Log.LogMessage("{0} instance started!", instancePublicAddress);
				InstancePublicDnsName = instancePublicAddress;
				InstanceId = instanceId;
				return true;
			}
		}

		private bool IsInstancePending(AmazonEC2 ec2Client, string instanceId)
		{
			return GetInstanceStatus(ec2Client, instanceId) == "pending";
		}

		private string GetInstanceStatus(AmazonEC2 ec2Client, string instanceId)
		{
			return GetRunningInstance(ec2Client, instanceId).InstanceState.Name;
		}

		private string GetPublicDnsName(AmazonEC2 ec2Client, string instanceId)
		{
			return GetRunningInstance(ec2Client, instanceId).PublicDnsName;
		}

		private RunningInstance GetRunningInstance(AmazonEC2 ec2Client, string instanceId)
		{
			var describeInstancesResponse = ec2Client.DescribeInstances(new DescribeInstancesRequest().WithInstanceId(instanceId));
			return describeInstancesResponse.DescribeInstancesResult.Reservation[0].RunningInstance.Find(
				r => r.InstanceId == instanceId);
		}
	}
}