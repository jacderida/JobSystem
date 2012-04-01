using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
	{
	}
}