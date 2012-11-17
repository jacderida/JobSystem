using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data.NHibernate
{
	[DataObject]
	public class NHibernateConsignmentReportDataProvider : NHibernateReportDataProviderBase, IConsignmentReportDataProvider
	{
		[DataObjectMethod(DataObjectMethodType.Select)]
		public List<ConsignmentReportModel> GetConsignmentReportData(Guid consignmentId)
		{
			var result = new List<ConsignmentReportModel>();
			var consignment = CurrentSession.Get<Consignment>(consignmentId);
			var supplier = consignment.Supplier;
			foreach (var item in consignment.Items)
			{
				var reportItem = new ConsignmentReportModel();
				PopulateCompanyDetails(reportItem);
				PopulateSupplierInfo(supplier, reportItem);
				reportItem.ConsignmentNo = consignment.ConsignmentNo;
				reportItem.SupplierTel = !String.IsNullOrEmpty(supplier.Telephone) ? supplier.Telephone : String.Empty;
				reportItem.SupplierFax = !String.IsNullOrEmpty(supplier.Fax) ? supplier.Fax : String.Empty;
				reportItem.DateCreated = consignment.DateCreated.ToShortDateString();
				reportItem.RaisedBy = consignment.CreatedBy.Name;
				reportItem.JobRef = String.Format("{0}/{1}", item.JobItem.Job.JobNo, item.JobItem.ItemNo);
				reportItem.Description = GetInstrumentDescription(item.JobItem.Instrument);
				reportItem.Instructions = !String.IsNullOrEmpty(item.Instructions) ? item.Instructions : String.Empty;
				result.Add(reportItem);
			}
			return result.OrderBy(i => i.JobRef).ToList();
		}

		private void PopulateSupplierInfo(Supplier supplier, ConsignmentReportModel reportModel)
		{
			reportModel.SupplierName = supplier.Name;
			var addressLines = new string[5];
			addressLines[0] = supplier.Address1.Trim();
			addressLines[1] = supplier.Address2.Trim();
			addressLines[2] = supplier.Address3.Trim();
			addressLines[3] = supplier.Address4.Trim();
			addressLines[4] = supplier.Address5.Trim();
			var i = 0;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.SupplierAddress1 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.SupplierAddress2 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.SupplierAddress3 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.SupplierAddress4 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.SupplierAddress5 = i < addressLines.Length ? addressLines[i++] : String.Empty;
		}
	}
}