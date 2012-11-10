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
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;
			var invoiceService = DependencyResolver.Current.GetService<InvoiceService>();
			var invoice = invoiceService.GetById(Model);
			switch (invoice.Currency.Name)
			{
				case "GBP":
					report.ItemSubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.VatTotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
					break;
				case "USD":
					report.ItemSubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.VatTotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
					break;
				case "EUR":
					report.ItemSubTotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.SubTotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.VatTotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					report.TotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
					break;
				default:
					report.ItemSubTotalTextBox.Format = "{0:N2}";
					report.SubTotalTextBox.Format = "{0:N2}";
					report.VatTotalTextBox.Format = "{0:N2}";
					report.TotalTextBox.Format = "{0:N2}";
					break;
			}			
			var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
			instanceReportSource.ReportDocument = report;
			ReportViewer1.ReportSource = instanceReportSource;
		}
	</script>
</body>
</html>