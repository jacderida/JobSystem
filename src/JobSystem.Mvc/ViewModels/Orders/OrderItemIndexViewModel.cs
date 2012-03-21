﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Orders
{
	public class OrderItemIndexViewModel
	{
		public Guid Id { get; set; }
		public string SupplierName { get; set; }
		public string Instructions { get; set; }
		public string Description { get; set; }
		public string PartNo { get; set; }
		public string DeliveryDays { get; set; }
		public string Price { get; set; }
		public string Quantity { get; set; }
		public Guid JobItemId { get; set; }
		public bool IsIndividual { get; set; }
		public string Currency { get; set; }
	}
}