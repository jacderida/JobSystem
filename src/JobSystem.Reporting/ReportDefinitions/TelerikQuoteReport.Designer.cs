namespace JobSystem.Reporting.ReportDefinitions
{
	partial class TelerikQuoteReport
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
			this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
			this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.labelsGroup = new Telerik.Reporting.Group();
			this.pageHeader = new Telerik.Reporting.PageHeaderSection();
			this.pageFooter = new Telerik.Reporting.PageFooterSection();
			this.currentTimeTextBox = new Telerik.Reporting.TextBox();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
			this.reportFooter = new Telerik.Reporting.ReportFooterSection();
			this.detail = new Telerik.Reporting.DetailSection();
			this.MainLogo = new Telerik.Reporting.PictureBox();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.textBox1 = new Telerik.Reporting.TextBox();
			this.textBox2 = new Telerik.Reporting.TextBox();
			this.textBox3 = new Telerik.Reporting.TextBox();
			this.textBox4 = new Telerik.Reporting.TextBox();
			this.textBox5 = new Telerik.Reporting.TextBox();
			this.textBox6 = new Telerik.Reporting.TextBox();
			this.textBox7 = new Telerik.Reporting.TextBox();
			this.textBox9 = new Telerik.Reporting.TextBox();
			this.textBox10 = new Telerik.Reporting.TextBox();
			this.textBox11 = new Telerik.Reporting.TextBox();
			this.textBox12 = new Telerik.Reporting.TextBox();
			this.textBox13 = new Telerik.Reporting.TextBox();
			this.textBox14 = new Telerik.Reporting.TextBox();
			this.textBox15 = new Telerik.Reporting.TextBox();
			this.textBox16 = new Telerik.Reporting.TextBox();
			this.ReportData = new Telerik.Reporting.ObjectDataSource();
			this.textBox17 = new Telerik.Reporting.TextBox();
			this.textBox8 = new Telerik.Reporting.TextBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// labelsGroupHeader
			// 
			this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
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
			// currentTimeTextBox
			// 
			this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(5.885624885559082D));
			this.currentTimeTextBox.Name = "currentTimeTextBox";
			this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.currentTimeTextBox.Style.Font.Name = "Arial";
			this.currentTimeTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
			this.currentTimeTextBox.StyleName = "PageInfo";
			this.currentTimeTextBox.Value = "=NOW()";
			// 
			// pageInfoTextBox
			// 
			this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.pageInfoTextBox.Name = "pageInfoTextBox";
			this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.pageInfoTextBox.Style.Font.Name = "Arial";
			this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
			this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.pageInfoTextBox.StyleName = "PageInfo";
			this.pageInfoTextBox.Value = "=PageNumber";
			// 
			// reportHeader
			// 
			this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(8.4854249954223633D);
			this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.MainLogo,
            this.titleTextBox,
            this.textBox1,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.currentTimeTextBox,
            this.textBox15,
            this.textBox14,
            this.textBox17,
            this.textBox8});
			this.reportHeader.Name = "reportHeader";
			// 
			// reportFooter
			// 
			this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.reportFooter.Name = "reportFooter";
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(3.0858256816864014D);
			this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox9,
            this.textBox10,
            this.textBox11,
            this.textBox12,
            this.textBox13,
            this.textBox16});
			this.detail.Name = "detail";
			// 
			// MainLogo
			// 
			this.MainLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(415.748046875D), Telerik.Reporting.Drawing.Unit.Pixel(7.0157470703125D));
			this.MainLogo.Name = "MainLogo";
			this.MainLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(179.41732788085938D), Telerik.Reporting.Drawing.Unit.Pixel(111.26771545410156D));
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(0.18562497198581696D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1926026344299316D), Telerik.Reporting.Drawing.Unit.Cm(0.88562500476837158D));
			this.titleTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.titleTextBox.Style.Font.Name = "Arial";
			this.titleTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
			this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "Quotation";
			// 
			// textBox1
			// 
			this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.4001997709274292D), Telerik.Reporting.Drawing.Unit.Cm(4.98562479019165D));
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.346881866455078D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox1.Style.Font.Bold = true;
			this.textBox1.Value = "=Fields.Contact";
			// 
			// textBox2
			// 
			this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(4.98562479019165D));
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.98333281278610229D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox2.Style.Font.Bold = true;
			this.textBox2.Value = "FAO:";
			// 
			// textBox3
			// 
			this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(1.6856250762939453D));
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4833335876464844D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
			this.textBox3.Style.Font.Bold = true;
			this.textBox3.Value = "=Fields.CompanyAddress1";
			// 
			// textBox4
			// 
			this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(2.285825252532959D));
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4833335876464844D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
			this.textBox4.Style.Font.Bold = true;
			this.textBox4.Value = "=Fields.CompanyAddress2";
			// 
			// textBox5
			// 
			this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(2.8860256671905518D));
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4833335876464844D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
			this.textBox5.Style.Font.Bold = true;
			this.textBox5.Value = "=Fields.CompanyAddress3";
			// 
			// textBox6
			// 
			this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(3.4862260818481445D));
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4833335876464844D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox6.Style.Font.Bold = true;
			this.textBox6.Value = "=Fields.CompanyAddress4";
			// 
			// textBox7
			// 
			this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(4.0862259864807129D));
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4833335876464844D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
			this.textBox7.Style.Font.Bold = true;
			this.textBox7.Value = "=Fields.CompanyAddress5";
			// 
			// textBox9
			// 
			this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5D), Telerik.Reporting.Drawing.Unit.Cm(0.28582477569580078D));
			this.textBox9.Name = "textBox9";
			this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8999981880187988D), Telerik.Reporting.Drawing.Unit.Cm(0.49919998645782471D));
			this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox9.Value = "=Fields.Calibration";
			// 
			// textBox10
			// 
			this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5D), Telerik.Reporting.Drawing.Unit.Cm(0.78502392768859863D));
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(0.49920079112052917D));
			this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox10.Value = "=Fields.Repair";
			// 
			// textBox11
			// 
			this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5D), Telerik.Reporting.Drawing.Unit.Cm(1.2842247486114502D));
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(0.49919998645782471D));
			this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox11.Value = "=Fields.Carriage";
			// 
			// textBox12
			// 
			this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5D), Telerik.Reporting.Drawing.Unit.Cm(1.7834254503250122D));
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8999991416931152D), Telerik.Reporting.Drawing.Unit.Cm(0.49919998645782471D));
			this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox12.Value = "=Fields.Investigation";
			// 
			// textBox13
			// 
			this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5D), Telerik.Reporting.Drawing.Unit.Cm(2.2826263904571533D));
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8999991416931152D), Telerik.Reporting.Drawing.Unit.Cm(0.49919915199279785D));
			this.textBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox13.Value = "=Fields.Parts";
			// 
			// textBox14
			// 
			this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.41666707396507263D), Telerik.Reporting.Drawing.Unit.Cm(6.985626220703125D));
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.99240255355835D), Telerik.Reporting.Drawing.Unit.Cm(0.59999889135360718D));
			this.textBox14.Value = "=Fields.QuoteNo";
			// 
			// textBox15
			// 
			this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.6999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(6.98562479019165D));
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
			this.textBox15.Value = "=Fields.JobRef";
			// 
			// textBox16
			// 
			this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Cm(1.5858244895935059D));
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7000007629394531D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
			this.textBox16.Value = "=Fields.Instrument";
			// 
			// ReportData
			// 
			this.ReportData.DataMember = "GetQuoteReportData";
			this.ReportData.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateQuoteReportDataProvider);
			this.ReportData.Name = "ReportData";
			this.ReportData.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("quoteId", typeof(System.Guid), null)});
			// 
			// textBox17
			// 
			this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.6999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(7.7856249809265137D));
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox17.Value = "=Fields.AdviceNo";
			// 
			// textBox8
			// 
			this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.42426350712776184D), Telerik.Reporting.Drawing.Unit.Cm(7.7864251136779785D));
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.99240255355835D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
			this.textBox8.Value = "=Fields.OrderNo";
			// 
			// TelerikQuoteReport
			// 
			this.DataSource = this.ReportData;
			this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup});
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
			this.Name = "TelerikQuoteReport";
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
			this.Width = Telerik.Reporting.Drawing.Unit.Pixel(635D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.ObjectDataSource ReportData;
		private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
		private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
		private Telerik.Reporting.Group labelsGroup;
		private Telerik.Reporting.PageHeaderSection pageHeader;
		private Telerik.Reporting.PageFooterSection pageFooter;
		private Telerik.Reporting.TextBox currentTimeTextBox;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.ReportHeaderSection reportHeader;
		private Telerik.Reporting.ReportFooterSection reportFooter;
		private Telerik.Reporting.DetailSection detail;
		public Telerik.Reporting.PictureBox MainLogo;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.TextBox textBox1;
		private Telerik.Reporting.TextBox textBox2;
		private Telerik.Reporting.TextBox textBox3;
		private Telerik.Reporting.TextBox textBox4;
		private Telerik.Reporting.TextBox textBox5;
		private Telerik.Reporting.TextBox textBox6;
		private Telerik.Reporting.TextBox textBox7;
		private Telerik.Reporting.TextBox textBox9;
		private Telerik.Reporting.TextBox textBox10;
		private Telerik.Reporting.TextBox textBox11;
		private Telerik.Reporting.TextBox textBox12;
		private Telerik.Reporting.TextBox textBox13;
		private Telerik.Reporting.TextBox textBox14;
		private Telerik.Reporting.TextBox textBox15;
		private Telerik.Reporting.TextBox textBox16;
		private Telerik.Reporting.TextBox textBox17;
		private Telerik.Reporting.TextBox textBox8;

	}
}