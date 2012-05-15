﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;
using NHibernate.Linq;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateJobItemsAtStatusReportDataProvider : NHibernateReportDataProviderBase, IJobItemsAtStatusReportDataProvider
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<JobItemAtStatusReportModel> GetJobItemsAtStatusReportData(ListItemType status)
		{
			var result = new List<JobItemAtStatusReportModel>();
			var jobItemsNotInvoiced = CurrentSession.Query<JobItem>().Where(ji => ji.Status.Type == status);
			foreach (var jobItem in jobItemsNotInvoiced)
			{
				var reportItem = new JobItemAtStatusReportModel();
				PopulateCompanyDetails(reportItem);
				var job = jobItem.Job;
				reportItem.CustomerName = job.Customer.Name;
				reportItem.OrderNo = job.OrderNo;
				reportItem.AdviceNo = job.AdviceNo;
				reportItem.DateReceived = job.DateCreated;
				reportItem.SerialNo = jobItem.SerialNo;
				reportItem.AssetNo = jobItem.AssetNo;
				reportItem.ItemNo = jobItem.ItemNo;
				reportItem.Status = jobItem.Status.ToString();
				reportItem.EquipmentDescription = jobItem.Instrument.ToString();
				var quoteItem = CurrentSession.Query<QuoteItem>().Where(qi => qi.JobItem.Id == jobItem.Id).SingleOrDefault();
				if (quoteItem != null)
				{
					var quote = quoteItem.Quote;
					if (!String.IsNullOrEmpty(quote.OrderNumber))
						reportItem.OrderNo = quote.QuoteNumber;
					if (!String.IsNullOrEmpty(quote.AdviceNumber))
						reportItem.AdviceNo = quote.AdviceNumber;
				}
				result.Add(reportItem);
			}
			return result;
		}
	}
}