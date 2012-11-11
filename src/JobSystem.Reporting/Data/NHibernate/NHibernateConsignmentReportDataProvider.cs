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
				reportItem.ConsignmentNo = consignment.ConsignmentNo;
				reportItem.SupplierName = supplier.Name;
				reportItem.SupplierAddress1 = !String.IsNullOrEmpty(supplier.Address1) ? supplier.Address1 : String.Empty;
				reportItem.SupplierAddress2 = !String.IsNullOrEmpty(supplier.Address2) ? supplier.Address2 : String.Empty;
				reportItem.SupplierAddress3 = !String.IsNullOrEmpty(supplier.Address3) ? supplier.Address3 : String.Empty;
				reportItem.SupplierAddress4 = !String.IsNullOrEmpty(supplier.Address4) ? supplier.Address4 : String.Empty;
				reportItem.SupplierAddress5 = !String.IsNullOrEmpty(supplier.Address5) ? supplier.Address5 : String.Empty;
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
	}
}