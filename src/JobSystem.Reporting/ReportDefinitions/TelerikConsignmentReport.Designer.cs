namespace JobSystem.Reporting.ReportDefinitions
{
	partial class TelerikConsignmentReport
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
			this.pageHeader = new Telerik.Reporting.PageHeaderSection();
			this.pageFooter = new Telerik.Reporting.PageFooterSection();
			this.currentTimeTextBox = new Telerik.Reporting.TextBox();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.consignmentNoDataTextBox = new Telerik.Reporting.TextBox();
			this.supplierNameDataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress1DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress2DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress3DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress4DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress5DataTextBox = new Telerik.Reporting.TextBox();
			this.raisedByDataTextBox = new Telerik.Reporting.TextBox();
			this.dateCreatedDataTextBox = new Telerik.Reporting.TextBox();
			this.textBox1 = new Telerik.Reporting.TextBox();
			this.textBox2 = new Telerik.Reporting.TextBox();
			this.textBox3 = new Telerik.Reporting.TextBox();
			this.textBox4 = new Telerik.Reporting.TextBox();
			this.textBox5 = new Telerik.Reporting.TextBox();
			this.textBox6 = new Telerik.Reporting.TextBox();
			this.reportFooter = new Telerik.Reporting.ReportFooterSection();
			this.detail = new Telerik.Reporting.DetailSection();
			this.jobRefDataTextBox1 = new Telerik.Reporting.TextBox();
			this.descriptionDataTextBox = new Telerik.Reporting.TextBox();
			this.instructionsDataTextBox = new Telerik.Reporting.TextBox();
			this.MainLogo = new Telerik.Reporting.PictureBox();
			this.ReportData = new Telerik.Reporting.ObjectDataSource();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
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
            this.currentTimeTextBox,
            this.pageInfoTextBox});
			this.pageFooter.Name = "pageFooter";
			// 
			// currentTimeTextBox
			// 
			this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.currentTimeTextBox.Name = "currentTimeTextBox";
			this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.currentTimeTextBox.StyleName = "PageInfo";
			this.currentTimeTextBox.Value = "=NOW()";
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
			this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(6.7856249809265137D);
			this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.consignmentNoDataTextBox,
            this.supplierNameDataTextBox,
            this.supplierAddress1DataTextBox,
            this.supplierAddress2DataTextBox,
            this.supplierAddress3DataTextBox,
            this.supplierAddress4DataTextBox,
            this.supplierAddress5DataTextBox,
            this.raisedByDataTextBox,
            this.dateCreatedDataTextBox,
            this.textBox1,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.MainLogo});
			this.reportHeader.Name = "reportHeader";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2537503242492676D), Telerik.Reporting.Drawing.Unit.Cm(0.88562500476837158D));
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "Consignment";
			// 
			// consignmentNoDataTextBox
			// 
			this.consignmentNoDataTextBox.CanGrow = true;
			this.consignmentNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052914243191480637D), Telerik.Reporting.Drawing.Unit.Cm(0.88582503795623779D));
			this.consignmentNoDataTextBox.Name = "consignmentNoDataTextBox";
			this.consignmentNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5933332443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.consignmentNoDataTextBox.StyleName = "Data";
			this.consignmentNoDataTextBox.Value = "=Fields.ConsignmentNo";
			// 
			// supplierNameDataTextBox
			// 
			this.supplierNameDataTextBox.CanGrow = true;
			this.supplierNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(1.7856249809265137D));
			this.supplierNameDataTextBox.Name = "supplierNameDataTextBox";
			this.supplierNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612503051757812D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierNameDataTextBox.StyleName = "Data";
			this.supplierNameDataTextBox.Value = "=Fields.SupplierName";
			// 
			// supplierAddress1DataTextBox
			// 
			this.supplierAddress1DataTextBox.CanGrow = true;
			this.supplierAddress1DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.38582444190979D));
			this.supplierAddress1DataTextBox.Name = "supplierAddress1DataTextBox";
			this.supplierAddress1DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612493515014648D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierAddress1DataTextBox.StyleName = "Data";
			this.supplierAddress1DataTextBox.Value = "=Fields.SupplierAddress1";
			// 
			// supplierAddress2DataTextBox
			// 
			this.supplierAddress2DataTextBox.CanGrow = true;
			this.supplierAddress2DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.9860243797302246D));
			this.supplierAddress2DataTextBox.Name = "supplierAddress2DataTextBox";
			this.supplierAddress2DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612493515014648D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierAddress2DataTextBox.StyleName = "Data";
			this.supplierAddress2DataTextBox.Value = "=Fields.SupplierAddress2";
			// 
			// supplierAddress3DataTextBox
			// 
			this.supplierAddress3DataTextBox.CanGrow = true;
			this.supplierAddress3DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052915856242179871D), Telerik.Reporting.Drawing.Unit.Cm(3.5862252712249756D));
			this.supplierAddress3DataTextBox.Name = "supplierAddress3DataTextBox";
			this.supplierAddress3DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.461249828338623D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierAddress3DataTextBox.StyleName = "Data";
			this.supplierAddress3DataTextBox.Value = "=Fields.SupplierAddress3";
			// 
			// supplierAddress4DataTextBox
			// 
			this.supplierAddress4DataTextBox.CanGrow = true;
			this.supplierAddress4DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0529174730181694D), Telerik.Reporting.Drawing.Unit.Cm(4.1864256858825684D));
			this.supplierAddress4DataTextBox.Name = "supplierAddress4DataTextBox";
			this.supplierAddress4DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612493515014648D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierAddress4DataTextBox.StyleName = "Data";
			this.supplierAddress4DataTextBox.Value = "=Fields.SupplierAddress4";
			// 
			// supplierAddress5DataTextBox
			// 
			this.supplierAddress5DataTextBox.CanGrow = true;
			this.supplierAddress5DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052915856242179871D), Telerik.Reporting.Drawing.Unit.Cm(4.7866253852844238D));
			this.supplierAddress5DataTextBox.Name = "supplierAddress5DataTextBox";
			this.supplierAddress5DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612493515014648D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.supplierAddress5DataTextBox.StyleName = "Data";
			this.supplierAddress5DataTextBox.Value = "=Fields.SupplierAddress5";
			// 
			// raisedByDataTextBox
			// 
			this.raisedByDataTextBox.CanGrow = true;
			this.raisedByDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052914243191480637D), Telerik.Reporting.Drawing.Unit.Cm(5.3868260383605957D));
			this.raisedByDataTextBox.Name = "raisedByDataTextBox";
			this.raisedByDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612507820129395D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.raisedByDataTextBox.StyleName = "Data";
			this.raisedByDataTextBox.Value = "=Fields.RaisedBy";
			// 
			// dateCreatedDataTextBox
			// 
			this.dateCreatedDataTextBox.CanGrow = true;
			this.dateCreatedDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0529150515794754D), Telerik.Reporting.Drawing.Unit.Cm(5.9870271682739258D));
			this.dateCreatedDataTextBox.Name = "dateCreatedDataTextBox";
			this.dateCreatedDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4612493515014648D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.dateCreatedDataTextBox.StyleName = "Data";
			this.dateCreatedDataTextBox.Value = "=Fields.DateCreated";
			// 
			// textBox1
			// 
			this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559782981872559D), Telerik.Reporting.Drawing.Unit.Cm(2.6002001762390137D));
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008328437805176D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
			this.textBox1.Value = "=Fields.CompanyName";
			// 
			// textBox2
			// 
			this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559783935546875D), Telerik.Reporting.Drawing.Unit.Cm(3.2004008293151855D));
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008328437805176D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
			this.textBox2.Value = "=Fields.CompanyAddress1";
			// 
			// textBox3
			// 
			this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559783935546875D), Telerik.Reporting.Drawing.Unit.Cm(3.8006017208099365D));
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008323669433594D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox3.Value = "=Fields.CompanyAddress2";
			// 
			// textBox4
			// 
			this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559782981872559D), Telerik.Reporting.Drawing.Unit.Cm(4.4008016586303711D));
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.200833797454834D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox4.Value = "=Fields.CompanyAddress3";
			// 
			// textBox5
			// 
			this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559783935546875D), Telerik.Reporting.Drawing.Unit.Cm(5.0010013580322266D));
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008323669433594D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
			this.textBox5.Value = "=Fields.CompanyAddress4";
			// 
			// textBox6
			// 
			this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.559783935546875D), Telerik.Reporting.Drawing.Unit.Cm(5.6012020111083984D));
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.200833797454834D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox6.Value = "=Fields.CompanyAddress5";
			// 
			// reportFooter
			// 
			this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.reportFooter.Name = "reportFooter";
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.jobRefDataTextBox1,
            this.descriptionDataTextBox,
            this.instructionsDataTextBox});
			this.detail.Name = "detail";
			// 
			// jobRefDataTextBox1
			// 
			this.jobRefDataTextBox1.CanGrow = true;
			this.jobRefDataTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.jobRefDataTextBox1.Name = "jobRefDataTextBox1";
			this.jobRefDataTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.jobRefDataTextBox1.StyleName = "Data";
			this.jobRefDataTextBox1.Value = "=Fields.JobRef";
			// 
			// descriptionDataTextBox
			// 
			this.descriptionDataTextBox.CanGrow = true;
			this.descriptionDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3066668510437012D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.descriptionDataTextBox.Name = "descriptionDataTextBox";
			this.descriptionDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.descriptionDataTextBox.StyleName = "Data";
			this.descriptionDataTextBox.Value = "=Fields.Description";
			// 
			// instructionsDataTextBox
			// 
			this.instructionsDataTextBox.CanGrow = true;
			this.instructionsDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.560417175292969D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.instructionsDataTextBox.Name = "instructionsDataTextBox";
			this.instructionsDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.instructionsDataTextBox.StyleName = "Data";
			this.instructionsDataTextBox.Value = "=Fields.Instructions";
			// 
			// MainLogo
			// 
			this.MainLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(497.40542602539062D), Telerik.Reporting.Drawing.Unit.Pixel(0.0037841796875D));
			this.MainLogo.Name = "MainLogo";
			this.MainLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(98.2677001953125D), Telerik.Reporting.Drawing.Unit.Pixel(98.267715454101562D));
			// 
			// ReportData
			// 
			this.ReportData.DataMember = "GetConsignmentReportData";
			this.ReportData.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateConsignmentReportDataProvider);
			this.ReportData.Name = "ReportData";
			this.ReportData.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("consignmentId", typeof(System.Guid), null)});
			// 
			// TelerikConsignmentReport
			// 
			this.DataSource = this.ReportData;
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
			this.Name = "TelerikConsignmentReport";
			this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
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
			this.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Pixel;
			this.Width = Telerik.Reporting.Drawing.Unit.Pixel(595.6768798828125D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.ObjectDataSource ReportData;
		private Telerik.Reporting.PageHeaderSection pageHeader;
		private Telerik.Reporting.PageFooterSection pageFooter;
		private Telerik.Reporting.TextBox currentTimeTextBox;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.ReportHeaderSection reportHeader;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.TextBox consignmentNoDataTextBox;
		private Telerik.Reporting.TextBox supplierNameDataTextBox;
		private Telerik.Reporting.TextBox supplierAddress1DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress2DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress3DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress4DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress5DataTextBox;
		private Telerik.Reporting.TextBox raisedByDataTextBox;
		private Telerik.Reporting.TextBox dateCreatedDataTextBox;
		private Telerik.Reporting.ReportFooterSection reportFooter;
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.TextBox jobRefDataTextBox1;
		private Telerik.Reporting.TextBox descriptionDataTextBox;
		private Telerik.Reporting.TextBox instructionsDataTextBox;
		private Telerik.Reporting.TextBox textBox1;
		private Telerik.Reporting.TextBox textBox2;
		private Telerik.Reporting.TextBox textBox3;
		private Telerik.Reporting.TextBox textBox4;
		private Telerik.Reporting.TextBox textBox5;
		private Telerik.Reporting.TextBox textBox6;
		public Telerik.Reporting.PictureBox MainLogo;

	}
}