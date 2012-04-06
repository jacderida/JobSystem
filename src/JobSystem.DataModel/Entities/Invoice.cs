using System;

namespace JobSystem.DataModel.Entities
{
	public class Invoice
	{
		public Guid Id { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime DateCreated { get; set; }
		public ListItem Currency { get; set; }
		public Customer Customer { get; set; }
		public BankDetails BankDetails { get; set; }
		public ListItem PaymentTerm { get; set; }
		public TaxCode TaxCode { get; set; }
	}
}