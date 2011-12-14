using System;
using JobSystem.BusinessLogic.Validation;
using JobSystem.BusinessLogic.Validation.Extensions;
using JobSystem.DataModel;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Services
{
	public class CustomerService : ServiceBase
	{
		private ICustomerRepository _customerRepository;
		private CustomerValidator _customerValidator;

		public CustomerService(
			IUserContext applicationContext,
			ICustomerRepository customerRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_customerRepository = customerRepository;
			_customerValidator = new CustomerValidator(customerRepository);
		}

		#region Public Implementation

		public Customer Create(
			Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo,
			string invoiceTitle, Address invoiceAddressDetails, ContactInfo invoiceContactInfo,
			string deliveryTitle, Address deliveryAddressDetails, ContactInfo deliveryContactInfo)
		{
			if (id == null || id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the customer", "id");
			var customer = new Customer();
			customer.Id = id;
			customer.Name = name;
			customer.InvoiceTitle = !String.IsNullOrEmpty(invoiceTitle) ? invoiceTitle : String.Empty;
			customer.DeliveryTitle = !String.IsNullOrEmpty(deliveryTitle) ? deliveryTitle : String.Empty;
			PopulateTradingAddressInfo(customer, tradingAddressDetails);
			PopulateTradingContactInfo(customer, tradingContactInfo);
			PopulateInvoiceAddressInfo(customer, invoiceAddressDetails);
			PopulateSalesContactInfo(customer, invoiceContactInfo);
			PopulateDeliveryAddressInfo(customer, deliveryAddressDetails);
			PopulateDeliveryContactInfo(customer, deliveryContactInfo);
			ValidateAnnotatedObjectThrowOnFailure(customer);
			_customerValidator.ValidateThrowOnFailure(customer);
			_customerRepository.Create(customer);
			return customer;
		}

		public Customer Edit(
			Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo,
			string invoiceTitle, Address invoiceAddressDetails, ContactInfo invoiceContactInfo,
			string deliveryTitle, Address deliveryAddressDetails, ContactInfo deliveryContactInfo)
		{
			if (id == null || id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the customer", "id");
			var customer = _customerRepository.GetById(id);
			if (customer == null)
				throw new ArgumentException("An invalid ID was supplied for the customer", "id");
			customer.Name = name;
			customer.InvoiceTitle = !String.IsNullOrEmpty(invoiceTitle) ? invoiceTitle : String.Empty;
			customer.DeliveryTitle = !String.IsNullOrEmpty(deliveryTitle) ? deliveryTitle : String.Empty;
			PopulateTradingAddressInfo(customer, tradingAddressDetails);
			PopulateTradingContactInfo(customer, tradingContactInfo);
			PopulateInvoiceAddressInfo(customer, invoiceAddressDetails);
			PopulateSalesContactInfo(customer, invoiceContactInfo);
			PopulateDeliveryAddressInfo(customer, deliveryAddressDetails);
			PopulateDeliveryContactInfo(customer, deliveryContactInfo);
			ValidateAnnotatedObjectThrowOnFailure(customer);
			_customerValidator.ValidateThrowOnFailure(customer);
			_customerRepository.Update(customer);
			return customer;
		}

		public Customer GetById(Guid id)
		{
			var customer = _customerRepository.GetById(id);
			if (customer == null)
				throw new ArgumentException("No customer exists with the ID specified", "id");
			return customer;
		}

		public IEnumerable<Customer> GetCustomers()
		{
			return _customerRepository.GetCustomers();
		}

		#endregion
		#region Private Implementation

		private void PopulateTradingAddressInfo(Customer customer, Address tradingAddressDetails)
		{
			customer.Address1 = !String.IsNullOrEmpty(tradingAddressDetails.Line1) ? tradingAddressDetails.Line1 : String.Empty;
			customer.Address2 = !String.IsNullOrEmpty(tradingAddressDetails.Line2) ? tradingAddressDetails.Line2 : String.Empty;
			customer.Address3 = !String.IsNullOrEmpty(tradingAddressDetails.Line3) ? tradingAddressDetails.Line3 : String.Empty;
			customer.Address4 = !String.IsNullOrEmpty(tradingAddressDetails.Line4) ? tradingAddressDetails.Line4 : String.Empty;
			customer.Address5 = !String.IsNullOrEmpty(tradingAddressDetails.Line5) ? tradingAddressDetails.Line5 : String.Empty;
		}

		private void PopulateTradingContactInfo(Customer customer, ContactInfo tradingContactInfo)
		{
			customer.Telephone = !String.IsNullOrEmpty(tradingContactInfo.Telephone) ? tradingContactInfo.Telephone : String.Empty;
			customer.Fax = !String.IsNullOrEmpty(tradingContactInfo.Fax) ? tradingContactInfo.Fax : String.Empty;
			customer.Email = !String.IsNullOrEmpty(tradingContactInfo.Email) ? tradingContactInfo.Email : String.Empty;
			customer.Contact1 = !String.IsNullOrEmpty(tradingContactInfo.Contact1) ? tradingContactInfo.Contact1 : String.Empty;
			customer.Contact2 = !String.IsNullOrEmpty(tradingContactInfo.Contact2) ? tradingContactInfo.Contact2 : String.Empty;
		}

		private void PopulateInvoiceAddressInfo(Customer customer, Address invoiceAddressDetails)
		{
			customer.InvoiceAddress1 = !String.IsNullOrEmpty(invoiceAddressDetails.Line1) ? invoiceAddressDetails.Line1 : String.Empty;
			customer.InvoiceAddress2 = !String.IsNullOrEmpty(invoiceAddressDetails.Line2) ? invoiceAddressDetails.Line2 : String.Empty;
			customer.InvoiceAddress3 = !String.IsNullOrEmpty(invoiceAddressDetails.Line3) ? invoiceAddressDetails.Line3 : String.Empty;
			customer.InvoiceAddress4 = !String.IsNullOrEmpty(invoiceAddressDetails.Line4) ? invoiceAddressDetails.Line4 : String.Empty;
			customer.InvoiceAddress5 = !String.IsNullOrEmpty(invoiceAddressDetails.Line5) ? invoiceAddressDetails.Line5 : String.Empty;
		}

		private void PopulateSalesContactInfo(Customer customer, ContactInfo salesContactInfo)
		{
			customer.SalesTelephone = !String.IsNullOrEmpty(salesContactInfo.Telephone) ? salesContactInfo.Telephone : String.Empty;
			customer.SalesFax = !String.IsNullOrEmpty(salesContactInfo.Fax) ? salesContactInfo.Fax : String.Empty;
			customer.SalesEmail = !String.IsNullOrEmpty(salesContactInfo.Email) ? salesContactInfo.Email : String.Empty;
			customer.SalesContact1 = !String.IsNullOrEmpty(salesContactInfo.Contact1) ? salesContactInfo.Contact1 : String.Empty;
			customer.SalesContact2 = !String.IsNullOrEmpty(salesContactInfo.Contact2) ? salesContactInfo.Contact2 : String.Empty;
		}

		private void PopulateDeliveryAddressInfo(Customer customer, Address deliveryAddressDetails)
		{
			customer.DeliveryAddress1 = !String.IsNullOrEmpty(deliveryAddressDetails.Line1) ? deliveryAddressDetails.Line1 : String.Empty;
			customer.DeliveryAddress2 = !String.IsNullOrEmpty(deliveryAddressDetails.Line2) ? deliveryAddressDetails.Line2 : String.Empty;
			customer.DeliveryAddress3 = !String.IsNullOrEmpty(deliveryAddressDetails.Line3) ? deliveryAddressDetails.Line3 : String.Empty;
			customer.DeliveryAddress4 = !String.IsNullOrEmpty(deliveryAddressDetails.Line4) ? deliveryAddressDetails.Line4 : String.Empty;
			customer.DeliveryAddress5 = !String.IsNullOrEmpty(deliveryAddressDetails.Line5) ? deliveryAddressDetails.Line5 : String.Empty;
		}

		private void PopulateDeliveryContactInfo(Customer customer, ContactInfo deliveryContactInfo)
		{
			customer.DeliveryTelephone = !String.IsNullOrEmpty(deliveryContactInfo.Telephone) ? deliveryContactInfo.Telephone : String.Empty;
			customer.DeliveryFax = !String.IsNullOrEmpty(deliveryContactInfo.Fax) ? deliveryContactInfo.Fax : String.Empty;
			customer.DeliveryEmail = !String.IsNullOrEmpty(deliveryContactInfo.Email) ? deliveryContactInfo.Email : String.Empty;
			customer.DeliveryContact1 = !String.IsNullOrEmpty(deliveryContactInfo.Contact1) ? deliveryContactInfo.Contact1 : String.Empty;
			customer.DeliveryContact2 = !String.IsNullOrEmpty(deliveryContactInfo.Contact2) ? deliveryContactInfo.Contact2 : String.Empty;
		}

		#endregion
	}
}