using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Orders;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class OrderItem
	{
		public virtual Guid Id { get; set; }
		public virtual Order Order { get; set; }
		public virtual int ItemNo { get; set; }
		[Required(ErrorMessageResourceName = "EmptyDescription", ErrorMessageResourceType = typeof(OrderItemMessages))]
		[StringLength(50, ErrorMessageResourceName = "InvalidDescription", ErrorMessageResourceType = typeof(OrderItemMessages))]
		public virtual string Description { get; set; }
		public virtual int Quantity { get; set; }
		[StringLength(50, ErrorMessageResourceName = "InvalidPartNo", ErrorMessageResourceType = typeof(OrderItemMessages))]
		public virtual string PartNo { get; set; }
		[StringLength(2000, ErrorMessageResourceName = "InvalidInstructions", ErrorMessageResourceType = typeof(OrderItemMessages))]
		public virtual string Instructions { get; set; }
		public virtual int DeliveryDays { get; set; }
		public virtual DateTime? DateReceived { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual decimal Price { get; set; }
		public virtual bool InvoiceReceived { get; set; }
	}
}