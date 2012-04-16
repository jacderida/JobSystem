using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;

namespace JobSystem.Reporting.Models
{
	public abstract class CustomerReportModel : ReportModelBase
	{
		public string CustomerName { get; set; }
		public string CustomerAddress1 { get; set; }
		public string CustomerAddress2 { get; set; }
		public string CustomerAddress3 { get; set; }
		public string CustomerAddress4 { get; set; }
		public string CustomerAddress5 { get; set; }
	}
}