using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Admin.Mvc.Data
{
	public interface ITenantRepository
	{
		List<Tuple<Guid, string>> GetTenants();
	}
}