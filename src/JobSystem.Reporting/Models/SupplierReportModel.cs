using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.Reporting.Models
{
	public abstract class SupplierReportModel : ReportModelBase
	{
		public string SupplierName { get; set; }
		public string SupplierAddress1 { get; set; }
		public string SupplierAddress2 { get; set; }
		public string SupplierAddress3 { get; set; }
		public string SupplierAddress4 { get; set; }
		public string SupplierAddress5 { get; set; }
		public string SupplierTel { get; set; }
		public string SupplierFax { get; set; }
	}
}