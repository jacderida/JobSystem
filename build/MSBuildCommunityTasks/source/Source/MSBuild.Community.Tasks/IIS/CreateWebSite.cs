using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Build;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Web.Administration;

namespace MSBuild.Community.Tasks.IIS
{
	public class CreateWebSite : Task
	{
		[Required] public string ComputerName { get; set; }
		[Required] public string SiteName { get; set; }
		[Required] public string SitePhysicalPath { get; set; }
		[Required] public string SitePort { get; set; }

		public override bool Execute()
		{
			var serverManager = ServerManager.OpenRemote(ComputerName);
			serverManager.Sites.Add(SiteName, SitePhysicalPath, int.Parse(SitePort));
			serverManager.CommitChanges();
			return true;
		}
	}
}