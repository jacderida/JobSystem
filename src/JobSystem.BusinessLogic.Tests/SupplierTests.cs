using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Suppliers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class SupplierTests
	{
		private SupplierService _supplierService;
		private Supplier _savedSupplier;
		private DomainValidationException _domainValidationException;

		#region Setup and Utils

		[SetUp]
		public void Setup()
		{
			_supplierService = SupplierServiceFactory.Create();
			_domainValidationException = null;
		}

		private Address GetAddressDetails(string type)
		{
			return new Address
			{
				Line1 = String.Format("{0}Line1", type),
				Line2 = String.Format("{0}Line2", type),
				Line3 = String.Format("{0}Line3", type),
				Line4 = String.Format("{0}Line4", type),
				Line5 = String.Format("{0}Line5", type),
			};
		}

		private ContactInfo GetContactInfo(string type)
		{
			return new ContactInfo
			{
				Telephone = String.Format("{0}Telephone", type),
				Fax = String.Format("{0}Fax", type),
				Email = String.Format("{0}Email", type),
				Contact1 = String.Format("{0}Contact1", type),
				Contact2 = String.Format("{0}Contact2", type)
			};
		}

		#endregion
		#region Create

		[Test]
		public void Create_SuccessfullyCreateSupplier_SupplierCreated()
		{
			var supplierRepositoryMock = MockRepository.GenerateMock<ISupplierRepository>();
			supplierRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			supplierRepositoryMock.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryMock);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			supplierRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedSupplier.Id != Guid.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			CreateSupplier(
				Guid.Empty, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
		}

		[Test]
		public void Create_NameNotSupplied_DomainValidationExceptionThrown()
		{
			CreateSupplier(
				Guid.NewGuid(), String.Empty, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameRequired));
		}

		[Test]
		public void Create_NonUniqueName_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(new Supplier { Name = "Gael Ltd" });
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DuplicateName));
		}

		[Test]
		public void Create_NameGreaterThan256Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			CreateSupplier(
				Guid.NewGuid(), new string('a', 256), GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameTooLarge));
		}

		[Test]
		public void Create_Address1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line1 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
		 	var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line2 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line3 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line4 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line5 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_TelephoneGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Telephone = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_FaxGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Fax= new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_EmailGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Email = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_Contact1GreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact1 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_Contact2GreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact2 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				GetAddressDetails("Sales"), GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_SalesAddress1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesAddressDetails = GetAddressDetails("Sales");
			salesAddressDetails.Line1 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("trading"), GetContactInfo("Trading"),
				salesAddressDetails, GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_SalesAddress2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesAddressDetails = GetAddressDetails("Sales");
			salesAddressDetails.Line2 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("trading"), GetContactInfo("Trading"),
				salesAddressDetails, GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_SalesAddress3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesAddressDetails = GetAddressDetails("Sales");
			salesAddressDetails.Line3 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("trading"), GetContactInfo("Trading"),
				salesAddressDetails, GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_SalesAddress4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesAddressDetails = GetAddressDetails("Sales");
			salesAddressDetails.Line4 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("trading"), GetContactInfo("Trading"),
				salesAddressDetails, GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_SalesAddress5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesAddressDetails = GetAddressDetails("Sales");
			salesAddressDetails.Line5 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("trading"), GetContactInfo("Trading"),
				salesAddressDetails, GetContactInfo("Sales"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_SalesTelephoneGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesContactInfo = GetContactInfo("Sales");
			salesContactInfo.Telephone = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), salesContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_SalesFaxGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesContactInfo = GetContactInfo("Sales");
			salesContactInfo.Fax = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), salesContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_SalesEmailGreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesContactInfo = GetContactInfo("Sales");
			salesContactInfo.Email = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), salesContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_SalesContact1GreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesContactInfo = GetContactInfo("Sales");
			salesContactInfo.Contact1 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), salesContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_SalesContact2GreaterThan50Characters_DomainValidationException()
		{
			var supplierRepositoryStub = MockRepository.GenerateStub<ISupplierRepository>();
			supplierRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_supplierService = SupplierServiceFactory.Create(supplierRepositoryStub);
			var salesContactInfo = GetContactInfo("Sales");
			salesContactInfo.Contact2 = new String('a', 256);
			CreateSupplier(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GetAddressDetails("Sales"), salesContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.ContactInfoTooLarge));
		}

		private void CreateSupplier(
			Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo, Address salesAddressDetails, ContactInfo salesContactInfo)
		{
			try
			{
				_savedSupplier = _supplierService.Create(id, name, tradingAddressDetails, tradingContactInfo, salesAddressDetails, salesContactInfo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}