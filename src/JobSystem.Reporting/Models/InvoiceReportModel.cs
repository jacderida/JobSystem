using System;

namespace JobSystem.Reporting.Models
{
	public class InvoiceReportModel : CustomerReportModel
	{
		public string InvoiceNo { get; set; }
		public DateTime InvoiceDate { get; set; }
		public string OrderNo { get; set; }
		public int ItemNo { get; set; }
		public string Description { get; set; }
		public decimal Repair { get; set; }
		public decimal Calibration { get; set; }
		public decimal Carriage { get; set; }
		public decimal Parts { get; set; }
		public decimal Investigation { get; set; }
		public string JobRef { get; set; }
		public string SerialNo { get; set; }
		public string BankName { get; set; }
		public string BankAddress { get; set; }
		public string BankAddress1 { get; set; }
		public string BankAddress2 { get; set; }
		public string BankAddress3 { get; set; }
		public string BankAddress4 { get; set; }
		public string BankAddress5 { get; set; }
		public string BankAccountNo { get; set; }
		public string BankSortCode { get; set; }
		public string BankIban { get; set; }
		public string PaymentTerms { get; set; }
		public string CurrencyMessage { get; set; }
	}
}