﻿namespace JobSystem.Reporting.Models
{
    public class DeliveryReportModel : CustomerReportModel
    {
        public string AdditionalLine1 { get; set; }
        public string AdditionalLine2 { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string DeliveryNumber { get; set; }
        public string PreparedBy { get; set; }
        public string DateCreated { get; set; }
        public string JobItemReference { get; set; }
        public string Instrument { get; set; }
        public string Notes { get; set; }
        public string Accessories { get; set; }
        public string CertificateAttached { get; set; }
        public string OrderNo { get; set; }
    }
}