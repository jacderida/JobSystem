using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;
using NHibernate.Linq;

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
				GetAddressCustomerDetails(reportItem, customer);
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

		public void GetAddressCustomerDetails(DeliveryReportModel reportItem, Customer customer)
		{
			if (!String.IsNullOrEmpty(customer.DeliveryTitle))
				GetDeliveryDetails(reportItem, customer);
			else
				GetCustomerDetails(reportItem, customer);
		}

		private void GetDeliveryDetails(DeliveryReportModel reportItem, Customer customer)
		{
			reportItem.CustomerName = customer.DeliveryTitle;
			var addressLines = new string[7];
			addressLines[0] = customer.DeliveryAddress1.Trim();
			addressLines[1] = customer.DeliveryAddress2.Trim();
			addressLines[2] = customer.DeliveryAddress3.Trim();
			addressLines[3] = customer.DeliveryAddress4.Trim();
			addressLines[4] = customer.DeliveryAddress5.Trim();
			addressLines[5] = customer.DeliveryAddress6.Trim();
			addressLines[6] = customer.DeliveryAddress7.Trim();
			var i = 0;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress1 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress2 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress3 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress4 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress5 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.AdditionalLine1 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.AdditionalLine2 = i < addressLines.Length ? addressLines[i++] : String.Empty;
		}

		private void GetCustomerDetails(DeliveryReportModel reportItem, Customer customer)
		{
			reportItem.CustomerName = customer.Name;
			var addressLines = new string[5];
			addressLines[0] = customer.Address1.Trim();
			addressLines[1] = customer.Address2.Trim();
			addressLines[2] = customer.Address3.Trim();
			addressLines[3] = customer.Address4.Trim();
			addressLines[4] = customer.Address5.Trim();
			var i = 0;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress1 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress2 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress3 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress4 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportItem.CustomerAddress5 = i < addressLines.Length ? addressLines[i++] : String.Empty;
		}
	}
}