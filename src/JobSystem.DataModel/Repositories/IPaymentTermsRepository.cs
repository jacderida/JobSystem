using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	public interface IPaymentTermsRepository : IReadWriteRepository<PaymentTerm, Guid>
	{
	}
}