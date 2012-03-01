namespace JobSystem.Reporting.Models
{
	public class ConsignmentReportModel
	{
		public string ConsignmentNo { get; set; }
		public string SupplierName { get; set; }
		public string SupplierAddress1 { get; set; }
		public string SupplierAddress2 { get; set; }
		public string SupplierAddress3 { get; set; }
		public string SupplierAddress4 { get; set; }
		public string SupplierAddress5 { get; set; }
		public string SupplierTel { get; set; }
		public string SupplierFax { get; set; }
		public string DateCreated { get; set; }
		public string RaisedBy { get; set; }
		public string Description { get; set; }
		public string JobRef { get; set; }
		public string Instructions { get; set; }
	}
}