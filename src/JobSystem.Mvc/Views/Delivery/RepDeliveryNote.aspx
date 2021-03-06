﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Guid>" %>
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
	<title>Delivery Note</title>
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
			dataSource.DataSource = typeof(NHibernateDeliveryReportDataProvider);
			dataSource.DataMember = "GetReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("itemId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikDeliveryReport();
			var logo = companyDetailsService.GetCompanyLogo();
			var pageWidth = report.Width.Value;
			var company = companyDetailsService.GetCompany();
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(company.DefaultCultureCode);
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;

			var deliveryService = DependencyResolver.Current.GetService<DeliveryService>();
			var delivery = deliveryService.GetById(Model);
			report.DocumentName = delivery.DeliveryNoteNumber;
			var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
			instanceReportSource.ReportDocument = report;
			ReportViewer1.ReportSource = instanceReportSource;
		}
	</script>
</body>
</html>