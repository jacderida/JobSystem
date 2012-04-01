using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface ICertificateRepository : IReadWriteRepository<Certificate, Guid>
	{
	}
}