using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.Reporting.Data.NHibernate
{
	public abstract class NHibernateReportDataProviderBase
	{
		public ISession CurrentSession
		{
			get { return NHibernateSession.Current; }
		}
	}
}