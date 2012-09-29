using System;
using System.Collections.Generic;
using System.ComponentModel;
using JobSystem.Reporting.Models;
using JobSystem.DataModel.Entities;
using System.Text;

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
				reportItem.OrderNo = invoice.OrderNo;
				var jobItem = invoiceItem.JobItem;
				reportItem.JobRef = GetJobItemReference(jobItem);
				reportItem.Description = GetInstrumentDescription(jobItem.Instrument);
				reportItem.SerialNo = jobItem.SerialNo;
				reportItem.Calibration = invoiceItem.CalibrationPrice;
				reportItem.Repair = invoiceItem.RepairPrice;
				reportItem.Parts = invoiceItem.PartsPrice;
				reportItem.Carriage = invoiceItem.CarriagePrice;
				reportItem.Investigation = invoiceItem.InvestigationPrice;
				reportItem.CurrencyMessage = invoice.Currency.DisplayMessage;
				result.Add(reportItem);
			}
			return result;
		}

		public void PopulateBankDetails(BankDetails bankDetails, InvoiceReportModel reportItem)
		{
			reportItem.BankName = bankDetails.Name;
			reportItem.BankAddress1 = !String.IsNullOrEmpty(bankDetails.Address1) ? bankDetails.Address1 : String.Empty;
			reportItem.BankAddress2 = !String.IsNullOrEmpty(bankDetails.Address2) ? bankDetails.Address2 : String.Empty;
			reportItem.BankAddress3 = !String.IsNullOrEmpty(bankDetails.Address3) ? bankDetails.Address3 : String.Empty;
			reportItem.BankAddress4 = !String.IsNullOrEmpty(bankDetails.Address4) ? bankDetails.Address4 : String.Empty;
			reportItem.BankAddress5 = !String.IsNullOrEmpty(bankDetails.Address5) ? bankDetails.Address5 : String.Empty;
			reportItem.BankAccountNo = bankDetails.AccountNo;
			reportItem.BankSortCode = bankDetails.SortCode;
			reportItem.BankIban = bankDetails.Iban;
			reportItem.BankAddress = GetFullBankAddress(reportItem);
		}

		public string GetFullBankAddress(InvoiceReportModel reportItem)
		{
			var sb = new StringBuilder();
			if (!String.IsNullOrEmpty(reportItem.BankAddress1))
				sb.AppendFormat("{0}, ", reportItem.BankAddress1);
			if (!String.IsNullOrEmpty(reportItem.BankAddress2))
				sb.AppendFormat("{0}, ", reportItem.BankAddress2);
			if (!String.IsNullOrEmpty(reportItem.BankAddress3))
				sb.AppendFormat("{0}, ", reportItem.BankAddress3);
			if (!String.IsNullOrEmpty(reportItem.BankAddress4))
				sb.AppendFormat("{0}, ", reportItem.BankAddress4);
			if (!String.IsNullOrEmpty(reportItem.BankAddress5))
				sb.AppendFormat("{0}, ", reportItem.BankAddress5);
			return sb.ToString().Trim(", ".ToCharArray());
		}
	}
}