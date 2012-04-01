using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using JobSystem.DataAccess.NHibernate;
using JobSystem.Reporting.Models;
using JobSystem.DataModel.Entities;

namespace JobSystem.Reporting.Data.NHibernate
{
	public abstract class NHibernateReportDataProviderBase
	{
		public ISession CurrentSession
		{
			get { return NHibernateSession.Current; }
		}

		public void PopulateCompanyDetails(ReportModelBase reportModel)
		{
			var companyDetails = CurrentSession.Query<CompanyDetails>().First();
			reportModel.CompanyName = companyDetails.Name;
			reportModel.CompanyAddress1 = !String.IsNullOrEmpty(companyDetails.Address1) ? companyDetails.Address1 : String.Empty;
			reportModel.CompanyAddress2 = !String.IsNullOrEmpty(companyDetails.Address2) ? companyDetails.Address2 : String.Empty;
			reportModel.CompanyAddress3 = !String.IsNullOrEmpty(companyDetails.Address3) ? companyDetails.Address3 : String.Empty;
			reportModel.CompanyAddress4 = !String.IsNullOrEmpty(companyDetails.Address4) ? companyDetails.Address4 : String.Empty;
			reportModel.CompanyAddress5 = !String.IsNullOrEmpty(companyDetails.Address5) ? companyDetails.Address5 : String.Empty;
			reportModel.CompanyTelephone = !String.IsNullOrEmpty(companyDetails.Telephone) ? companyDetails.Telephone : String.Empty;
			reportModel.CompanyFax = !String.IsNullOrEmpty(companyDetails.Fax) ? companyDetails.Fax : String.Empty;
			reportModel.CompanyRegNo = !String.IsNullOrEmpty(companyDetails.RegNo) ? companyDetails.RegNo : String.Empty;
			reportModel.CompanyVatRegNo = !String.IsNullOrEmpty(companyDetails.VatRegNo) ? companyDetails.VatRegNo : String.Empty;
			reportModel.CompanyEmail = !String.IsNullOrEmpty(companyDetails.Email) ? companyDetails.Email : String.Empty;
			reportModel.CompanyWww = !String.IsNullOrEmpty(companyDetails.Www) ? companyDetails.Www : String.Empty;
			reportModel.CompanyTermsAndConditions = !String.IsNullOrEmpty(companyDetails.TermsAndConditions) ? companyDetails.TermsAndConditions : String.Empty;
		}

		public string GetInstrumentDescription(Instrument instrument)
		{
			return String.Format("{0}, {1}, {2}, {3}",
				instrument.Manufacturer, instrument.ModelNo,
				!String.IsNullOrEmpty(instrument.Range) ? instrument.Range : String.Empty,
				!String.IsNullOrEmpty(instrument.Description) ? instrument.Description : String.Empty);
		}

		public string GetJobItemReference(JobItem jobItem)
		{
			return String.Format("{0}/{1}", jobItem.Job.JobNo, jobItem.ItemNo);
		}
	}
}