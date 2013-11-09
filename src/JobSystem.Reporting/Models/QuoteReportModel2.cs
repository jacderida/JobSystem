namespace JobSystem.Reporting.Models
{
    public class QuoteReportModel2 : ReportModelBase
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
        public string JobNo { get; set; }
        public string ItemNo { get; set; }
        public string JobItemRef { get; set; }
        public string LineDescription { get; set; }
        public string Report { get; set; }
        public decimal SubTotal { get; set; }
        public string Days { get; set; }
        public string PreparedBy { get; set; }
        public string CurrencyMessage { get; set; }
        public decimal Total { get; set; }
        public string OrderEmailLabel { get; set; }
        public decimal RepairTotal { get; set; }
        public decimal CalibrationTotal { get; set; }
        public decimal CarriageTotal { get; set; }
        public decimal PartsTotal { get; set; }
        public decimal InvestigationTotal { get; set; }
		public string SummaryText { get; set; }
    }
}