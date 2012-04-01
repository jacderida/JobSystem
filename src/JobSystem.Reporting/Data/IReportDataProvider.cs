using System;
using System.Collections.Generic;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data
{
	public interface IReportDataProvider<T> where T : ReportModelBase
	{
		IList<T> GetReportData(Guid itemId);
	}
}