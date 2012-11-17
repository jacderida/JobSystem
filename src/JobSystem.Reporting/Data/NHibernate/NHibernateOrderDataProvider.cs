using System;
using System.Collections.Generic;
using System.ComponentModel;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateOrderDataProvider : NHibernateSupplierDataProvider, IReportDataProvider<OrderReportModel>
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<OrderReportModel> GetReportData(Guid itemId)
		{
			var result = new List<OrderReportModel>();
			var order = CurrentSession.Get<Order>(itemId);
			var supplier = order.Supplier;
			foreach (var orderItem in order.OrderItems)
			{
				var reportItem = new OrderReportModel();
				PopulateCompanyDetails(reportItem);
				PopulateSupplierInfo(supplier, reportItem);
				reportItem.OrderNo = order.OrderNo;
				reportItem.OrderDate = order.DateCreated;
				reportItem.Contact = supplier.Contact1;
				reportItem.EquipmentDescription = orderItem.Description;
				reportItem.OrderInstructions = order.Instructions;
				reportItem.ItemInstructions = orderItem.Instructions;
				reportItem.Price = orderItem.Price;
				reportItem.Quantity = orderItem.Quantity;
				reportItem.TotalPrice = orderItem.Price * orderItem.Quantity;
				reportItem.Days = orderItem.DeliveryDays;
				reportItem.JobRef = GetJobItemReference(orderItem.JobItem);
				reportItem.PreparedBy = order.CreatedBy.Name;
				result.Add(reportItem);
			}
			return result;
		}
	}
}