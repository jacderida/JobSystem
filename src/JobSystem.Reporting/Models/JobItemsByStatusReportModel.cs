using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.Reporting.Models
{
    class JobItemsByStatusReportModel
    {
        public int JobNo { get; set; }
        public int ItemNo { get; set; }
        public string EquipmentDescription { get; set; }
    }
}