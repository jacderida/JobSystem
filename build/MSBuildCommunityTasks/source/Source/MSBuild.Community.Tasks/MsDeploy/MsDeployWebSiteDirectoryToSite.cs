using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Diagnostics;

namespace MSBuild.Community.Tasks.MsDeploy
{
	public class MsDeployWebSiteDirectoryToSite : Task
	{
		[Required] public string SourceContentPath { get; set; }
		[Required] public string DestinationSiteName { get; set; }
		[Required] public string MsDeployServerLocation { get; set; }
		[Required] public string MsDeployServerUserName { get; set; }
		[Required] public string MsDeployServerPassword { get; set; }

		private const string MsDeployCopyContentCommand =
			@"-allowUntrusted -verb:sync -source:contentPath=""{0}"" -dest:contentPath=""{1}"",wmsvc={2},userName={3},password={4}";
		private const string MsDeployCreateSiteCommand =
			@"-allowUntrusted -verb:sync -source:createApp -dest:createApp=""{0}"",wmsvc={1},userName={2},password={3}";

		public override bool Execute()
		{
			try
			{
				CopyDirectoryContentToMsDeployServer();
				CreateSite();
				return true;
			}
			catch (ApplicationException)
			{
				return false;
			}
		}

		private void CopyDirectoryContentToMsDeployServer()
		{
			var error = String.Empty;
			var process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = "msdeploy";
			process.StartInfo.Arguments = String.Format(
				MsDeployCopyContentCommand, SourceContentPath, DestinationSiteName, MsDeployServerLocation, MsDeployServerUserName, MsDeployServerPassword);
			process.Start();
			error = process.StandardError.ReadToEnd();
			process.WaitForExit();
			Log.LogMessage(error);
			if (ProcessExitedWithError(error))
				throw new ApplicationException("There was an error when running msdeploy");
		}

		private void CreateSite()
		{
			var process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = "msdeploy";
			process.StartInfo.Arguments = String.Format(
				MsDeployCreateSiteCommand, DestinationSiteName, MsDeployServerLocation, MsDeployServerUserName, MsDeployServerPassword);
			process.Start();
			var error = process.StandardError.ReadToEnd();
			process.WaitForExit();
			Log.LogMessage(error);
			if (ProcessExitedWithError(error))
				throw new ApplicationException("There was an error when running msdeploy");
		}

		private bool ProcessExitedWithError(string processError)
		{
			return processError.Contains("Error count");
		}
	}
}