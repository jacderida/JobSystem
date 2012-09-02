using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JobSystem.BuildTasks
{
	public class UpdateAwsVersionNumber : Task
	{
		[Required]
		public string FilePath { get; set; }
		[Required]
		public string Version { get; set; }

		public override bool Execute()
		{
			var contents = File.ReadAllText(FilePath);
			contents = contents.Replace("Application.Version = 0.0.0.0", String.Format("Application.Version = {0}", Version));
			File.WriteAllText(FilePath, contents);
			return true;
		}
	}
}