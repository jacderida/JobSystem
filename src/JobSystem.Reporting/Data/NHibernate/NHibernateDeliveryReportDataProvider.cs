using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JobSystem.Reporting.Models;
using JobSystem.DataModel.Entities;
using NHibernate.Linq;
using System.Text;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateDeliveryReportDataProvider : NHibernateReportDataProviderBase, IReportDataProvider<DeliveryReportModel>
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<DeliveryReportModel> GetReportData(Guid itemId)
		{
			var result = new List<DeliveryReportModel>();
			var delivery = CurrentSession.Get<Delivery>(itemId);
			var customer = delivery.Customer;
			foreach (var deliveryItem in delivery.DeliveryItems)
			{
				var reportItem = new DeliveryReportModel();
				PopulateCompanyDetails(reportItem);
				reportItem.DeliveryNumber = delivery.DeliveryNoteNumber;
				reportItem.CustomerName = customer.Name;
				reportItem.CustomerAddress1 = !String.IsNullOrEmpty(customer.Address1) ? customer.Address1 : String.Empty;
				reportItem.CustomerAddress2 = !String.IsNullOrEmpty(customer.Address2) ? customer.Address2 : String.Empty;
				reportItem.CustomerAddress3 = !String.IsNullOrEmpty(customer.Address3) ? customer.Address3 : String.Empty;
				reportItem.CustomerAddress4 = !String.IsNullOrEmpty(customer.Address4) ? customer.Address4 : String.Empty;
				reportItem.CustomerAddress5 = !String.IsNullOrEmpty(customer.Address5) ? customer.Address5 : String.Empty;
				reportItem.PreparedBy = String.Format(delivery.CreatedBy.Name);
				reportItem.DateCreated = delivery.DateCreated.ToShortDateString();
				var jobItem = deliveryItem.JobItem;
				reportItem.JobItemReference = GetJobItemReference(jobItem);
				reportItem.Instrument = GetDescription(jobItem);
				reportItem.Notes = !String.IsNullOrEmpty(deliveryItem.Notes) ? deliveryItem.Notes : String.Empty;
				reportItem.Accessories = !String.IsNullOrEmpty(jobItem.Accessories) ? jobItem.Accessories : String.Empty;
				reportItem.CertificateAttached = GetCertificateAttached(jobItem.Id);
				reportItem.OrderNo = GetOrderNumber(deliveryItem);
				result.Add(reportItem);
			}
			return result.OrderBy(i => i.JobItemReference).ToList();
		}

		public string GetCertificateAttached(Guid jobItemId)
		{
			return CurrentSession.Query<Certificate>().Where(ci => ci.JobItem.Id == jobItemId).Count() > 0 ? "Yes" : "No";
		}

		public string GetOrderNumber(DeliveryItem deliveryItem)
		{
			var quoteItem = deliveryItem.QuoteItem;
			if (quoteItem != null)
				return !String.IsNullOrEmpty(quoteItem.Quote.OrderNumber) ? quoteItem.Quote.OrderNumber : String.Empty;
			return !String.IsNullOrEmpty(deliveryItem.JobItem.Job.OrderNo) ? deliveryItem.JobItem.Job.OrderNo : String.Empty;
		}

		public string GetDescription(JobItem jobItem)
		{
			var sb = new StringBuilder();
			sb.Append(GetInstrumentDescription(jobItem.Instrument));
			if (!String.IsNullOrEmpty(jobItem.Accessories))
			{
				sb.Append(Environment.NewLine);
				sb.AppendLine("Accessories:");
				sb.Append(jobItem.Accessories);
			}
			return sb.ToString();
		}
	}
}