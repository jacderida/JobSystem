using System;
using System.Collections.Generic;
using System.ComponentModel;
using JobSystem.Reporting.Models;
using JobSystem.DataModel.Entities;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateInvoiceReportDataProvider : NHibernateCustomerDataProviderBase, IReportDataProvider<InvoiceReportModel>
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IList<InvoiceReportModel> GetReportData(Guid itemId)
		{
			var result = new List<InvoiceReportModel>();
			var invoice = CurrentSession.Get<Invoice>(itemId);
			var customer = invoice.Customer;
			foreach (var invoiceItem in invoice.InvoiceItems)
			{
				var reportItem = new InvoiceReportModel();
				PopulateCompanyDetails(reportItem);
				PopulateCustomerInfo(customer, reportItem);
				PopulateBankDetails(invoice.BankDetails, reportItem);
				reportItem.PaymentTerms = invoice.PaymentTerm.Name;
				//reportItem.OrderNo = invoice.order
				result.Add(reportItem);
			}
			return result;
		}

		public void PopulateBankDetails(BankDetails bankDetails, InvoiceReportModel reportItem)
		{
			reportItem.BankName = bankDetails.Name;
			reportItem.BankAddress1 = !String.IsNullOrEmpty(reportItem.BankAddress1) ? reportItem.BankAddress1 : String.Empty;
			reportItem.BankAddress2 = !String.IsNullOrEmpty(reportItem.BankAddress2) ? reportItem.BankAddress2 : String.Empty;
			reportItem.BankAddress3 = !String.IsNullOrEmpty(reportItem.BankAddress3) ? reportItem.BankAddress3 : String.Empty;
			reportItem.BankAddress4 = !String.IsNullOrEmpty(reportItem.BankAddress4) ? reportItem.BankAddress4 : String.Empty;
			reportItem.BankAddress5 = !String.IsNullOrEmpty(reportItem.BankAddress5) ? reportItem.BankAddress5 : String.Empty;
			reportItem.BankAccountNo = bankDetails.AccountNo;
			reportItem.BankSortCode = bankDetails.SortCode;
			reportItem.BankIban = bankDetails.Iban;
		}
	}
}