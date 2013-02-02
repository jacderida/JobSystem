namespace JobSystem.Reporting.Models
{
	public class QuoteReportModel : ReportModelBase
	{
		public string CustomerName { get; set; }
	    public string AssetLine { get; set; }
	    public string CustomerAddress1 { get; set; }
		public string CustomerAddress2 { get; set; }
		public string CustomerAddress3 { get; set; }
		public string CustomerAddress4 { get; set; }
		public string CustomerAddress5 { get; set; }
		public string QuoteNo { get; set; }
		public string DateCreated { get; set; }
		public string OrderNo { get; set; }
		public string AdviceNo { get; set; }
		public string Contact { get; set; }
		public string Telephone { get; set; }
		public string Fax { get; set; }
		public string JobNo { get; set; }
		public string ItemNo { get; set; }
		public string Instrument { get; set; }
		public string Report { get; set; }
		public decimal Repair { get; set; }
		public decimal Calibration { get; set; }
		public decimal Carriage { get; set; }
		public decimal Parts { get; set; }
		public decimal Investigation { get; set; }
	    public decimal SubTotal { get; set; }
	    public string Days { get; set; }
		public string JobRef { get; set; }
		public string PreparedBy { get; set; }
		public string CurrencyMessage { get; set; }
		public decimal Total { get; set; }
		public string OrderEmailLabel { get; set; }
	}
}