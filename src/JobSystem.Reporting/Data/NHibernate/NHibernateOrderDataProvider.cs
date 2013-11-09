using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;
using NHibernate.Linq;

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
            var itemNo = 1;
			var company = CurrentSession.Query<CompanyDetails>().Single();
            foreach (var orderItem in order.OrderItems)
            {
                var carriage = orderItem.Carriage.HasValue ? orderItem.Carriage.Value : 0;
                var reportItem = new OrderReportModel();
                PopulateCompanyDetails(reportItem);
                PopulateSupplierInfo(supplier, reportItem);
                reportItem.ItemNo = itemNo++;
                reportItem.OrderNo = order.OrderNo;
                reportItem.OrderDate = order.DateCreated;
                reportItem.Contact = supplier.Contact1;
                reportItem.EquipmentDescription = orderItem.Description;
                reportItem.OrderInstructions = order.Instructions;
                reportItem.ItemInstructions = orderItem.Instructions;
                reportItem.Price = orderItem.Price;
                reportItem.Carriage = carriage;
                reportItem.Quantity = orderItem.Quantity;
                reportItem.TotalPrice = (orderItem.Price * orderItem.Quantity) + carriage;
                reportItem.Days = orderItem.DeliveryDays;
                reportItem.JobRef = GetJobItemReference(orderItem.JobItem);
                reportItem.PreparedBy = order.CreatedBy.Name;
                reportItem.PartNo = orderItem.PartNo;
				reportItem.AcknowledgeText = company.OrderAcknowledgeText;
                result.Add(reportItem);
            }
            return result.OrderBy(o => o.ItemNo).ToList();
        }
    }
}