using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Delivery;

namespace JobSystem.DataModel.Entities
{
    [Serializable]
    public class DeliveryItem
    {
        public virtual Guid Id { get; set; }
        public virtual int ItemNo { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual JobItem JobItem { get; set; }
        public virtual QuoteItem QuoteItem { get; set; }
        [StringLength(255, ErrorMessageResourceName = "InvalidNotes", ErrorMessageResourceType = typeof(DeliveryItemMessages))]
        public virtual string Notes { get; set; }
    }
}