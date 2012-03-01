<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Guid>" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.0.12.215, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
	Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Consignment Note</title>
</head>
<body>
	<form clientidmode="Static" id="frep" runat="server">
		<telerik:ReportViewer ID="ReportViewer1" runat="server" Report="JobSystem.Reporting.ReportDefinitions.TelerikConsignmentReport, JobSystem.Reporting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
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
			dataSource.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateConsignmentReportDataProvider);
			dataSource.DataMember = "GetConsignmentReportData";
			dataSource.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("consignmentId", typeof(Guid), Model));
			var report = new JobSystem.Reporting.ReportDefinitions.TelerikConsignmentReport();
			report.DataSource = dataSource;
			ReportViewer1.Report = report;
			base.OnLoad(e);
		}
	</script>
</body>
</html>
