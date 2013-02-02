using System;

namespace JobSystem.Reporting.Models
{
    public class EquipmentProgressReportModel : ReportModelBase
    {
        public string CustomerName { get; set; }
        public string OrderNo { get; set; }
        public string AdviceNo { get; set; }
        public string JobRef { get; set; }
        public int ItemNo { get; set; }
        public DateTime DateReceived { get; set; }
        public string EquipmentDescription { get; set; }
        public string SerialNo { get; set; }
        public string AssetNo { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public DateTime? DueDate { get; set; }
    }
}