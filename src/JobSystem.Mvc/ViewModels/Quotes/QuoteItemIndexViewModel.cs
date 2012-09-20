using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.DataModel.Entities;

namespace JobSystem.Mvc.ViewModels.Quotes
{
	public class QuoteItemIndexViewModel
	{
		public Guid Id { get; set; }
		public Guid JobItemId { get; set; }
		public Guid JobId { get; set; }
		[DataType(DataType.Currency)]
		public double Repair { get; set; }
		public double Calibration { get; set; }
		public double Parts { get; set; }
		public double Carriage { get; set; }
		public double Investigation { get; set; }
		public int Days { get; set; }
		[Display(Name = "Item BER")]
		public bool ItemBER { get; set; }
		[Display(Name = "Order Number")]
		public string OrderNo { get; set; }
		[Display(Name = "Advice Number")]
		public string AdviceNo { get; set; }
		public string Report { get; set; }
		public string JobNo { get; set; }
		public string ItemNo { get; set; }
		public bool IsQuoted { get; set; }
		public string JobItemRef { get; set; }
		public string Status { get; set; }
		public string QuoteNo { get; set; }
		public ListItemType StatusType { get; set; }
	}
}