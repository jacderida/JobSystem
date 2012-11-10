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

		protected void PopulateCompanyDetails(ReportModelBase reportModel)
		{
			var companyDetails = CurrentSession.Query<CompanyDetails>().First();
			reportModel.CompanyName = companyDetails.Name;
			reportModel.CompanyAddress1 = !String.IsNullOrEmpty(companyDetails.Address1) ? companyDetails.Address1 : String.Empty;
			reportModel.CompanyAddress2 = !String.IsNullOrEmpty(companyDetails.Address2) ? companyDetails.Address2 : String.Empty;
			reportModel.CompanyAddress3 = !String.IsNullOrEmpty(companyDetails.Address3) ? companyDetails.Address3 : String.Empty;
			reportModel.CompanyAddress4 = !String.IsNullOrEmpty(companyDetails.Address4) ? companyDetails.Address4 : String.Empty;
			reportModel.CompanyAddress5 = !String.IsNullOrEmpty(companyDetails.Address5) ? companyDetails.Address5 : String.Empty;
			reportModel.CompanyAddress = AssignFullCompanyAddress(reportModel);
			reportModel.CompanyTelephone = !String.IsNullOrEmpty(companyDetails.Telephone) ? companyDetails.Telephone : String.Empty;
			reportModel.CompanyFax = !String.IsNullOrEmpty(companyDetails.Fax) ? companyDetails.Fax : String.Empty;
			reportModel.CompanyRegNo = !String.IsNullOrEmpty(companyDetails.RegNo) ? companyDetails.RegNo : String.Empty;
			reportModel.CompanyVatRegNo = !String.IsNullOrEmpty(companyDetails.VatRegNo) ? String.Format("VAT No: {0}", companyDetails.VatRegNo) : String.Empty;
			reportModel.CompanyEmail = !String.IsNullOrEmpty(companyDetails.Email) ? companyDetails.Email : String.Empty;
			reportModel.CompanyWww = !String.IsNullOrEmpty(companyDetails.Www) ? companyDetails.Www : String.Empty;
			reportModel.CompanyContactInfo = AssignCombinedContactInfo(reportModel);
			reportModel.CompanyTermsAndConditions = !String.IsNullOrEmpty(companyDetails.TermsAndConditions) ? companyDetails.TermsAndConditions : String.Empty;
		}

		public string AssignFullCompanyAddress(ReportModelBase reportModel)
		{
			return String.Format(
				"{0}, {1}, {2}, {3}, {4}",
				reportModel.CompanyAddress1,
				reportModel.CompanyAddress2,
				reportModel.CompanyAddress3,
				reportModel.CompanyAddress4,
				reportModel.CompanyAddress5);
		}

		public string AssignCombinedContactInfo(ReportModelBase reportModel)
		{
			return String.Format(
				"T: {0}, F: {1}, W: {2}",
				reportModel.CompanyTelephone, reportModel.CompanyFax, reportModel.CompanyWww);
		}

		protected string GetInstrumentDescription(Instrument instrument)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}, ", instrument.Manufacturer);
			sb.AppendFormat("{0}, ", instrument.ModelNo);
			if (!String.IsNullOrEmpty(instrument.Range) || instrument.Range.Trim() != "Not Specified")
				sb.AppendFormat("{0}, ", instrument.Range);
			if (!String.IsNullOrEmpty(instrument.Description) || instrument.Description.Trim() != "Not Specified")
				sb.AppendFormat("{0}, ", instrument.Description);
			return sb.ToString().Trim(", ".ToCharArray());
		}

		protected string GetJobItemReference(JobItem jobItem)
		{
			if (jobItem == null)
				return String.Empty;
			return String.Format("{0}/{1}", jobItem.Job.JobNo, jobItem.ItemNo);
		}
	}
}