using System;
using JobSystem.DataModel.Entities;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data.NHibernate
{
	public abstract class NHibernateCustomerDataProviderBase : NHibernateReportDataProviderBase
	{
		public void PopulateCustomerInfo(Customer customer, CustomerReportModel reportModel)
		{
			reportModel.CustomerName = customer.Name;
			var addressLines = new string[5];
			addressLines[0] = customer.Address1.Trim();
			addressLines[1] = customer.Address2.Trim();
			addressLines[2] = customer.Address3.Trim();
			addressLines[3] = customer.Address4.Trim();
			addressLines[4] = customer.Address5.Trim();
			var i = 0;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.CustomerAddress1 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.CustomerAddress2 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.CustomerAddress3 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.CustomerAddress4 = i < addressLines.Length ? addressLines[i++] : String.Empty;
			while (i < addressLines.Length - 1 && String.IsNullOrEmpty(addressLines[i]))
				i++;
			reportModel.CustomerAddress5 = i < addressLines.Length ? addressLines[i++] : String.Empty;
		}
	}
}