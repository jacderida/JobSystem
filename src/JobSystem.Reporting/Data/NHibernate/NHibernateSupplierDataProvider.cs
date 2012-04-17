using System;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data.NHibernate
{
	public abstract class NHibernateSupplierDataProvider : NHibernateReportDataProviderBase
	{
		public void PopulateSupplierInfo(Supplier supplier, SupplierReportModel reportModel)
		{
			reportModel.SupplierName = supplier.Name;
			reportModel.SupplierAddress1 = !String.IsNullOrEmpty(supplier.Address1) ? supplier.Address1 : String.Empty;
			reportModel.SupplierAddress2 = !String.IsNullOrEmpty(supplier.Address2) ? supplier.Address2 : String.Empty;
			reportModel.SupplierAddress3 = !String.IsNullOrEmpty(supplier.Address3) ? supplier.Address3 : String.Empty;
			reportModel.SupplierAddress4 = !String.IsNullOrEmpty(supplier.Address4) ? supplier.Address4 : String.Empty;
			reportModel.SupplierAddress5 = !String.IsNullOrEmpty(supplier.Address5) ? supplier.Address5 : String.Empty;
		}
	}
}