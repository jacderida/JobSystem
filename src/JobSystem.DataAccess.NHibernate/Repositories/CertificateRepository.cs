using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
	{
		public IEnumerable<Certificate> GetCertificates()
		{
			return CurrentSession.Query<Certificate>();
		}

		public IEnumerable<Certificate> SearchByKeyword(string keyword)
		{
			return CurrentSession.Query<Certificate>().Where(c => c.CertificateNumber.ToLower().Contains(keyword.ToLower()));
		}

		public IEnumerable<Certificate> GetCertificatesForJobItem(Guid jobItemId)
		{
			return CurrentSession.Query<Certificate>().Where(ji => ji.JobItem.Id == jobItemId);
		}
	}
}