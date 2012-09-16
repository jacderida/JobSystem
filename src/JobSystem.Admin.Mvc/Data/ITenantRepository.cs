using System;
using System.Collections.Generic;

namespace JobSystem.Admin.Mvc.Data
{
	public interface ITenantRepository
	{
		bool TenantNameExists(string tenantName);
		void InsertTenant(Guid id, string tenantName, string companyName);
		void InsertTenantConnectionString(string tenantName, string connectionString);
		List<Tuple<Guid, string, string>> GetTenants();
	}
}