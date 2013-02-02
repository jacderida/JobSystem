using System;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
    public class CompanyDetailsRepository : RepositoryBase<CompanyDetails>, ICompanyDetailsRepository
    {
        public void ChangeMainLogo(Guid companyId, byte[] mainLogo)
        {
            throw new NotImplementedException();
        }

        public CompanyDetails GetCompany()
        {
            var companyDetails = CurrentSession.Query<CompanyDetails>().Single();
            NHibernateUtil.Initialize(companyDetails.DefaultBankDetails);
            NHibernateUtil.Initialize(companyDetails.DefaultCurrency);
            NHibernateUtil.Initialize(companyDetails.DefaultPaymentTerm);
            NHibernateUtil.Initialize(companyDetails.DefaultTaxCode);
            return companyDetails;
        }

        public byte[] GetLogoBytes()
        {
            return CurrentSession.Query<CompanyDetails>().Single().MainLogo;
        }

        public bool ApplyAllPrices()
        {
            return CurrentSession.Query<CompanyDetails>().Single().ApplyAllPrices;
        }

        public void UpdateCompanyDetails(CompanyDetails companyDetails)
        {
            var companyDetailsToUpdate = CurrentSession.Get<CompanyDetails>(companyDetails.Id);
            companyDetailsToUpdate.Name = companyDetails.Name;
            companyDetailsToUpdate.TermsAndConditions = companyDetails.TermsAndConditions;
            companyDetailsToUpdate.DefaultCurrency = companyDetails.DefaultCurrency;
            companyDetailsToUpdate.DefaultPaymentTerm = companyDetails.DefaultPaymentTerm;
            companyDetailsToUpdate.DefaultTaxCode = companyDetails.DefaultTaxCode;
            companyDetailsToUpdate.DefaultBankDetails = companyDetails.DefaultBankDetails;
            companyDetailsToUpdate.Address1 = companyDetails.Address1;
            companyDetailsToUpdate.Address2 = companyDetails.Address2;
            companyDetailsToUpdate.Address3 = companyDetails.Address3;
            companyDetailsToUpdate.Address4 = companyDetails.Address4;
            companyDetailsToUpdate.Address5 = companyDetails.Address5;
            companyDetailsToUpdate.Telephone = companyDetails.Telephone;
            companyDetailsToUpdate.Fax = companyDetails.Fax;
            companyDetailsToUpdate.Email = companyDetails.Email;
            companyDetailsToUpdate.Www = companyDetails.Www;
            companyDetailsToUpdate.RegNo = companyDetails.RegNo;
            companyDetailsToUpdate.VatRegNo = companyDetails.VatRegNo;
            CurrentSession.Update(companyDetailsToUpdate);
        }
    }
}