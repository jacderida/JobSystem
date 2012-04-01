namespace JobSystem.Reporting.Models
{
	public class DeliveryReportModel : ReportModelBase
	{
		public string DeliveryNumber { get; set; }
		public string CustomerName { get; set; }
		public string CustomerAddress1 { get; set; }
		public string CustomerAddress2 { get; set; }
		public string CustomerAddress3 { get; set; }
		public string CustomerAddress4 { get; set; }
		public string CustomerAddress5 { get; set; }
		public string PreparedBy { get; set; }
		public string DateCreated { get; set; }
		public string JobItemReference { get; set; }
		public string Instrument { get; set; }
		public string Notes { get; set; }
		public string Accessories { get; set; }
		public string CertificateAttached { get; set; }
		public string OrderNo { get; set; }
	}
}