<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Guid>" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.0.12.215, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="JobSystem.BusinessLogic.Services" %>
<%@ Import Namespace="JobSystem.Reporting.Data.NHibernate" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<title>Quote Note</title>
</head>
<body>
	<form clientidmode="Static" id="frep" runat="server">
		<telerik:ReportViewer ID="ReportViewer1" runat="server" 
			Report="JobSystem.Reporting.ReportDefinitions.TelerikQuoteReport, JobSystem.Reporting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
			Height="842px" Width="650px">
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
			var dataSource = new Telerik.Reporting.ObjectDataSource();
			dataSource.DataSource = typeof(NHibernateQuoteReportDataProvider);
			dataSource.DataMember = "GetQuoteReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("quoteId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikQuoteReport();
			var logo = companyDetailsService.GetCompanyLogo();
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;

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
						report.CurrencyMessageTextBox.Visible = true;
						break;
					}
			}
			ReportViewer1.Report = report;
			base.OnLoad(e);
		}
	</script>
</body>
</html>
