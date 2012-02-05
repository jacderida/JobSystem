using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.Consignments;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class ConsignmentTests
	{
		private Consignment _savedConsignment;
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

			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_consignmentService = ConsignmentServiceFactory.Create(consignmentRepositoryMock, supplierId);
			CreateConsignment(id, supplierId);
			consignmentRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedConsignment.Id != Guid.Empty);
			Assert.That(!String.IsNullOrEmpty(_savedConsignment.ConsignmentNo) && _savedConsignment.ConsignmentNo.StartsWith("CR"));
			Assert.AreEqual(_savedConsignment.DateCreated, _dateCreated);
			Assert.AreEqual("test@usercontext.com", _savedConsignment.CreatedBy.EmailAddress);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var supplierId = Guid.NewGuid();

			_consignmentService = ConsignmentServiceFactory.Create(supplierId);
			CreateConsignment(id, supplierId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidSupplierId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var supplierId = Guid.Empty;

			_consignmentService = ConsignmentServiceFactory.Create(supplierId);
			CreateConsignment(id, supplierId);
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var supplierId = Guid.NewGuid();

			_consignmentService = ConsignmentServiceFactory.Create(supplierId,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
			CreateConsignment(id, supplierId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void CreateConsignment(Guid id, Guid supplierId)
		{
			try
			{
				_savedConsignment = _consignmentService.Create(id, supplierId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		[Test]
		public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			try
			{
				_consignmentService = ConsignmentServiceFactory.Create(
					TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
				_consignmentService.GetById(Guid.NewGuid());
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		[Test]
		public void GetConsignments_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			try
			{
				_consignmentService = ConsignmentServiceFactory.Create(
					TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
				_consignmentService.GetConsignments();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}
	}
}