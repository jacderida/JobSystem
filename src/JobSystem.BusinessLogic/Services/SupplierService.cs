using System;
using JobSystem.BusinessLogic.Validation;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.BusinessLogic.Validation.Extensions;
using JobSystem.DataModel;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Suppliers;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Services
{
    public class SupplierService : ServiceBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly SupplierValidator _supplierValidator;

        public SupplierService(
            IUserContext applicationContext,
            ISupplierRepository supplierRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _supplierRepository = supplierRepository;
            _supplierValidator = new SupplierValidator(supplierRepository);
        }

        public Supplier Create(
            Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo, Address salesAddressDetails, ContactInfo salesContactInfo)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the supplier.");
            var supplier = new Supplier();
            supplier.Id = id;
            supplier.Name = name;
            PopulateTradingAddressInfo(supplier, tradingAddressDetails);
            PopulateTradingContactInfo(supplier, tradingContactInfo);
            PopulateSalesAddressInfo(supplier, salesAddressDetails);
            PopulateSalesContactInfo(supplier, salesContactInfo);
            ValidateAnnotatedObjectThrowOnFailure(supplier);
            _supplierValidator.ValidateThrowOnFailure(supplier);
            _supplierRepository.Create(supplier);
            return supplier;
        }

        public Supplier Edit(
            Guid id, string name, Address tradingAddressDetails, ContactInfo tradingContactInfo, Address salesAddressDetails, ContactInfo salesContactInfo)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the supplier.");
            var supplier = _supplierRepository.GetById(id);
            if (supplier == null)
                throw new ArgumentException("An invalid ID was supplied for the supplier.");
            supplier.Name = name;
            PopulateTradingAddressInfo(supplier, tradingAddressDetails);
            PopulateTradingContactInfo(supplier, tradingContactInfo);
            PopulateSalesAddressInfo(supplier, salesAddressDetails);
            PopulateSalesContactInfo(supplier, salesContactInfo);
            ValidateAnnotatedObjectThrowOnFailure(supplier);
            _supplierValidator.ValidateThrowOnFailure(supplier);
            _supplierRepository.Update(supplier);
            return supplier;
        }

        public Supplier GetById(Guid id)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _supplierRepository.GetById(id);
        }

        public int GetSuppliersCount()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _supplierRepository.GetSuppliersCount();
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _supplierRepository.GetSuppliers();
        }

        public IEnumerable<Supplier> SearchByKeyword(string keyword)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _supplierRepository.SearchByKeyword(keyword);
        }

        private void PopulateTradingAddressInfo(Supplier supplier, Address tradingAddressDetails)
        {
            supplier.Address1 = !String.IsNullOrEmpty(tradingAddressDetails.Line1) ? tradingAddressDetails.Line1 : String.Empty;
            supplier.Address2 = !String.IsNullOrEmpty(tradingAddressDetails.Line2) ? tradingAddressDetails.Line2 : String.Empty;
            supplier.Address3 = !String.IsNullOrEmpty(tradingAddressDetails.Line3) ? tradingAddressDetails.Line3 : String.Empty;
            supplier.Address4 = !String.IsNullOrEmpty(tradingAddressDetails.Line4) ? tradingAddressDetails.Line4 : String.Empty;
            supplier.Address5 = !String.IsNullOrEmpty(tradingAddressDetails.Line5) ? tradingAddressDetails.Line5 : String.Empty;
        }

        private void PopulateTradingContactInfo(Supplier supplier, ContactInfo tradingContactInfo)
        {
            supplier.Telephone = !String.IsNullOrEmpty(tradingContactInfo.Telephone) ? tradingContactInfo.Telephone : String.Empty;
            supplier.Fax = !String.IsNullOrEmpty(tradingContactInfo.Fax) ? tradingContactInfo.Fax : String.Empty;
            supplier.Email = !String.IsNullOrEmpty(tradingContactInfo.Email) ? tradingContactInfo.Email : String.Empty;
            supplier.Contact1 = !String.IsNullOrEmpty(tradingContactInfo.Contact1) ? tradingContactInfo.Contact1 : String.Empty;
            supplier.Contact2 = !String.IsNullOrEmpty(tradingContactInfo.Contact2) ? tradingContactInfo.Contact2 : String.Empty;
        }

        private void PopulateSalesAddressInfo(Supplier supplier, Address salesAddressDetails)
        {
            supplier.SalesAddress1 = !String.IsNullOrEmpty(salesAddressDetails.Line1) ? salesAddressDetails.Line1 : String.Empty;
            supplier.SalesAddress2 = !String.IsNullOrEmpty(salesAddressDetails.Line2) ? salesAddressDetails.Line2 : String.Empty;
            supplier.SalesAddress3 = !String.IsNullOrEmpty(salesAddressDetails.Line3) ? salesAddressDetails.Line3 : String.Empty;
            supplier.SalesAddress4 = !String.IsNullOrEmpty(salesAddressDetails.Line4) ? salesAddressDetails.Line4 : String.Empty;
            supplier.SalesAddress5 = !String.IsNullOrEmpty(salesAddressDetails.Line5) ? salesAddressDetails.Line5 : String.Empty;
        }

        private void PopulateSalesContactInfo(Supplier supplier, ContactInfo salesContactInfo)
        {
            supplier.SalesTelephone = !String.IsNullOrEmpty(salesContactInfo.Telephone) ? salesContactInfo.Telephone : String.Empty;
            supplier.SalesFax = !String.IsNullOrEmpty(salesContactInfo.Fax) ? salesContactInfo.Fax : String.Empty;
            supplier.SalesEmail = !String.IsNullOrEmpty(salesContactInfo.Email) ? salesContactInfo.Email : String.Empty;
            supplier.SalesContact1 = !String.IsNullOrEmpty(salesContactInfo.Contact1) ? salesContactInfo.Contact1 : String.Empty;
            supplier.SalesContact2 = !String.IsNullOrEmpty(salesContactInfo.Contact2) ? salesContactInfo.Contact2 : String.Empty;
        }
    }
}