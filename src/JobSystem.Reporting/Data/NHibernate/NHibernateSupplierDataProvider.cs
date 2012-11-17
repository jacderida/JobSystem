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