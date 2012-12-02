using System;

namespace JobSystem.Mvc.ViewModels.Deliveries
{
	public class DeliveryIndexViewModel
	{
		public Guid Id { get; set; }
		public string DeliveryNo { get; set; }
		public string Notes { get; set; }
		public string Fao { get; set; }
		public string DateCreated { get; set; }
		public string CreatedBy { get; set; }
		public string CustomerName { get; set; }
		public int ItemCount { get; set; }
	}
}