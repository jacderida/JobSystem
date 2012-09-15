<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Guid>" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.1.12.820, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Reporting, Version=6.1.12.820, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.Reporting" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="JobSystem.BusinessLogic.Services" %>
<%@ Import Namespace="JobSystem.Reporting.Data.NHibernate" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<title>Invoice Note</title>
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
			dataSource.DataSource = typeof(NHibernateInvoiceReportDataProvider);
			dataSource.DataMember = "GetReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("itemId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikInvoiceReport();
			var company = companyDetailsService.GetCompany();
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(company.DefaultCultureCode);
			var logo = companyDetailsService.GetCompanyLogo();
			var pageWidth = report.Width.Value;
			report.DataSource = dataSource;
			var invoiceService = DependencyResolver.Current.GetService<InvoiceService>();
			var invoice = invoiceService.GetById(Model);
			switch (invoice.Currency.Name)
			{
				case "GBP":
					report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.RepairTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.PartsTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					break;
				case "USD":
					report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.RepairTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.PartsTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					break;
				case "EUR":
					report.CalibrationTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.RepairTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.PartsTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.CarriageTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.InvestigationTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					break;
				default:
					report.CalibrationTextBox.Format = "{0:N2}";
					report.RepairTextBox.Format = "{0:N2}";
					report.PartsTextBox.Format = "{0:N2}";
					report.CarriageTextBox.Format = "{0:N2}";
					report.SubTotalTextBox.Format = "{0:N2}";
					report.InvestigationTextBox.Format = "{0:N2}";
					report.CurrencyMessageTextBox.Visible = true;
					break;
			}			
			var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
			instanceReportSource.ReportDocument = report;
			ReportViewer1.ReportSource = instanceReportSource;
		}
	</script>
</body>
</html>