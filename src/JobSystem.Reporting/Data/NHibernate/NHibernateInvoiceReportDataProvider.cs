using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                reportItem.InvoiceNo = invoice.InvoiceNumber;
                reportItem.InvoiceDate = invoice.DateCreated;
                reportItem.PaymentTerms = invoice.PaymentTerm.Name;
                reportItem.OrderNo = invoice.OrderNo;
                var jobItem = invoiceItem.JobItem;
                reportItem.JobRef = GetJobItemReference(jobItem);
                reportItem.Description = GetDescription(jobItem, invoice.Currency, invoiceItem.CarriagePrice);
                reportItem.SerialNo = jobItem.SerialNo;
                reportItem.Calibration = invoiceItem.CalibrationPrice;
                reportItem.Repair = invoiceItem.RepairPrice;
                reportItem.Parts = invoiceItem.PartsPrice;
                reportItem.Carriage = invoiceItem.CarriagePrice;
                reportItem.Investigation = invoiceItem.InvestigationPrice;
                reportItem.CurrencyMessage = invoice.Currency.DisplayMessage;
                reportItem.Currency = invoice.Currency.Name;
                result.Add(reportItem);
            }
            ApplyTotals(result, invoice);
            return result.OrderBy(i => i.JobRef).ToList();
        }

        private void PopulateBankDetails(BankDetails bankDetails, InvoiceReportModel reportItem)
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

        private string GetFullBankAddress(InvoiceReportModel reportItem)
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

        private string GetDescription(JobItem jobItem, Currency currency, decimal carriage)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, Serial No: {1}", GetInstrumentDescription(jobItem.Instrument), jobItem.SerialNo);
            if (carriage > 0)
            {
                var currencySymbol = String.Empty;
                switch (currency.Name)
                {
                    case "GBP":
                        currencySymbol = "£";
                        break;
                    case "USD":
                        currencySymbol = "$";
                        break;
                    default:
                        currencySymbol = currency.Name;
                        break;
                }
                sb.AppendFormat(" (includes {0}{1} for carriage)", currencySymbol, String.Format("{0:0.00}", carriage));
            }
            return sb.ToString();
        }

        private void ApplyTotals(List<InvoiceReportModel> items, Invoice invoice)
        {
            var subTotal = items.Sum(i => i.Calibration + i.Repair + i.Parts + i.Carriage + i.Investigation);
            var vatTotal = subTotal * (decimal)invoice.TaxCode.Rate;
            var total = subTotal + vatTotal;
            foreach (var item in items)
            {
                item.SubTotal = subTotal;
                item.VatTotal = vatTotal;
                item.VatLabel = invoice.TaxCode.Description;
                item.Total = total;
            }
        }
    }
}