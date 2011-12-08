using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Dto;
using NUnit.Framework;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class CompanyDetailsTests
	{
		private CompanyDetailsService _companyDetailsService;
		private DomainValidationException _domainValidationException;

		#region Setup and Utils

		[SetUp]
		public void Setup()
		{
			_companyDetailsService = CompanyDetailsServiceFactory.Create();
			_domainValidationException = null;
		}

		private Address GetAddressDetails()
		{
			return new Address
			{
				Line1 = String.Format("Line1"),
				Line2 = String.Format("Line2"),
				Line3 = String.Format("Line3"),
				Line4 = String.Format("Line4"),
				Line5 = String.Format("Line5")
			};
		}

		#endregion
		#region Create

		[Test]
		public void Create_SuccessfullyCreateCompanyDetails_CompanyDetailsCreated()
		{
			var companyDetailsRepositoryMock = MockRepository.GenerateMock<ICompanyDetailsRepository>();
			companyDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var companyDetails = _companyDetailsService.Create(
				Guid.NewGuid(), "EMIS (UK) Ltd", GetAddressDetails(),
				"01224 894494", "01224 894929", "info@emis-uk.com",
				"www.emis-uk.com", "REGNO123456", "VATNO123456",
				"terms and conditions", Guid.NewGuid(), Guid.NewGuid(),
				Guid.NewGuid(), Guid.NewGuid(), new byte[] { 1, 2, 3, 4, 5 });
			companyDetailsRepositoryMock.VerifyAllExpectations();
			Assert.That(companyDetails.Id != Guid.Empty);
		}

		#endregion
	}
}