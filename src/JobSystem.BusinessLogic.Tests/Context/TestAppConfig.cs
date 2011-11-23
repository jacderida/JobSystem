using JobSystem.Framework.Configuration;

namespace JobSystem.BusinessLogic.Tests.Context
{
	public class TestAppConfig
	{
		public IAppConfig AppConfig
		{
			get;
			set;
		}

		public TestAppConfig(IAppConfig appConfig)
		{
			AppConfig = appConfig;
		}

		public static TestAppConfig Create()
		{
			return new TestAppConfig(new LocalAppConfig());
		}
	}
}