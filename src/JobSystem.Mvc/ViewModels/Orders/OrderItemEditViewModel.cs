using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace JobSystem.Mvc.ViewModels.Orders
{
	public class OrderItemEditViewModel
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		[Display(Name = "Supplier")]
		[Required]
		public Guid SupplierId { get; set; }
		public string SupplierName { get; set; }
		public string Instructions { get; set; }
		public string Description { get; set; }
		[Display(Name = "Part Number")]
		public string PartNo { get; set; }
		[Integer]
		[Min(0, ErrorMessage = "Please enter a value of at least 0.")]
		[Required]
		public int DeliveryDays { get; set; }
		[Min(0, ErrorMessage = "Please enter a value of at least 0.")]
		[Required]
		public decimal Price { get; set; }
		[Integer]
		[Min(1, ErrorMessage = "Please enter a value of at least 1.")]
		[Required]
		public int Quantity { get; set; }
		[Display(Name = "Currency")]
		public IEnumerable<SelectListItem> Currencies { get; set; }
		public Guid CurrencyId { get; set; }
		public Guid JobItemId { get; set; }
		public Guid JobId { get; set; }
	}
}