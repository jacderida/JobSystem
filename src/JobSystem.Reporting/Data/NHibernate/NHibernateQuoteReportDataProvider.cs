﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
                reportItem.OrderNo = quote.OrderNumber;
                reportItem.AdviceNo = quote.AdviceNumber;
                reportItem.Contact = quote.Customer.Contact1;
                reportItem.CustomerName = quote.Customer.Name;
                reportItem.AssetLine = customer.AssetLine;
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
                ApplySubTotal(reportItem);
                reportItem.Report = !String.IsNullOrEmpty(item.Report) ? item.Report : String.Empty;
                reportItem.Days = string.Format("{0} days from go ahead", item.Days);
                reportItem.JobRef = GetJobItemReference(item.JobItem);
                reportItem.PreparedBy = quote.CreatedBy.Name;
                reportItem.Instrument = GetDescription(item.JobItem);
                reportItem.CurrencyMessage = quote.Currency.DisplayMessage;
                reportItem.OrderEmailLabel =
                    String.Format("If orders are being sent by email, please use the following address: {0}", reportItem.CompanyEmail);
                result.Add(reportItem);
            }
            ApplyTotal(result);
            return result.OrderBy(i => i.JobRef).ToList();
        }

        private void ApplyTotal(List<QuoteReportModel> items)
        {
            var total = items.Sum(i => i.Calibration + i.Repair + i.Carriage + i.Parts);
            foreach (var item in items)
                item.Total = total;
        }

        private void ApplySubTotal(QuoteReportModel reportItem)
        {
            reportItem.SubTotal = reportItem.Calibration + reportItem.Repair + reportItem.Carriage + reportItem.Parts;
        }

        private string GetDescription(JobItem jobItem)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, Serial No: {1}", GetInstrumentDescription(jobItem.Instrument), jobItem.SerialNo);
            if (!String.IsNullOrEmpty(jobItem.AssetNo))
                sb.AppendFormat(", Asset No: {0}", jobItem.AssetNo);
            return sb.ToString();
        }
    }
}