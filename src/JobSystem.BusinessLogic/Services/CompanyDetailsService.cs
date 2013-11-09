using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Dto;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.CompanyDetails;

namespace JobSystem.BusinessLogic.Services
{
    public class CompanyDetailsService : ServiceBase
    {
        private readonly ICompanyDetailsRepository _companyDetailsRepository;
        private readonly IBankDetailsRepository _bankDetailsRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IListItemRepository _listItemRepository;
        private readonly ITaxCodeRepository _taxCodeRepository;

        public CompanyDetailsService(
            IUserContext applicationContext,
            ICompanyDetailsRepository companyDetailsRepository,
            IBankDetailsRepository bankDetailsRepository,
            ICurrencyRepository currencyRepository,
            IListItemRepository listItemRepository,
            ITaxCodeRepository taxCodeRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _companyDetailsRepository = companyDetailsRepository;
            _bankDetailsRepository = bankDetailsRepository;
            _currencyRepository = currencyRepository;
            _listItemRepository = listItemRepository;
            _taxCodeRepository = taxCodeRepository;
        }

        #region Public Interface

        public CompanyDetails Create(
            Guid id, string name, Address addressDetails,
            string telephone, string fax, string email,
            string www, string regNo, string vatRegNo,
            string termsAndConditions, Guid defaultCurrencyId, Guid defaultTaxCodeId,
            Guid defaultPaymentTermId, Guid defaultBankDetailsId, string cultureCode)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("A value must be provided for id");
            if (!CurrentUser.HasRole(UserRole.Admin))
                throw new DomainValidationException(JobSystem.Resources.CompanyDetails.Messages.InsufficientSecurityClearance, "CurrentUser");
            var companyDetails = new CompanyDetails();
            companyDetails.Id = id;
            companyDetails.Name = name;
            companyDetails.TermsAndConditions = termsAndConditions;
            companyDetails.DefaultCurrency = GetDefaultCurrency(defaultCurrencyId);
            companyDetails.DefaultPaymentTerm = GetListItem(defaultPaymentTermId);
            companyDetails.DefaultTaxCode = GetDefaultTaxCode(defaultTaxCodeId);
            companyDetails.DefaultBankDetails = GetDefaultBankDetails(defaultBankDetailsId);
            companyDetails.DefaultCultureCode = cultureCode;
            AssignAddressDetails(companyDetails, addressDetails);
            AssignContactInfo(companyDetails, telephone, fax, email, www);
            AssignRegNoInfo(companyDetails, regNo, vatRegNo);
            ValidateAnnotatedObjectThrowOnFailure(companyDetails);
            _companyDetailsRepository.Create(companyDetails);
            return companyDetails;
        }

        public CompanyDetails Edit(
            string name, Address addressDetails,
            string telephone, string fax, string email,
            string www, string regNo, string vatRegNo,
            string termsAndConditions, Guid defaultCurrencyId, Guid defaultTaxCodeId,
            Guid defaultPaymentTermId, Guid defaultBankDetailsId, string cultureCode, string quoteSummaryText, string orderAcknowledgeText)
        {
            var companyDetails = GetCompany();
            if (!CurrentUser.HasRole(UserRole.Admin))
                throw new DomainValidationException(JobSystem.Resources.CompanyDetails.Messages.InsufficientSecurityClearance, "CurrentUser");
            companyDetails.Name = name;
            companyDetails.TermsAndConditions = termsAndConditions;
            companyDetails.DefaultCurrency = GetDefaultCurrency(defaultCurrencyId);
            companyDetails.DefaultPaymentTerm = GetListItem(defaultPaymentTermId);
            companyDetails.DefaultTaxCode = GetDefaultTaxCode(defaultTaxCodeId);
            companyDetails.DefaultBankDetails = GetDefaultBankDetails(defaultBankDetailsId);
            companyDetails.DefaultCultureCode = cultureCode;
			companyDetails.QuoteSummaryText = quoteSummaryText;
			companyDetails.OrderAcknowledgeText = orderAcknowledgeText;
            AssignAddressDetails(companyDetails, addressDetails);
            AssignContactInfo(companyDetails, telephone, fax, email, www);
            AssignRegNoInfo(companyDetails, regNo, vatRegNo);
            ValidateAnnotatedObjectThrowOnFailure(companyDetails);
            _companyDetailsRepository.UpdateCompanyDetails(companyDetails);
            return companyDetails;
        }

        public CompanyDetails GetCompany()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.ViewCompanyInsufficientSecurityClearance);
            return _companyDetailsRepository.GetCompany();
        }

        public IEnumerable<TaxCode> GetTaxCodes()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.ViewTaxCodeDetailsInsufficientSecurityClearance);
            return _taxCodeRepository.GetTaxCodes();
        }

        public IEnumerable<BankDetails> GetBankDetails()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.ViewBankDetailsInsufficientSecurityClearance);
            return _bankDetailsRepository.GetBankDetails();
        }

        public Dictionary<string, string> GetSupportedCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures).OrderBy(c => c.DisplayName).ToDictionary(c => c.Name, c => c.DisplayName);
        }

        public Image GetCompanyLogo()
        {
            var logoBytes = _companyDetailsRepository.GetLogoBytes();
            return Image.FromStream(new MemoryStream(logoBytes));
        }

        #endregion
        #region Private Implementation

        private void AssignAddressDetails(CompanyDetails companyDetails, Address addressDetails)
        {
            companyDetails.Address1 = addressDetails.Line1;
            companyDetails.Address2 = addressDetails.Line2;
            companyDetails.Address3 = addressDetails.Line3;
            companyDetails.Address4 = addressDetails.Line4;
            companyDetails.Address5 = addressDetails.Line5;
        }

        private void AssignContactInfo(CompanyDetails companyDetails, string telephone, string fax, string email, string www)
        {
            companyDetails.Telephone = telephone;
            companyDetails.Fax = fax;
            companyDetails.Email = email;
            companyDetails.Www = www;
        }

        private void AssignRegNoInfo(CompanyDetails companyDetails, string regNo, string vatRegNo)
        {
            companyDetails.RegNo = regNo;
            companyDetails.VatRegNo = vatRegNo;
        }

        private ListItem GetListItem(Guid listItemId)
        {
            var listItem = _listItemRepository.GetById(listItemId);
            if (listItem == null)
                throw new ArgumentException(String.Format("There is no list item with ID {0}", listItemId));
            return listItem;
        }

        private Currency GetDefaultCurrency(Guid defaultCurrencyId)
        {
            var currency = _currencyRepository.GetById(defaultCurrencyId);
            if (currency == null)
                throw new ArgumentException(String.Format("There is no currency with ID {0}", defaultCurrencyId));
            return currency;
        }

        private TaxCode GetDefaultTaxCode(Guid defaultTaxCodeId)
        {
            var taxCode = _taxCodeRepository.GetById(defaultTaxCodeId);
            if (taxCode == null)
                throw new ArgumentException(String.Format("There is no tax code with ID {0}", defaultTaxCodeId));
            return taxCode;
        }

        private BankDetails GetDefaultBankDetails(Guid defaultBankDetailsId)
        {
            var bankDetails = _bankDetailsRepository.GetById(defaultBankDetailsId);
            if (bankDetails == null)
                throw new ArgumentException(String.Format("There is no tax code with ID {0}", defaultBankDetailsId));
            return bankDetails;
        }

        #endregion
    }
}