using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Resources.Consignments;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class ConsignmentTests
	{
		private Consignment _savedConsignment;
		private ConsignmentService _consignmentService;
		private DomainValidationException _domainValidationException;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		private Consignment _consignmentForEdit;
		private Guid _consignmentForEditId;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_consignmentForEditId = Guid.NewGuid();
			_consignmentForEdit = new Consignment
			{
				Id = _consignmentForEditId,
				ConsignmentNo = "CR2000",
				DateCreated = DateTime.UtcNow,
				CreatedBy = new UserAccount(),
				Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Supplier Name" },
				IsOrdered = false
			};
		}

		#region Create

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

		#endregion
		#region Edit

		[Test]
		public void Edit_UserIsAdmin_SuccessfulEdit()
		{
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(_consignmentForEditId)).Return(_consignmentForEdit);
			consignmentRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "New Supplier" });
			_consignmentService = ConsignmentServiceFactory.Create(
				consignmentRepositoryMock,
				supplierRepositoryStub,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Admin | UserRole.Member));

			Edit(_consignmentForEditId, supplierId);
			consignmentRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_consignmentForEditId, _consignmentForEdit.Id);
			Assert.AreEqual(supplierId, _consignmentForEdit.Supplier.Id);
		}

		[Test]
		public void Edit_ValidDetails_SuccessfulEdit()
		{
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(_consignmentForEditId)).Return(_consignmentForEdit);
			consignmentRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "New Supplier" });
			_consignmentService = ConsignmentServiceFactory.Create(consignmentRepositoryMock, supplierRepositoryStub);

			Edit(_consignmentForEditId, supplierId);
			consignmentRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(_consignmentForEditId, _consignmentForEdit.Id);
			Assert.AreEqual(supplierId, _consignmentForEdit.Supplier.Id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidConsignmentId_ArgumentExceptionThrown()
		{
			var consignmentId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(consignmentId)).Return(null);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "New Supplier" });
			_consignmentService = ConsignmentServiceFactory.Create(consignmentRepositoryMock, supplierRepositoryStub);
			Edit(consignmentId, supplierId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidSupplierId_ArgumentExceptionThrown()
		{
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(_consignmentForEditId)).Return(_consignmentForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(null);
			_consignmentService = ConsignmentServiceFactory.Create(consignmentRepositoryMock, supplierRepositoryStub);
			Edit(_consignmentForEditId, supplierId);
		}

		[Test]
		public void Edit_ConsignmentIsOrdered_DomainValidationExceptionThrown()
		{
			_consignmentForEdit.IsOrdered = true;
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(_consignmentForEditId)).Return(_consignmentForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "New Supplier" });
			_consignmentService = ConsignmentServiceFactory.Create(consignmentRepositoryMock, supplierRepositoryStub);

			Edit(_consignmentForEditId, supplierId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ConsignmentIsOrdered));
		}

		[Test]
		public void Edit_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_consignmentForEdit.IsOrdered = true;
			var supplierId = Guid.NewGuid();
			var consignmentRepositoryMock = MockRepository.GenerateMock<IConsignmentRepository>();
			consignmentRepositoryMock.Stub(x => x.GetById(_consignmentForEditId)).Return(_consignmentForEdit);
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetById(supplierId)).Return(new Supplier { Id = supplierId, Name = "New Supplier" });
			_consignmentService = ConsignmentServiceFactory.Create(
				consignmentRepositoryMock,
				supplierRepositoryStub,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));

			Edit(_consignmentForEditId, supplierId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void Edit(Guid consignmentId, Guid supplierId)
		{
			try
			{
				_consignmentForEdit = _consignmentService.Edit(consignmentId, supplierId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetById

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

		#endregion
		#region GetConsignments

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

		#endregion
	}
}