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
	}
}