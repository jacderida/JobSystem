using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Delivery;

namespace JobSystem.Mvc.ViewModels.Deliveries
{
	public class DeliveryItemEditViewModel
	{
		public Guid Id { get; set; }
		public Guid DeliveryId { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InvalidNotes", ErrorMessageResourceType = typeof(DeliveryItemMessages))]
		public string Notes { get; set; }
	}
}