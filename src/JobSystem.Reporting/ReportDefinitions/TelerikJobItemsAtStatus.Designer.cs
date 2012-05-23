namespace JobSystem.Reporting.ReportDefinitions
{
	partial class TelerikJobItemsAtStatus
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
			this.ReportDataSource = new Telerik.Reporting.ObjectDataSource();
			this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
			this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.labelsGroup = new Telerik.Reporting.Group();
			this.customerNameCaptionTextBox = new Telerik.Reporting.TextBox();
			this.jobRefCaptionTextBox = new Telerik.Reporting.TextBox();
			this.customerNameGroupHeader = new Telerik.Reporting.GroupHeaderSection();
			this.customerNameGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.customerNameGroup = new Telerik.Reporting.Group();
			this.jobRefGroupHeader = new Telerik.Reporting.GroupHeaderSection();
			this.jobRefGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.jobRefGroup = new Telerik.Reporting.Group();
			this.customerNameDataTextBox = new Telerik.Reporting.TextBox();
			this.jobRefDataTextBox = new Telerik.Reporting.TextBox();
			this.pageHeader = new Telerik.Reporting.PageHeaderSection();
			this.pageFooter = new Telerik.Reporting.PageFooterSection();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.reportFooter = new Telerik.Reporting.ReportFooterSection();
			this.detail = new Telerik.Reporting.DetailSection();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// ReportDataSource
			// 
			this.ReportDataSource.DataMember = "GetJobItemsAtStatusReportData";
			this.ReportDataSource.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateJobItemsAtStatusReportDataProvider);
			this.ReportDataSource.Name = "ReportDataSource";
			this.ReportDataSource.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("statusId", typeof(System.Guid), null)});
			// 
			// labelsGroupHeader
			// 
			this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.customerNameCaptionTextBox,
            this.jobRefCaptionTextBox});
			this.labelsGroupHeader.Name = "labelsGroupHeader";
			this.labelsGroupHeader.PrintOnEveryPage = true;
			// 
			// labelsGroupFooter
			// 
			this.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.labelsGroupFooter.Name = "labelsGroupFooter";
			this.labelsGroupFooter.Style.Visible = false;
			// 
			// labelsGroup
			// 
			this.labelsGroup.GroupFooter = this.labelsGroupFooter;
			this.labelsGroup.GroupHeader = this.labelsGroupHeader;
			this.labelsGroup.Name = "labelsGroup";
			// 
			// customerNameCaptionTextBox
			// 
			this.customerNameCaptionTextBox.CanGrow = true;
			this.customerNameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.customerNameCaptionTextBox.Name = "customerNameCaptionTextBox";
			this.customerNameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.customerNameCaptionTextBox.StyleName = "Caption";
			this.customerNameCaptionTextBox.Value = "Customer Name";
			// 
			// jobRefCaptionTextBox
			// 
			this.jobRefCaptionTextBox.CanGrow = true;
			this.jobRefCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.jobRefCaptionTextBox.Name = "jobRefCaptionTextBox";
			this.jobRefCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.jobRefCaptionTextBox.StyleName = "Caption";
			this.jobRefCaptionTextBox.Value = "Job Ref";
			// 
			// customerNameGroupHeader
			// 
			this.customerNameGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.customerNameGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.customerNameDataTextBox});
			this.customerNameGroupHeader.Name = "customerNameGroupHeader";
			// 
			// customerNameGroupFooter
			// 
			this.customerNameGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.customerNameGroupFooter.Name = "customerNameGroupFooter";
			// 
			// customerNameGroup
			// 
			this.customerNameGroup.GroupFooter = this.customerNameGroupFooter;
			this.customerNameGroup.GroupHeader = this.customerNameGroupHeader;
			this.customerNameGroup.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.CustomerName")});
			this.customerNameGroup.Name = "customerNameGroup";
			// 
			// jobRefGroupHeader
			// 
			this.jobRefGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.jobRefGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.jobRefDataTextBox});
			this.jobRefGroupHeader.Name = "jobRefGroupHeader";
			// 
			// jobRefGroupFooter
			// 
			this.jobRefGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.jobRefGroupFooter.Name = "jobRefGroupFooter";
			// 
			// jobRefGroup
			// 
			this.jobRefGroup.GroupFooter = this.jobRefGroupFooter;
			this.jobRefGroup.GroupHeader = this.jobRefGroupHeader;
			this.jobRefGroup.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.JobRef")});
			this.jobRefGroup.Name = "jobRefGroup";
			// 
			// customerNameDataTextBox
			// 
			this.customerNameDataTextBox.CanGrow = true;
			this.customerNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.customerNameDataTextBox.Name = "customerNameDataTextBox";
			this.customerNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.customerNameDataTextBox.StyleName = "Data";
			this.customerNameDataTextBox.Value = "=Fields.CustomerName";
			// 
			// jobRefDataTextBox
			// 
			this.jobRefDataTextBox.CanGrow = true;
			this.jobRefDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.jobRefDataTextBox.Name = "jobRefDataTextBox";
			this.jobRefDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.jobRefDataTextBox.StyleName = "Data";
			this.jobRefDataTextBox.Value = "=Fields.JobRef";
			// 
			// pageHeader
			// 
			this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.pageHeader.Name = "pageHeader";
			// 
			// pageFooter
			// 
			this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
			this.pageFooter.Name = "pageFooter";
			// 
			// pageInfoTextBox
			// 
			this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.pageInfoTextBox.Name = "pageInfoTextBox";
			this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.pageInfoTextBox.StyleName = "PageInfo";
			this.pageInfoTextBox.Value = "=PageNumber";
			// 
			// reportHeader
			// 
			this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D);
			this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
			this.reportHeader.Name = "reportHeader";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D), Telerik.Reporting.Drawing.Unit.Cm(0.98562496900558472D));
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "Job Items";
			// 
			// reportFooter
			// 
			this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.reportFooter.Name = "reportFooter";
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.detail.Name = "detail";
			// 
			// TelerikJobItemsAtStatus
			// 
			this.DataSource = this.ReportDataSource;
			this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup,
            this.customerNameGroup,
            this.jobRefGroup});
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.customerNameGroupHeader,
            this.customerNameGroupFooter,
            this.jobRefGroupHeader,
            this.jobRefGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
			this.Name = "TelerikJobItemsAtStatus";
			this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			this.Sortings.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.CustomerName", Telerik.Reporting.Data.SortDirection.Asc),
            new Telerik.Reporting.Data.Sorting("=Fields.CustomerName", Telerik.Reporting.Data.SortDirection.Asc)});
			this.Style.BackgroundColor = System.Drawing.Color.White;
			styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
			styleRule1.Style.Color = System.Drawing.Color.Black;
			styleRule1.Style.Font.Bold = true;
			styleRule1.Style.Font.Italic = false;
			styleRule1.Style.Font.Name = "Tahoma";
			styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(20D);
			styleRule1.Style.Font.Strikeout = false;
			styleRule1.Style.Font.Underline = false;
			styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
			styleRule2.Style.Color = System.Drawing.Color.Black;
			styleRule2.Style.Font.Name = "Tahoma";
			styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
			styleRule3.Style.Font.Name = "Tahoma";
			styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
			styleRule4.Style.Font.Name = "Tahoma";
			styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
			this.Width = Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.ObjectDataSource ReportDataSource;
		private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
		private Telerik.Reporting.TextBox customerNameCaptionTextBox;
		private Telerik.Reporting.TextBox jobRefCaptionTextBox;
		private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
		private Telerik.Reporting.Group labelsGroup;
		private Telerik.Reporting.GroupHeaderSection customerNameGroupHeader;
		private Telerik.Reporting.TextBox customerNameDataTextBox;
		private Telerik.Reporting.GroupFooterSection customerNameGroupFooter;
		private Telerik.Reporting.Group customerNameGroup;
		private Telerik.Reporting.GroupHeaderSection jobRefGroupHeader;
		private Telerik.Reporting.TextBox jobRefDataTextBox;
		private Telerik.Reporting.GroupFooterSection jobRefGroupFooter;
		private Telerik.Reporting.Group jobRefGroup;
		private Telerik.Reporting.PageHeaderSection pageHeader;
		private Telerik.Reporting.PageFooterSection pageFooter;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.ReportHeaderSection reportHeader;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.ReportFooterSection reportFooter;
		private Telerik.Reporting.DetailSection detail;

	}
}