using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.ViewModels.Certificates;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.Deliveries;
using JobSystem.Mvc.ViewModels.Orders;
using JobSystem.Mvc.ViewModels.Quotes;
using JobSystem.Mvc.ViewModels.WorkItems;

namespace JobSystem.Mvc.ViewModels.JobItems
{
    public class JobItemDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string JobItemRef { get; set; }
        public string SerialNo { get; set; }
        public string AssetNo { get; set; }
        public int CalPeriod { get; set; }
        public string Instructions { get; set; }
        public string Accessories { get; set; }
        public bool IsReturned { get; set; }
        public string ReturnReason { get; set; }
        public string Comments { get; set; }
        public string InstrumentDetails { get; set; }
        public IList<WorkItemDetailsViewModel> WorkItems { get; set; }
        public IList<CertificateIndexViewModel> Certificates { get; set; }
        public ConsignmentItemIndexViewModel ConsignmentItem { get; set; }
        public ConsignmentIndexViewModel Consignment { get; set; }
        public QuoteItemIndexViewModel QuoteItem { get; set; }
        public QuoteIndexViewModel Quote { get; set; }
        public OrderItemIndexViewModel OrderItem { get; set; }
        public OrderIndexViewModel Order { get; set; }
        public DeliveryIndexViewModel Delivery { get; set; }
        public DeliveryItemIndexViewModel DeliveryItem { get; set; }
        public string Instrument { get; set; }
        public string Status { get; set; }
        public string Field { get; set; }
        public bool IsInvoiced { get; set; }
        public bool IsMarkedForInvoicing { get; set; }
        public UserRole UserRole { get; set; }
    }
}