using System;
using System.Collections.Generic;
using JobSystem.Reporting.Models;
using JobSystem.DataModel.Entities;

namespace JobSystem.Reporting.Data
{
	public interface IJobItemsAtStatusReportDataProvider
	{
		List<JobItemAtStatusReportModel> GetJobItemsAtStatusReportData(Guid statusId);
	}
}