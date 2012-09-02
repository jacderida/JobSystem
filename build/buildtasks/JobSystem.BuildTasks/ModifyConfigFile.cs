using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JobSystem.BuildTasks
{
	public class ModifyConfigFile : Task
	{
		[Required]
		public string ConfigFilePath { get; set; }
		private XDocument ConfigDocument { get; set; }

		public string JobSystemConnectionString { get; set; }

		public override bool Execute()
		{
			ConfigDocument = XDocument.Load(new StringReader(File.ReadAllText(ConfigFilePath)));
			if (!String.IsNullOrEmpty(JobSystemConnectionString))
			{
				var elem = ConfigDocument.Descendants("connectionStrings").Elements().Where(e => e.Attribute("name").Value == "JobSystem").Single();
				elem.Attribute("connectionString").Value = JobSystemConnectionString;
			}
			ConfigDocument.Save(ConfigFilePath);
			return true;
		}
	}
}