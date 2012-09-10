using System;
using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	public class Config
	{
		public static Func<bool> UseRemoteConfig = () => bool.Parse(ConfigurationManager.AppSettings["UseRemoteConfiguration"]);
		public static Func<bool> UseHttps = () => bool.Parse(ConfigurationManager.AppSettings["UseHttps"]);
		public static string AwsKey = ConfigurationManager.AppSettings.Get("AwsKey");
		public static string AwsSecretKey = ConfigurationManager.AppSettings.Get("AwsSecretKey");
		public static string QueuePath = ConfigurationManager.AppSettings.Get("QueuePath");
	}
}