using System;
using System.Collections.Generic;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data
{
	public interface IConsignmentReportDataProvider
	{
		List<ConsignmentReportModel> GetConsignmentReportData(Guid consignmentId);
	}
}