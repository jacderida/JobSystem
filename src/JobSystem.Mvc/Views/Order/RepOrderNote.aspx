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
	<title>Order Note</title>
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
			dataSource.DataSource = typeof(NHibernateOrderDataProvider);
			dataSource.DataMember = "GetReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("itemId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikOrderReport();
			var company = companyDetailsService.GetCompany();
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(company.DefaultCultureCode);
			var logo = companyDetailsService.GetCompanyLogo();
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;

			var orderService = DependencyResolver.Current.GetService<OrderService>();
			var order = orderService.GetById(Model);
			switch (order.Currency.Name)
			{
				case "GBP":
					{
						report.PriceTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-GB");
						break;
					}
				case "USD":
					{
						report.PriceTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("en-US");
						break;
					}
				case "EUR":
					{
						report.PriceTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						report.TotalTextBox.Culture = new System.Globalization.CultureInfo("de-DE");
						break;
					}
				default:
					{
						report.PriceTextBox.Format = "{0:N2}";
						report.TotalTextBox.Format = "{0:N2}";
						break;
					}
			}
			var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
			instanceReportSource.ReportDocument = report;
			ReportViewer1.ReportSource = instanceReportSource;
		}
	</script>
</body>
</html>
