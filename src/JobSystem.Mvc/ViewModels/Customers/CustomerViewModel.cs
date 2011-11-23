using System;
using JobSystem.DataModel.Dto;

namespace JobSystem.Mvc.ViewModels.Customers
{
	public class CustomerViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Address TradingAddress { get; set; }
	}
}