using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.Win32;

namespace MSBuild.Community.Tasks.Subversion
{
	public class SvnTag : SvnClient
	{
		[Required] public string WorkingDirectory { get; set; }
		[Required] public string TagLocation { get; set; }
		[Required] public string TagDirectory { get; set; }
		public string RevisionNumber { get; set; }

		public override bool Execute()
		{
			var fullTagLocation = String.Format("{0}/{1}", TagLocation, TagDirectory);
			var svnPath = SvnHelper.GetFullPathToSvn();
			CreateDirectoriesForTagging(svnPath, fullTagLocation);
			return CreateTag(svnPath, fullTagLocation) == 0;
		}

		private void CreateDirectoriesForTagging(string svnPath, string fullTagLocation)
		{
			var arguments = String.Format("mkdir --parents {0} -m \"Creating {1} for tagging\"", fullTagLocation, TagDirectory);
			var process = GetSvnProcess(svnPath, arguments);
			Log.LogMessage("Launching {0} {1}", svnPath, arguments);
			process.Start();
			var output = process.StandardOutput.ReadToEnd();
			var errors = process.StandardError.ReadToEnd();
			Log.LogMessage("Svn output: {0}", output);
			Log.LogMessage("Svn errors: {0}", errors);
			process.WaitForExit();
		}

		private int CreateTag(string svnPath, string fullTagLocation)
		{
			var arguments = String.Format(
				"cp \"{0}\" \"{1}\" -r {2} -m \"Creating a tag at {3}\"",
				WorkingDirectory,
				fullTagLocation,
				!String.IsNullOrEmpty(RevisionNumber) ? RevisionNumber : "HEAD",
				fullTagLocation);
			var process = GetSvnProcess(svnPath, arguments);
			Log.LogMessage("Launching {0} {1}", svnPath, arguments);
			process.Start();
			var output = process.StandardOutput.ReadToEnd();
			var errors = process.StandardError.ReadToEnd();
			Log.LogMessage("Svn output: {0}", output);
			Log.LogMessage("Svn errors: {0}", errors);
			process.WaitForExit();
			return process.ExitCode;
		}

		private Process GetSvnProcess(string svnPath, string arguments)
		{
			var process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = svnPath;
			process.StartInfo.Arguments = arguments;
			return process;
		}
	}
}