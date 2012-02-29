<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=6.0.12.215, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Consignment Note</title>
</head>
<body>
    <form clientidmode="Static" id="frep" runat="server">

	<telerik:ReportViewer ID="ReportViewer1" runat="server" 
		Report="JobSystem.Mvc.Reports.ConsignmentNote, JobSystem.Mvc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
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
		//Bind the report viewer
		base.OnLoad(e);
	}
</script>

</body>
</html>
