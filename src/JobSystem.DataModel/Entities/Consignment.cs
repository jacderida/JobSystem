using System;
using System.Collections.Generic;

namespace JobSystem.DataModel.Entities
{
    [Serializable]
    public class Consignment
    {
        public virtual Guid Id { get; set; }
        public virtual string ConsignmentNo { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public virtual IList<ConsignmentItem> Items { get; set; }
        public virtual bool IsOrdered { get; set; }

        public Consignment()
        {
            Items = new List<ConsignmentItem>();
        }
    }
}