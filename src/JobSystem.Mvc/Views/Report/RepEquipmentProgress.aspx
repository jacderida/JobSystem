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
			Report="JobSystem.Reporting.ReportDefinitions.TelerikEquipmentProgressReport, JobSystem.Reporting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
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
			dataSource.DataSource = typeof(NHibernateEquipmentProgressReportDataProvider);
			dataSource.DataMember = "GetEquipmentProgressReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("customerId", typeof(Guid), Model));
			var companyDetailsService = DependencyResolver.Current.GetService<CompanyDetailsService>();
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikEquipmentProgressReport();
			var logo = companyDetailsService.GetCompanyLogo();
			report.MainLogo.Value = logo;
			report.DataSource = dataSource;	
			ReportViewer1.Report = report;
			base.OnLoad(e);
		}
	</script>
</body>
</html>
