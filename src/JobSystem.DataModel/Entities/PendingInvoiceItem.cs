using System;

namespace JobSystem.DataModel.Entities
{
    public class PendingInvoiceItem
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal CalibrationPrice { get; set; }
        public virtual decimal RepairPrice { get; set; }
        public virtual decimal PartsPrice { get; set; }
        public virtual decimal CarriagePrice { get; set; }
        public virtual decimal InvestigationPrice { get; set; }
        public virtual JobItem JobItem { get; set; }
        public virtual string OrderNo { get; set; }
    }
}