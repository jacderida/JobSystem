<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Guid>" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.2.12.1017, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Reporting, Version=6.2.12.1017, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.Reporting" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="JobSystem.BusinessLogic.Services" %>
<%@ Import Namespace="JobSystem.Reporting.Data.NHibernate" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<title>Quote Note</title>
</head>
<body>
	<form id="main" method="post" action="">
		<telerik:ReportViewer ID="ReportViewer1" Width="100%" Height="800px" runat="server">
		</telerik:ReportViewer>
	</form>
	<script runat="server">
		public override void VerifyRenderingInServerForm(Control control)
		{
			//To avoid the server form (<form runat="server">) requirement
			//base.VerifyRenderingInServerForm(control);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			var dataSource = new Telerik.Reporting.ObjectDataSource();
			dataSource.DataSource = typeof(NHibernateQuoteReportDataProvider);
			dataSource.DataMember = "GetQuoteReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("quoteId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikQuoteReport();
			var logo = companyDetailsService.GetCompanyLogo();
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;
			var company = companyDetailsService.GetCompany();
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(company.DefaultCultureCode);
			var quoteService = DependencyResolver.Current.GetService<QuoteService>();
			var quote = quoteService.GetById(Model);
			switch (quote.Currency.Name)
			{
				case "GBP":
					{
						report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.RepairTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.PartsTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						break;
					}
				case "USD":
					{
						report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.RepairTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.PartsTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						break;
					}
				case "EUR":
					{
						report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.RepairTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.PartsTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						break;
					}
				default:
					{
						report.CalibrationTextBox.Format = "{0:N2}";
						report.RepairTextBox.Format = "{0:N2}";
						report.PartsTextBox.Format = "{0:N2}";
						report.CarriageTextBox.Format = "{0:N2}";
						report.SubTotalTextBox.Format = "{0:N2}";
						report.InvestigationTextBox.Format = "{0:N2}";
						report.TotalTextBox.Format = "{0:N2}";
						break;
					}
			}

			report.DocumentName = quote.QuoteNumber;
			var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
			instanceReportSource.ReportDocument = report;
			ReportViewer1.ReportSource = instanceReportSource;
		}
	</script>
</body>
</html>