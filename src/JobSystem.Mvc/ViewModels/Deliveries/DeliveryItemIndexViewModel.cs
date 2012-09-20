using System;

namespace JobSystem.Mvc.ViewModels.Deliveries
{
	public class DeliveryItemIndexViewModel
	{
		public Guid Id { get; set; }
		public string JobRef { get; set; }
		public string Customer { get; set; }
		public string Notes { get; set; }
	}
}