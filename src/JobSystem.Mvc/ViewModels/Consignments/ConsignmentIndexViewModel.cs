using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSystem.Mvc.ViewModels.Consignments
{
    public class ConsignmentIndexViewModel
    {
        public Guid Id { get; set; }
        public string ConsignmentNo { get; set; }
        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public string SupplierName { get; set; }
        public int ItemCount { get; set; }
        public bool IsOrdered { get; set; }
    }
}