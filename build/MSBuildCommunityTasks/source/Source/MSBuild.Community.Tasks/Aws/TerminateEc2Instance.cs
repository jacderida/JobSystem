using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace MSBuild.Community.Tasks.Aws
{
	public class TerminateEc2Instance : Task
	{
		[Required] public string AwsAccessKey { get; set; }
		[Required] public string AwsSecretKey { get; set; }
		[Required] public string InstanceId { get; set; }
		public bool WaitForMachineToTerminate { get; set; }

		public override bool Execute()
		{
			using (var ec2Client = AWSClientFactory.CreateAmazonEC2Client(
				AwsAccessKey, AwsSecretKey, new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com")))
			{
				ec2Client.TerminateInstances(new TerminateInstancesRequest().WithInstanceId(InstanceId));
				if (WaitForMachineToTerminate)
					while (IsInstanceShuttingDown(ec2Client, InstanceId))
						;
				return true;
			}
		}

		private bool IsInstanceShuttingDown(AmazonEC2 ec2Client, string instanceId)
		{
			return GetInstanceStatus(ec2Client, instanceId) == "shutting-down";
		}

		private string GetInstanceStatus(AmazonEC2 ec2Client, string instanceId)
		{
			return GetRunningInstance(ec2Client, instanceId).InstanceState.Name;
		}

		private RunningInstance GetRunningInstance(AmazonEC2 ec2Client, string instanceId)
		{
			var describeInstancesResponse = ec2Client.DescribeInstances(new DescribeInstancesRequest().WithInstanceId(instanceId));
			return describeInstancesResponse.DescribeInstancesResult.Reservation[0].RunningInstance.Find(
				r => r.InstanceId == instanceId);
		}
	}
}