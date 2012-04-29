using JobSystem.DataAccess.NHibernate;
using NUnit.Framework;
using JobSystem.BusinessLogic.Services;

namespace JobSystem.BusinessLogic.IntegrationTests
{
	[TestFixture]
	public class CertificateSearchTests
	{
		private CertificateService _certificateService;

		[SetUp]
		public void Setup()
		{
			NHibernateSession.Current.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			NHibernateSession.Current.Transaction.Rollback();
		}

		public void SearchByKeyword_CertNoExactMatch_1Result()
		{

		}
	}
}