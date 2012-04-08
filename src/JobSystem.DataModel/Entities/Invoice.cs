using System;

namespace JobSystem.DataModel.Entities
{
	public class Invoice
	{
		public virtual Guid Id { get; set; }
		public virtual string InvoiceNumber { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual ListItem Currency { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual BankDetails BankDetails { get; set; }
		public virtual ListItem PaymentTerm { get; set; }
		public virtual TaxCode TaxCode { get; set; }
	}
}