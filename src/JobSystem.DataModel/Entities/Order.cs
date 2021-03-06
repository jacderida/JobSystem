﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Orders;

namespace JobSystem.DataModel.Entities
{
    [Serializable]
    public class Order
    {
        public virtual Guid Id { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        [StringLength(255, ErrorMessageResourceName = "InstructionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
        public virtual string Instructions { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual IList<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}