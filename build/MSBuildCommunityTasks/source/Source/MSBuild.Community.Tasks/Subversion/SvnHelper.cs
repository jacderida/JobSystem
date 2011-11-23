using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using Microsoft.Build.Framework;

namespace MSBuild.Community.Tasks.Subversion
{
	public static class SvnHelper
	{
		public static string GetFullPathToSvn()
		{
			string svnPath = null;
			var exeName = "svn.exe";			
			// 1) check registry
			RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\{0}", exeName), false);
			if (key != null)
			{
				var possiblePath = key.GetValue(null) as string;
				if (File.Exists(possiblePath))
					svnPath = Path.GetDirectoryName(possiblePath);
			}
			// 2) search the path
			if (svnPath == null)
			{
				var pathEnvironmentVariable = Environment.GetEnvironmentVariable("PATH");
				var paths = pathEnvironmentVariable.Split(Path.PathSeparator);
				foreach (var path in paths)
				{
					var fullPathToClient = Path.Combine(path, exeName);
					if (File.Exists(fullPathToClient))
					{
						svnPath = path;
						break;
					}
				}
			}
			// 3) try default install location
			if (svnPath == null)
				svnPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Subversion\bin");
			return Path.Combine(svnPath, exeName);
		}
	}
}