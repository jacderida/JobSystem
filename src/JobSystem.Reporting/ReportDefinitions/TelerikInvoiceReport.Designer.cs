namespace JobSystem.Reporting.ReportDefinitions
{
	partial class TelerikInvoiceReport
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
			this.detail = new Telerik.Reporting.DetailSection();
			this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
			this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeaderSection1
			// 
			this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
			this.pageHeaderSection1.Name = "pageHeaderSection1";
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
			this.detail.Name = "detail";
			// 
			// pageFooterSection1
			// 
			this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
			this.pageFooterSection1.Name = "pageFooterSection1";
			// 
			// objectDataSource1
			// 
			this.objectDataSource1.DataMember = "GetReportData";
			this.objectDataSource1.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateInvoiceReportDataProvider);
			this.objectDataSource1.Name = "objectDataSource1";
			this.objectDataSource1.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("itemId", typeof(System.Guid), null)});
			// 
			// TelerikInvoiceReport
			// 
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
			this.Name = "TelerikInvoiceReport";
			this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			this.Style.BackgroundColor = System.Drawing.Color.White;
			this.Width = Telerik.Reporting.Drawing.Unit.Cm(15D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.PageFooterSection pageFooterSection1;
		private Telerik.Reporting.ObjectDataSource objectDataSource1;
	}
}