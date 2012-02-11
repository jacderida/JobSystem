using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Orders
{
	public class OrderCreateViewModel
	{
		public Guid Id { get; set; }
		[Display(Name = "Supplier")]
		public Guid SupplierId { get; set; }
		public string Instructions { get; set; }
		public string Description { get; set; }
		[Display(Name = "Part Number")]
		public string PartNo { get; set; }
		[Display(Name = "Delivery Days")]
		public string DeliveryDays { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public bool Requisition { get; set; }
		public Guid JobItemId { get; set; }
	}
}