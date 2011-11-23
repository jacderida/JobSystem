using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel;

namespace JobSystem.BusinessLogic.Tests.Context
{
	public class TestRepositorySessionScope : IRepositorySessionScope
	{
		public void BeginTransaction()
		{
			throw new NotImplementedException();
		}

		public void RollbackTransaction()
		{
			throw new NotImplementedException();
		}

		public void CommitTransaction()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}