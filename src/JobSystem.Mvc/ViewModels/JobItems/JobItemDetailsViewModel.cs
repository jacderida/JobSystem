using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobSystem.Mvc.ViewModels.WorkItems;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.Quotes;
using JobSystem.Mvc.ViewModels.Orders;

namespace JobSystem.Mvc.ViewModels.JobItems
{
	public class JobItemDetailsViewModel
	{
		public Guid Id { get; set; }
		public Guid JobId { get; set; }
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
		public ConsignmentItemIndexViewModel ConsignmentItem { get; set; }
		public ConsignmentIndexViewModel Consignment { get; set; }
		public QuoteItemIndexViewModel QuoteItem { get; set; }
		public QuoteIndexViewModel Quote { get; set; }
		public OrderItemIndexViewModel OrderItem { get; set; }
		public OrderIndexViewModel Order { get; set; }
		public string Instrument { get; set; }
		public string InitialStatus { get; set; }
		public string Location { get; set; }
		public string Field { get; set; }
	}
}