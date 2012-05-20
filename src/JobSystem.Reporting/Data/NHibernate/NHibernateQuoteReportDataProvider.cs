using System;
using System.Collections.Generic;
using System.ComponentModel;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateQuoteReportDataProvider : NHibernateReportDataProviderBase, IQuoteReportDataProvider
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<QuoteReportModel> GetQuoteReportData(Guid quoteId)
		{
			var result = new List<QuoteReportModel>();
			var quote = CurrentSession.Get<Quote>(quoteId);
			var customer = quote.Customer;
			foreach (var item in quote.QuoteItems)
			{
				var reportItem = new QuoteReportModel();
				PopulateCompanyDetails(reportItem);
				reportItem.QuoteNo = quote.QuoteNumber;
				reportItem.CustomerName = quote.Customer.Name;
				reportItem.CustomerAddress1 = !String.IsNullOrEmpty(customer.Address1) ? customer.Address1 : String.Empty;
				reportItem.CustomerAddress2 = !String.IsNullOrEmpty(customer.Address2) ? customer.Address2 : String.Empty;
				reportItem.CustomerAddress3 = !String.IsNullOrEmpty(customer.Address3) ? customer.Address3 : String.Empty;
				reportItem.CustomerAddress4 = !String.IsNullOrEmpty(customer.Address4) ? customer.Address4 : String.Empty;
				reportItem.CustomerAddress5 = !String.IsNullOrEmpty(customer.Address5) ? customer.Address5 : String.Empty;
				reportItem.Telephone = !String.IsNullOrEmpty(customer.Telephone) ? customer.Telephone : String.Empty;
				reportItem.Fax = !String.IsNullOrEmpty(customer.Fax) ? customer.Fax : String.Empty;
				reportItem.JobNo = item.JobItem.Job.JobNo;
				reportItem.ItemNo = item.ItemNo.ToString();
				reportItem.Calibration = item.Calibration;
				reportItem.Repair = item.Labour;
				reportItem.Carriage = item.Carriage;
				reportItem.Parts = item.Parts;
				reportItem.Investigation = item.Investigation;
				reportItem.Report = !String.IsNullOrEmpty(item.Report) ? item.Report : String.Empty;
				reportItem.Days = item.Days.ToString();
				reportItem.JobRef = GetJobItemReference(item.JobItem);
				reportItem.PreparedBy = quote.CreatedBy.Name;
				reportItem.Instrument = GetInstrumentDescription(item.JobItem.Instrument);
				reportItem.CurrencyMessage = quote.Currency.DisplayMessage;
				result.Add(reportItem);
			}
			return result;
		}
	}
}