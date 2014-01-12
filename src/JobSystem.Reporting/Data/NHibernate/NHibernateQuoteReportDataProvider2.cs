using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;
using NHibernate.Linq;

namespace JobSystem.Reporting.Data.NHibernate
{
    [DataObject]
    public class NHibernateQuoteReportDataProvider2 : NHibernateCustomerDataProviderBase, IReportDataProvider<QuoteReportModel2>
    {
        private int _itemNo = 1;
        private decimal _repairTotal;
        private decimal _calibrationTotal;
        private decimal _partsTotal;
        private decimal _carriageTotal;
        private decimal _investigationTotal;

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<QuoteReportModel2> GetReportData(Guid itemId)
        {
            var result = new List<QuoteReportModel2>();
            var quote = CurrentSession.Get<Quote>(itemId);
            foreach (var item in quote.QuoteItems.OrderBy(q => q.JobItem.ItemNo))
            {
				AddRepairLineItem(result, quote, item);
                AddCalibrationLineItem(result, quote, item);
                AddPartsLineItem(result, quote, item);
                AddCarriageLineItem(result, quote, item);
                AddInvestigationLineItem(result, quote, item);
                AddReportLineItem(result, quote, item);
                AddDeliveryLineItem(result, quote, item);
                _itemNo = 1;
            }
            ApplySummary(result);
            ApplyTotal(result);
			ApplyDate(result, quote);
            return result;
        }

		private void ApplyDate(IEnumerable<QuoteReportModel2> items, Quote quote)
		{
			foreach (var item in items)
				item.DateCreated = quote.DateCreated.ToShortDateString();
		}

        private void ApplySummary(IEnumerable<QuoteReportModel2> items)
        {
			var company = CurrentSession.Query<CompanyDetails>().Single();
            foreach (var item in items)
            {
                item.RepairTotal = _repairTotal;
                item.CalibrationTotal = _calibrationTotal;
                item.PartsTotal = _partsTotal;
                item.CarriageTotal = _carriageTotal;
                item.InvestigationTotal = _investigationTotal;
				item.SummaryText = company.QuoteSummaryText;
            }
        }

        private void ApplyTotal(IEnumerable<QuoteReportModel2> items)
        {
            var total = _repairTotal;
			total += _calibrationTotal;
			total += _partsTotal;
			total += _carriageTotal;
            foreach (var item in items)
                item.Total = total;
        }

        private void AddRepairLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (quoteItem.Labour != 0)
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, _itemNo++);
                reportItem.LineDescription = string.Format("Repair for {0}, Serial No: {1}.", GetInstrumentDescription(quoteItem.JobItem.Instrument), quoteItem.JobItem.SerialNo);
                reportItem.Price = quoteItem.Labour;
                result.Add(reportItem);
                _repairTotal += quoteItem.Labour;
            }
        }

        private void AddCalibrationLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (quoteItem.Calibration != 0)
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, _itemNo++);
                reportItem.LineDescription = string.Format("Calibration for {0}, Serial No: {1}", GetInstrumentDescription(quoteItem.JobItem.Instrument), quoteItem.JobItem.SerialNo);
                reportItem.Price = quoteItem.Calibration;
                result.Add(reportItem);
                _calibrationTotal += quoteItem.Calibration;
            }
        }

        private void AddPartsLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (quoteItem.Parts != 0)
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, _itemNo++);
                reportItem.LineDescription = string.Format("Parts for {0}, Serial No: {1}", GetInstrumentDescription(quoteItem.JobItem.Instrument), quoteItem.JobItem.SerialNo);
                reportItem.Price = quoteItem.Parts;
                result.Add(reportItem);
                _partsTotal += quoteItem.Parts;
            }
        }

        private void AddCarriageLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (quoteItem.Carriage != 0)
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, _itemNo++);
                reportItem.LineDescription = string.Format("Carriage for {0}, Serial No: {1}", GetInstrumentDescription(quoteItem.JobItem.Instrument), quoteItem.JobItem.SerialNo);
                reportItem.Price = quoteItem.Carriage;
                result.Add(reportItem);
                _carriageTotal += quoteItem.Carriage;
            }
        }

        private void AddInvestigationLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (quoteItem.Investigation != 0)
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, _itemNo++);
                reportItem.LineDescription = string.Format("Investigation for {0}, Serial No: {1}", GetInstrumentDescription(quoteItem.JobItem.Instrument), quoteItem.JobItem.SerialNo);
                reportItem.Price = quoteItem.Investigation;
                result.Add(reportItem);
                _investigationTotal += quoteItem.Investigation;
            }
        }

        private void AddReportLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            if (!string.IsNullOrEmpty(quoteItem.Report))
            {
                var reportItem = new QuoteReportModel2();
                GetInvariantDetails(reportItem, quote, quoteItem, 0);
                reportItem.LineDescription = string.Format("Report: {0}", quoteItem.Report);
                reportItem.Price = 0;
                result.Add(reportItem);
            }
        }

        private void AddDeliveryLineItem(List<QuoteReportModel2> result, Quote quote, QuoteItem quoteItem)
        {
            var reportItem = new QuoteReportModel2();
            GetInvariantDetails(reportItem, quote, quoteItem, 0);
            reportItem.LineDescription = string.Format("Delivery: {0} days from go ahead", quoteItem.Days);
            reportItem.Price = 0;
            result.Add(reportItem);
        }

        private void GetInvariantDetails(QuoteReportModel2 reportItem, Quote quote, QuoteItem quoteItem, int itemNo)
        {
            PopulateCompanyDetails(reportItem);
            GetQuoteDetails(reportItem, quote);
            GetCustomerDetails(reportItem, quote.Customer);
            reportItem.JobNo = quoteItem.JobItem.Job.JobNo;
            reportItem.ItemNo = itemNo == 0 ? "0" : string.Format("{0}.{1}", quoteItem.JobItem.ItemNo, itemNo.ToString());
            reportItem.OrderEmailLabel =
                string.Format("If orders are being sent by email, please use the following address: {0}", reportItem.CompanyEmail);
        }

        private void GetQuoteDetails(QuoteReportModel2 reportItem, Quote quote)
        {
            reportItem.QuoteNo = quote.QuoteNumber;
            reportItem.OrderNo = quote.OrderNumber;
            reportItem.AdviceNo = quote.AdviceNumber;
        }

        private void GetCustomerDetails(QuoteReportModel2 reportItem, Customer customer)
        {
            reportItem.Contact = customer.Contact1;
            reportItem.CustomerName = customer.Name;
            reportItem.AssetLine = customer.AssetLine;
            reportItem.CustomerAddress1 = !string.IsNullOrEmpty(customer.Address1) ? customer.Address1 : string.Empty;
            reportItem.CustomerAddress2 = !string.IsNullOrEmpty(customer.Address2) ? customer.Address2 : string.Empty;
            reportItem.CustomerAddress3 = !string.IsNullOrEmpty(customer.Address3) ? customer.Address3 : string.Empty;
            reportItem.CustomerAddress4 = !string.IsNullOrEmpty(customer.Address4) ? customer.Address4 : string.Empty;
            reportItem.CustomerAddress5 = !string.IsNullOrEmpty(customer.Address5) ? customer.Address5 : string.Empty;
        }
    }
}