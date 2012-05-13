using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ICertificateRepository : IReadWriteRepository<Certificate, Guid>
	{
		IEnumerable<Certificate> GetCertificates();
		IEnumerable<Certificate> GetCertificatesForJobItem(Guid jobItemId);
		IEnumerable<Certificate> SearchByKeyword(string keyword);
	}
}