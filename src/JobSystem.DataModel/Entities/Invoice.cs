using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Invoices;

namespace JobSystem.DataModel.Entities
{
	public class Invoice
	{
		public virtual Guid Id { get; set; }
		public virtual string InvoiceNumber { get; set; }
		[StringLength(50, ErrorMessageResourceName = "OrderNoTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string OrderNo { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual ListItem Currency { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual BankDetails BankDetails { get; set; }
		public virtual ListItem PaymentTerm { get; set; }
		public virtual TaxCode TaxCode { get; set; }
		public virtual IList<InvoiceItem> InvoiceItems { get; set; }

		public Invoice()
		{
			InvoiceItems = new List<InvoiceItem>();
		}
	}
}