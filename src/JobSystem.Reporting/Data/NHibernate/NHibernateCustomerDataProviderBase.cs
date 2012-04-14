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
			reportModel.CustomerAddress1 = !String.IsNullOrEmpty(customer.Address1) ? customer.Address1 : String.Empty;
			reportModel.CustomerAddress2 = !String.IsNullOrEmpty(customer.Address2) ? customer.Address2 : String.Empty;
			reportModel.CustomerAddress3 = !String.IsNullOrEmpty(customer.Address3) ? customer.Address3 : String.Empty;
			reportModel.CustomerAddress4 = !String.IsNullOrEmpty(customer.Address4) ? customer.Address4 : String.Empty;
			reportModel.CustomerAddress5 = !String.IsNullOrEmpty(customer.Address5) ? customer.Address5 : String.Empty;
		}
	}
}