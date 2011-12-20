using System;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
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
			return CurrentSession.Query<CompanyDetails>().Single();
		}
	}
}