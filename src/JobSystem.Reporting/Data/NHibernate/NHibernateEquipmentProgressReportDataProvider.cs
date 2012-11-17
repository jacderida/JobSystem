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
	public class NHibernateEquipmentProgressReportDataProvider : NHibernateReportDataProviderBase, IEquipmentProgressReportDataProvider
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<EquipmentProgressReportModel> GetEquipmentProgressReportData(Guid customerId)
		{
			var result = new List<EquipmentProgressReportModel>();
			var jobItemsNotInvoiced = CurrentSession.Query<JobItem>().Where(ji => ji.Job.Customer.Id == customerId &&  ji.Status.Type != ListItemType.StatusInvoiced);
			foreach (var jobItem in jobItemsNotInvoiced)
			{
				var reportItem = new EquipmentProgressReportModel();
				PopulateCompanyDetails(reportItem);
				var job = jobItem.Job;
				reportItem.JobRef = job.JobNo;
				reportItem.CustomerName = GetCustomerName(job.Customer);
				reportItem.OrderNo = job.OrderNo;
				reportItem.AdviceNo = job.AdviceNo;
				reportItem.DateReceived = job.DateCreated;
				reportItem.SerialNo = jobItem.SerialNo;
				reportItem.AssetNo = jobItem.AssetNo;
				reportItem.ItemNo = jobItem.ItemNo;
				reportItem.Status = jobItem.Status.Name;
				reportItem.EquipmentDescription = jobItem.Instrument.ToString();
				var quoteItem = CurrentSession.Query<QuoteItem>().Where(qi => qi.JobItem.Id == jobItem.Id).SingleOrDefault();
				if (quoteItem != null)
				{
					var quote = quoteItem.Quote;
					if (!String.IsNullOrEmpty(quote.OrderNumber))
						reportItem.OrderNo =  quote.QuoteNumber;
					if (!String.IsNullOrEmpty(quote.AdviceNumber))
						reportItem.AdviceNo =  quote.AdviceNumber;
					reportItem.Cost += quoteItem.Labour;
					reportItem.Cost += quoteItem.Calibration;
					reportItem.Cost += quoteItem.Parts;
					reportItem.Cost += quoteItem.Carriage;
					reportItem.DueDate = job.DateCreated.AddDays(quoteItem.Days);
				}
				result.Add(reportItem);
			}
			return result;
		}

		private string GetCustomerName(Customer customer)
		{
			var sb = new StringBuilder();
			sb.Append(customer.Name);
			if (!String.IsNullOrEmpty(customer.AssetLine))
				sb.AppendFormat(" - {0}", customer.AssetLine);
			return sb.ToString();
		}
	}
}