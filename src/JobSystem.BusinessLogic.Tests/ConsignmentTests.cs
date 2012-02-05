using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class ConsignmentTests
	{
		private ConsignmentService _consignmentService;
		private DomainValidationException _domainValidationException;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
		}

		[Test]
		public void Create_ValidConsignmentDetails_SuccessfullyCreated()
		{
			var id = Guid.NewGuid();
			var supplierId = Guid.NewGuid();

			var supplierRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			supplierRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();

			supplierRepositoryMock.VerifyAllExpectations();
		}

		//private void CreateConsignment(
	}
}