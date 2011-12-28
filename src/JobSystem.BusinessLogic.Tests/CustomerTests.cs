using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.BusinessLogic.Tests.Context;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class CustomerTests
	{
		private const string GreaterThan256Characters = "jlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksdjlkasdj;lakjdflksd";
		private CustomerService _customerService;
		private DomainValidationException _domainValidationException;

		#region Setup and Utils

		[SetUp]
		public void Setup()
		{
			_customerService = CustomerServiceFactory.Create();
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
		public void Create_SuccessfullyCreateCustomer_CustomerCreated()
		{
			var customerRepositoryMock = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			customerRepositoryMock.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryMock);
			var customer = _customerService.Create(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			customerRepositoryMock.VerifyAllExpectations();
			Assert.That(customer.Id != Guid.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			CreateCustomer(
				Guid.Empty, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
		}

		[Test]
		public void Create_NameNotSupplied_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(new Customer { Name = "Gael Ltd" });
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				Guid.NewGuid(), String.Empty, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.NameRequired));
		}

		[Test]
		public void Create_NonUniqueName_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(new Customer { Name = "Gael Ltd" });
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.DuplicateName));
		}

		[Test]
		public void Create_NameGreaterThan256Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				Guid.NewGuid(), GreaterThan256Characters, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.NameTooLarge));
		}

		[Test]
		public void Create_Address1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line3 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line4 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_Address5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line5 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_TelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Telephone = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_FaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Fax = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_EmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Email = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_Contact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_Contact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_InvoiceTitleGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GreaterThan256Characters, GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.InvoiceTitleTooLarge));
		}

		[Test]
		public void Create_InvoiceAddress1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_InvoiceAddress2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_InvoiceAddress3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line3 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_InvoiceAddress4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line4 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_InvoiceAddress5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line5 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_InvoiceTelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Telephone = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_InvoiceFaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Fax = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_InvoiceEmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Email = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_InvoiceContact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Contact1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_InvoiceContact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Contact2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_DeliveryTitleGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Title", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				GreaterThan256Characters, GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.DeliveryTitleTooLarge));
		}

		[Test]
		public void Create_DeliveryAddress1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_DeliveryAddress2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_DeliveryAddress3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line3 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_DeliveryAddress4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line4 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_DeliveryAddress5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line5 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Create_DeliveryTelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Telephone = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_DeliveryFaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Fax = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_DeliveryEmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Email = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_DeliveryContact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Contact1 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Create_DeliveryContact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetByName("Gael Ltd")).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Contact2 = GreaterThan256Characters;
			CreateCustomer(
				Guid.NewGuid(), "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		private void CreateCustomer(
			Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo,
			string invoiceTitle, Address invoiceAddressDetails, ContactInfo invoiceContactInfo,
			string deliveryTitle, Address deliveryAddressDetails, ContactInfo deliveryContactInfo)
		{
			try
			{
				_customerService.Create(
					id, name, tradingAddressDetails, tradingContactInfo, invoiceTitle, invoiceAddressDetails, invoiceContactInfo, deliveryTitle, deliveryAddressDetails, deliveryContactInfo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Edit

		[Test]
		public void Edit_SuccessfullyEditCustomer_CustomerEdit()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryMock = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			customerRepositoryMock.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryMock);
			_customerService.Edit(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			customerRepositoryMock.VerifyAllExpectations();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_InvalidIdSupplied_CustomerEdit()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryMock = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryMock.Stub(x => x.GetById(Guid.NewGuid())).Return(null);
			_customerService = CustomerServiceFactory.Create(customerRepositoryMock);
			_customerService.Edit(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_IdNotSupplied_ArgumentExceptionThrown()
		{
			EditCustomer(
				Guid.Empty, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
		}

		[Test]
		public void Edit_NameNotSupplied_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				customer.Id, String.Empty, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.NameRequired));
		}

		[Test]
		public void Edit_NonUniqueName_DomainValidationExceptionThrown()
		{
			var nonUniqueCustomer = GetNonUniqueCustomerForEdit();
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			customerRepositoryStub.Stub(x => x.GetByName(nonUniqueCustomer.Name)).Return(nonUniqueCustomer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				customer.Id, nonUniqueCustomer.Name, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.DuplicateName));
		}

		[Test]
		public void Edit_NameGreaterThan256Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			CreateCustomer(
				customer.Id, GreaterThan256Characters, GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.NameTooLarge));
		}

		[Test]
		public void Edit_Address1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line3 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line4 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_Address5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingAddressDetails = GetAddressDetails("Trading");
			tradingAddressDetails.Line5 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", tradingAddressDetails, GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_TelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Telephone = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_FaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Fax = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_EmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Email = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_Contact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_Contact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var tradingContactInfo = GetContactInfo("Trading");
			tradingContactInfo.Contact2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), tradingContactInfo,
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_InvoiceTitleGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				GreaterThan256Characters, GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.InvoiceTitleTooLarge));
		}

		[Test]
		public void Edit_InvoiceAddress1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_InvoiceAddress2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_InvoiceAddress3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line3 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_InvoiceAddress4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line4 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_InvoiceAddress5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceAddressDetails = GetAddressDetails("Invoicing");
			invoiceAddressDetails.Line5 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", invoiceAddressDetails, GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_InvoiceTelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Telephone = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_InvoiceFaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Fax = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_InvoiceEmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Email = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_InvoiceContact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Contact1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_InvoiceContact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var invoiceContactInfo = GetContactInfo("Trading");
			invoiceContactInfo.Contact2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), invoiceContactInfo,
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_DeliveryTitleGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Title", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				GreaterThan256Characters, GetAddressDetails("Delivery"), GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.DeliveryTitleTooLarge));
		}

		[Test]
		public void Edit_DeliveryAddress1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_DeliveryAddress2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_DeliveryAddress3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line3 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_DeliveryAddress4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line4 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_DeliveryAddress5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryAddressDetails = GetAddressDetails("Invoicing");
			deliveryAddressDetails.Line5 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", deliveryAddressDetails, GetContactInfo("Delivery"));
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.AddressLineTooLarge));
		}

		[Test]
		public void Edit_DeliveryTelephoneGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Telephone = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_DeliveryFaxGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Fax = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_DeliveryEmailGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Email = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_DeliveryContact1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Contact1 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		[Test]
		public void Edit_DeliveryContact2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var customer = GetCustomerToEdit();
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			customerRepositoryStub.Stub(x => x.GetById(customer.Id)).Return(customer);
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub);
			var deliveryContactInfo = GetContactInfo("Delivery");
			deliveryContactInfo.Contact2 = GreaterThan256Characters;
			EditCustomer(
				customer.Id, "Gael Ltd", GetAddressDetails("Trading"), GetContactInfo("Trading"),
				"Gael Ltd Invoice Address", GetAddressDetails("Invoicing"), GetContactInfo("Invoicing"),
				"Gael Ltd Delivery Address", GetAddressDetails("Delivery"), deliveryContactInfo);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ContactInfoTooLarge));
		}

		private void EditCustomer(
			Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo,
			string invoiceTitle, Address invoiceAddressDetails, ContactInfo invoiceContactInfo,
			string deliveryTitle, Address deliveryAddressDetails, ContactInfo deliveryContactInfo)
		{
			try
			{
				_customerService.Edit(
					id, name, tradingAddressDetails, tradingContactInfo, invoiceTitle, invoiceAddressDetails, invoiceContactInfo, deliveryTitle, deliveryAddressDetails, deliveryContactInfo);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		private Customer GetCustomerToEdit()
		{
			return new Customer
			{
				Id = Guid.NewGuid(),
				Name = "EMIS UK Ltd",
				Address1 = "CEB",
				Address2 = "Greenwell Road",
				Address3 = "Aberdeen",
				Address4 = "AB12 3AX",
				Address5 = "UK",
				Telephone = "01224 894494",
				Fax = "01224 894929",
				Contact1 = "Joseph O'Neil",
				Contact2 = "Graham Robertson"
			};
		}

		private Customer GetNonUniqueCustomerForEdit()
		{
			return new Customer
			{
				Id = Guid.NewGuid(),
				Name = "Gael Ltd"
			};
		}

		#endregion
		#region Get

		[Test]
		public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
			try
			{
				_customerService.GetById(Guid.NewGuid());
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}			
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ViewCustomerInsufficientSecurityClearance));
		}

		[Test]
		public void GetCustomers_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var customerRepositoryStub = MockRepository.GenerateMock<ICustomerRepository>();
			_customerService = CustomerServiceFactory.Create(customerRepositoryStub, TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Public));
			try
			{
				_customerService.GetCustomers();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Customers.Messages.ViewCustomerListInsufficientSecurityClearance));
		}

		#endregion
	}
}