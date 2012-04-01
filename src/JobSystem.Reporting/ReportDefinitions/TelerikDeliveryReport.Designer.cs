namespace JobSystem.Reporting.ReportDefinitions
{
	partial class TelerikDeliveryReport
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
			this.ReportData = new Telerik.Reporting.ObjectDataSource();
			this.textBox23 = new Telerik.Reporting.TextBox();
			this.textBox16 = new Telerik.Reporting.TextBox();
			this.textBox22 = new Telerik.Reporting.TextBox();
			this.textBox21 = new Telerik.Reporting.TextBox();
			this.textBox20 = new Telerik.Reporting.TextBox();
			this.textBox19 = new Telerik.Reporting.TextBox();
			this.textBox18 = new Telerik.Reporting.TextBox();
			this.textBox17 = new Telerik.Reporting.TextBox();
			this.textBox6 = new Telerik.Reporting.TextBox();
			this.textBox5 = new Telerik.Reporting.TextBox();
			this.textBox2 = new Telerik.Reporting.TextBox();
			this.textBox3 = new Telerik.Reporting.TextBox();
			this.textBox4 = new Telerik.Reporting.TextBox();
			this.textBox1 = new Telerik.Reporting.TextBox();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.currentTimeTextBox = new Telerik.Reporting.TextBox();
			this.MainLogo = new Telerik.Reporting.PictureBox();
			this.supplierAddress5DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress4DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress3DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress2DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierAddress1DataTextBox = new Telerik.Reporting.TextBox();
			this.supplierNameDataTextBox = new Telerik.Reporting.TextBox();
			this.consignmentNoDataTextBox = new Telerik.Reporting.TextBox();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.textBox15 = new Telerik.Reporting.TextBox();
			this.textBox14 = new Telerik.Reporting.TextBox();
			this.textBox13 = new Telerik.Reporting.TextBox();
			this.textBox12 = new Telerik.Reporting.TextBox();
			this.shape5 = new Telerik.Reporting.Shape();
			this.textBox8 = new Telerik.Reporting.TextBox();
			this.textBox7 = new Telerik.Reporting.TextBox();
			this.dateCreatedDataTextBox = new Telerik.Reporting.TextBox();
			this.raisedByDataTextBox = new Telerik.Reporting.TextBox();
			this.shape2 = new Telerik.Reporting.Shape();
			this.shape3 = new Telerik.Reporting.Shape();
			this.shape7 = new Telerik.Reporting.Shape();
			this.shape8 = new Telerik.Reporting.Shape();
			this.shape6 = new Telerik.Reporting.Shape();
			this.textBox9 = new Telerik.Reporting.TextBox();
			this.textBox10 = new Telerik.Reporting.TextBox();
			this.textBox11 = new Telerik.Reporting.TextBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeaderSection1
			// 
			this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D);
			this.pageHeaderSection1.Name = "pageHeaderSection1";
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(9.2999992370605469D);
			this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.shape7,
            this.shape6,
            this.shape8,
            this.shape2,
            this.shape3,
            this.shape5,
            this.MainLogo,
            this.supplierAddress5DataTextBox,
            this.supplierAddress4DataTextBox,
            this.supplierAddress3DataTextBox,
            this.supplierAddress2DataTextBox,
            this.supplierAddress1DataTextBox,
            this.supplierNameDataTextBox,
            this.consignmentNoDataTextBox,
            this.titleTextBox,
            this.textBox15,
            this.textBox14,
            this.textBox13,
            this.textBox12,
            this.textBox8,
            this.textBox7,
            this.dateCreatedDataTextBox,
            this.raisedByDataTextBox,
            this.textBox9,
            this.textBox10,
            this.textBox11});
			this.detail.Name = "detail";
			// 
			// pageFooterSection1
			// 
			this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.8921844959259033D);
			this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox23,
            this.textBox16,
            this.textBox22,
            this.textBox21,
            this.textBox20,
            this.textBox19,
            this.textBox18,
            this.textBox17,
            this.textBox6,
            this.textBox5,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox1,
            this.pageInfoTextBox,
            this.currentTimeTextBox});
			this.pageFooterSection1.Name = "pageFooterSection1";
			// 
			// ReportData
			// 
			this.ReportData.DataMember = "GetConsignmentReportData";
			this.ReportData.DataSource = typeof(JobSystem.Reporting.Data.NHibernate.NHibernateConsignmentReportDataProvider);
			this.ReportData.Name = "ReportData";
			this.ReportData.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("consignmentId", typeof(System.Guid), null)});
			// 
			// textBox23
			// 
			this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.2814579010009766D), Telerik.Reporting.Drawing.Unit.Cm(1.6933332681655884D));
			this.textBox23.Name = "textBox23";
			this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox23.Style.Color = System.Drawing.Color.Gray;
			this.textBox23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox23.Value = ",";
			// 
			// textBox16
			// 
			this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.2814579010009766D), Telerik.Reporting.Drawing.Unit.Cm(1.2170833349227905D));
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox16.Style.Color = System.Drawing.Color.Gray;
			this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox16.Value = ",";
			// 
			// textBox22
			// 
			this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(395D), Telerik.Reporting.Drawing.Unit.Pixel(82D));
			this.textBox22.Name = "textBox22";
			this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(96.15020751953125D), Telerik.Reporting.Drawing.Unit.Pixel(14.525634765625D));
			this.textBox22.Style.Color = System.Drawing.Color.Gray;
			this.textBox22.Value = "=Fields.CompanyWww";
			// 
			// textBox21
			// 
			this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.001249313354492D), Telerik.Reporting.Drawing.Unit.Cm(2.1695833206176758D));
			this.textBox21.Name = "textBox21";
			this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.43875086307525635D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox21.Style.Color = System.Drawing.Color.Gray;
			this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox21.Value = "W:";
			// 
			// textBox20
			// 
			this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.402916431427002D), Telerik.Reporting.Drawing.Unit.Cm(2.1695833206176758D));
			this.textBox20.Name = "textBox20";
			this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.43875086307525635D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox20.Style.Color = System.Drawing.Color.Gray;
			this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox20.Value = "F:";
			// 
			// textBox19
			// 
			this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(132D), Telerik.Reporting.Drawing.Unit.Pixel(82D));
			this.textBox19.Name = "textBox19";
			this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(110.6065673828125D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox19.Style.Color = System.Drawing.Color.Gray;
			this.textBox19.Value = "=Fields.CompanyTelephone";
			// 
			// textBox18
			// 
			this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.042708158493042D), Telerik.Reporting.Drawing.Unit.Cm(2.1695833206176758D));
			this.textBox18.Name = "textBox18";
			this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.43875086307525635D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox18.Style.Color = System.Drawing.Color.Gray;
			this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox18.Value = "T:";
			// 
			// textBox17
			// 
			this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(259D), Telerik.Reporting.Drawing.Unit.Pixel(82D));
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(119.4803466796875D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox17.Style.Color = System.Drawing.Color.Gray;
			this.textBox17.Value = "=Fields.CompanyFax";
			// 
			// textBox6
			// 
			this.textBox6.CanShrink = true;
			this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.4137496948242188D), Telerik.Reporting.Drawing.Unit.Cm(1.6933332681655884D));
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.3502306938171387D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox6.Style.Color = System.Drawing.Color.Gray;
			this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox6.Value = "=Fields.CompanyAddress5";
			// 
			// textBox5
			// 
			this.textBox5.CanShrink = true;
			this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3283333778381348D), Telerik.Reporting.Drawing.Unit.Cm(1.6933332681655884D));
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.9654159545898438D), Telerik.Reporting.Drawing.Unit.Cm(0.38452392816543579D));
			this.textBox5.Style.Color = System.Drawing.Color.Gray;
			this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.textBox5.Value = "=Fields.CompanyAddress4";
			// 
			// textBox2
			// 
			this.textBox2.CanShrink = true;
			this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3283333778381348D), Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D));
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(11.448140144348145D), Telerik.Reporting.Drawing.Unit.Cm(0.38452392816543579D));
			this.textBox2.Style.Color = System.Drawing.Color.Gray;
			this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox2.Value = "=Fields.CompanyAddress1";
			// 
			// textBox3
			// 
			this.textBox3.CanShrink = true;
			this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3283333778381348D), Telerik.Reporting.Drawing.Unit.Cm(1.2170833349227905D));
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.9654159545898438D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox3.Style.Color = System.Drawing.Color.Gray;
			this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.textBox3.Value = "=Fields.CompanyAddress2";
			// 
			// textBox4
			// 
			this.textBox4.CanShrink = true;
			this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.4137496948242188D), Telerik.Reporting.Drawing.Unit.Cm(1.2170833349227905D));
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.3500304222106934D), Telerik.Reporting.Drawing.Unit.Cm(0.38452315330505371D));
			this.textBox4.Style.Color = System.Drawing.Color.Gray;
			this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.textBox4.Value = "=Fields.CompanyAddress3";
			// 
			// textBox1
			// 
			this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3283333778381348D), Telerik.Reporting.Drawing.Unit.Cm(0.23812499642372131D));
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(11.448139190673828D), Telerik.Reporting.Drawing.Unit.Cm(0.38452392816543579D));
			this.textBox1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox1.Value = "=Fields.CompanyName";
			// 
			// pageInfoTextBox
			// 
			this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.552083015441895D), Telerik.Reporting.Drawing.Unit.Cm(2.1695833206176758D));
			this.pageInfoTextBox.Name = "pageInfoTextBox";
			this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2429072856903076D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.pageInfoTextBox.Style.Color = System.Drawing.Color.Gray;
			this.pageInfoTextBox.Style.Font.Name = "Arial";
			this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.pageInfoTextBox.StyleName = "PageInfo";
			this.pageInfoTextBox.Value = "=PageNumber";
			// 
			// currentTimeTextBox
			// 
			this.currentTimeTextBox.Format = "{0:D}";
			this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.1695833206176758D));
			this.currentTimeTextBox.Name = "currentTimeTextBox";
			this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9470834732055664D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.currentTimeTextBox.Style.Color = System.Drawing.Color.Gray;
			this.currentTimeTextBox.Style.Font.Name = "Arial";
			this.currentTimeTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.currentTimeTextBox.StyleName = "PageInfo";
			this.currentTimeTextBox.Value = "=NOW()";
			// 
			// MainLogo
			// 
			this.MainLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(415.24407958984375D), Telerik.Reporting.Drawing.Unit.Pixel(7.559051513671875D));
			this.MainLogo.Name = "MainLogo";
			this.MainLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(179.41732788085938D), Telerik.Reporting.Drawing.Unit.Pixel(111.26771545410156D));
			// 
			// supplierAddress5DataTextBox
			// 
			this.supplierAddress5DataTextBox.CanGrow = true;
			this.supplierAddress5DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(3.9116613864898682D));
			this.supplierAddress5DataTextBox.Name = "supplierAddress5DataTextBox";
			this.supplierAddress5DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612489700317383D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierAddress5DataTextBox.Style.Font.Bold = true;
			this.supplierAddress5DataTextBox.Style.Font.Name = "Arial";
			this.supplierAddress5DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierAddress5DataTextBox.StyleName = "Data";
			this.supplierAddress5DataTextBox.Value = "=Fields.SupplierAddress5";
			// 
			// supplierAddress4DataTextBox
			// 
			this.supplierAddress4DataTextBox.CanGrow = true;
			this.supplierAddress4DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(3.4074540138244629D));
			this.supplierAddress4DataTextBox.Name = "supplierAddress4DataTextBox";
			this.supplierAddress4DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612489700317383D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierAddress4DataTextBox.Style.Font.Bold = true;
			this.supplierAddress4DataTextBox.Style.Font.Name = "Arial";
			this.supplierAddress4DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierAddress4DataTextBox.StyleName = "Data";
			this.supplierAddress4DataTextBox.Value = "=Fields.SupplierAddress4";
			// 
			// supplierAddress3DataTextBox
			// 
			this.supplierAddress3DataTextBox.CanGrow = true;
			this.supplierAddress3DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(2.9032468795776367D));
			this.supplierAddress3DataTextBox.Name = "supplierAddress3DataTextBox";
			this.supplierAddress3DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612499237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierAddress3DataTextBox.Style.Font.Bold = true;
			this.supplierAddress3DataTextBox.Style.Font.Name = "Arial";
			this.supplierAddress3DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierAddress3DataTextBox.StyleName = "Data";
			this.supplierAddress3DataTextBox.Value = "=Fields.SupplierAddress3";
			// 
			// supplierAddress2DataTextBox
			// 
			this.supplierAddress2DataTextBox.CanGrow = true;
			this.supplierAddress2DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(2.3990395069122314D));
			this.supplierAddress2DataTextBox.Name = "supplierAddress2DataTextBox";
			this.supplierAddress2DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612489700317383D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierAddress2DataTextBox.Style.Font.Bold = true;
			this.supplierAddress2DataTextBox.Style.Font.Name = "Arial";
			this.supplierAddress2DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierAddress2DataTextBox.StyleName = "Data";
			this.supplierAddress2DataTextBox.Value = "=Fields.SupplierAddress2";
			// 
			// supplierAddress1DataTextBox
			// 
			this.supplierAddress1DataTextBox.CanGrow = true;
			this.supplierAddress1DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(1.8948321342468262D));
			this.supplierAddress1DataTextBox.Name = "supplierAddress1DataTextBox";
			this.supplierAddress1DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612489700317383D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierAddress1DataTextBox.Style.Font.Bold = true;
			this.supplierAddress1DataTextBox.Style.Font.Name = "Arial";
			this.supplierAddress1DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierAddress1DataTextBox.StyleName = "Data";
			this.supplierAddress1DataTextBox.Value = "=Fields.SupplierAddress1";
			// 
			// supplierNameDataTextBox
			// 
			this.supplierNameDataTextBox.CanGrow = true;
			this.supplierNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(1.3906248807907105D));
			this.supplierNameDataTextBox.Name = "supplierNameDataTextBox";
			this.supplierNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8612504005432129D), Telerik.Reporting.Drawing.Unit.Cm(0.50420725345611572D));
			this.supplierNameDataTextBox.Style.Font.Bold = true;
			this.supplierNameDataTextBox.Style.Font.Name = "Arial";
			this.supplierNameDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.supplierNameDataTextBox.StyleName = "Data";
			this.supplierNameDataTextBox.Value = "=Fields.SupplierName";
			// 
			// consignmentNoDataTextBox
			// 
			this.consignmentNoDataTextBox.CanGrow = true;
			this.consignmentNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3283333778381348D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
			this.consignmentNoDataTextBox.Name = "consignmentNoDataTextBox";
			this.consignmentNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.085416316986084D), Telerik.Reporting.Drawing.Unit.Cm(0.88552480936050415D));
			this.consignmentNoDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.consignmentNoDataTextBox.Style.Font.Name = "Arial";
			this.consignmentNoDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			this.consignmentNoDataTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.consignmentNoDataTextBox.StyleName = "Data";
			this.consignmentNoDataTextBox.Value = "=Fields.ConsignmentNo";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.40333291888237D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8966671228408814D), Telerik.Reporting.Drawing.Unit.Cm(0.88562500476837158D));
			this.titleTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.titleTextBox.Style.Font.Bold = true;
			this.titleTextBox.Style.Font.Name = "Arial";
			this.titleTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "Delivery";
			// 
			// textBox15
			// 
			this.textBox15.Angle = 0D;
			this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(8.4488449096679688D), Telerik.Reporting.Drawing.Unit.Pixel(249.8582763671875D));
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(241.8897705078125D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox15.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox15.Style.Font.Bold = true;
			this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox15.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox15.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox15.Value = "RECIEVED BY";
			// 
			// textBox14
			// 
			this.textBox14.Angle = 0D;
			this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(484.44888305664062D), Telerik.Reporting.Drawing.Unit.Pixel(249.8582763671875D));
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(118.55528259277344D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox14.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox14.Style.Font.Bold = true;
			this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox14.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox14.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox14.Value = "DATE";
			// 
			// textBox13
			// 
			this.textBox13.Angle = 0D;
			this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(249.44883728027344D), Telerik.Reporting.Drawing.Unit.Pixel(249.8582763671875D));
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(234.33073425292969D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox13.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox13.Style.Font.Bold = true;
			this.textBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox13.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox13.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox13.Value = "SIGNED";
			// 
			// textBox12
			// 
			this.textBox12.Angle = 0D;
			this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(250.44883728027344D), Telerik.Reporting.Drawing.Unit.Pixel(185.8582763671875D));
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(233.68534851074219D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox12.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox12.Style.Font.Bold = true;
			this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox12.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox12.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox12.Value = "SIGNED";
			// 
			// shape5
			// 
			this.shape5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(249.44883728027344D), Telerik.Reporting.Drawing.Unit.Pixel(173.8582763671875D));
			this.shape5.Name = "shape5";
			this.shape5.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(234.33074951171875D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape5.Stretch = true;
			this.shape5.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.shape5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			// 
			// textBox8
			// 
			this.textBox8.Angle = 0D;
			this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(484.44888305664062D), Telerik.Reporting.Drawing.Unit.Pixel(185.8582763671875D));
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(118.55528259277344D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox8.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox8.Style.Font.Bold = true;
			this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox8.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox8.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox8.Value = "DATE";
			// 
			// textBox7
			// 
			this.textBox7.Angle = 0D;
			this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(8.4488449096679688D), Telerik.Reporting.Drawing.Unit.Pixel(185.8582763671875D));
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(241.8897705078125D), Telerik.Reporting.Drawing.Unit.Pixel(26.456695556640625D));
			this.textBox7.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox7.Style.Font.Bold = true;
			this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.textBox7.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
			this.textBox7.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(5D);
			this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.textBox7.Value = "PREPARED BY";
			// 
			// dateCreatedDataTextBox
			// 
			this.dateCreatedDataTextBox.CanGrow = true;
			this.dateCreatedDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.817709922790527D), Telerik.Reporting.Drawing.Unit.Cm(5.4995837211608887D));
			this.dateCreatedDataTextBox.Name = "dateCreatedDataTextBox";
			this.dateCreatedDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.1298997402191162D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.dateCreatedDataTextBox.Style.Font.Name = "Arial";
			this.dateCreatedDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.dateCreatedDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.dateCreatedDataTextBox.StyleName = "Data";
			this.dateCreatedDataTextBox.Value = "=Fields.DateCreated";
			// 
			// raisedByDataTextBox
			// 
			this.raisedByDataTextBox.CanGrow = true;
			this.raisedByDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.22354234755039215D), Telerik.Reporting.Drawing.Unit.Cm(5.4995837211608887D));
			this.raisedByDataTextBox.Name = "raisedByDataTextBox";
			this.raisedByDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.4000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.raisedByDataTextBox.Style.Font.Name = "Arial";
			this.raisedByDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.raisedByDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
			this.raisedByDataTextBox.StyleName = "Data";
			this.raisedByDataTextBox.Value = "=Fields.RaisedBy";
			// 
			// shape2
			// 
			this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(8.4488449096679688D), Telerik.Reporting.Drawing.Unit.Pixel(173.8582763671875D));
			this.shape2.Name = "shape2";
			this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(241.88980102539063D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape2.Stretch = true;
			this.shape2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			// 
			// shape3
			// 
			this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(484.44888305664062D), Telerik.Reporting.Drawing.Unit.Pixel(173.8582763671875D));
			this.shape3.Name = "shape3";
			this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(118.5552978515625D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape3.Stretch = true;
			this.shape3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			// 
			// shape7
			// 
			this.shape7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(8.4488449096679688D), Telerik.Reporting.Drawing.Unit.Pixel(237.8582763671875D));
			this.shape7.Name = "shape7";
			this.shape7.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(241.88983154296875D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape7.Stretch = true;
			this.shape7.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			// 
			// shape8
			// 
			this.shape8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(484.44888305664062D), Telerik.Reporting.Drawing.Unit.Pixel(237.8582763671875D));
			this.shape8.Name = "shape8";
			this.shape8.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(118.5552978515625D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape8.Stretch = true;
			this.shape8.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			// 
			// shape6
			// 
			this.shape6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(249.44883728027344D), Telerik.Reporting.Drawing.Unit.Pixel(237.8582763671875D));
			this.shape6.Name = "shape6";
			this.shape6.ShapeType = new Telerik.Reporting.Drawing.Shapes.PolygonShape(4, 45D, 0);
			this.shape6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(234.33074951171875D), Telerik.Reporting.Drawing.Unit.Pixel(64.251983642578125D));
			this.shape6.Stretch = true;
			this.shape6.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			// 
			// textBox9
			// 
			this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(7.81890869140625D), Telerik.Reporting.Drawing.Unit.Pixel(321.25985717773438D));
			this.textBox9.Name = "textBox9";
			this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(83.14959716796875D), Telerik.Reporting.Drawing.Unit.Pixel(22.677154541015625D));
			this.textBox9.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox9.Style.Font.Bold = true;
			this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox9.Value = "ITEM NO.";
			// 
			// textBox10
			// 
			this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(94.81890869140625D), Telerik.Reporting.Drawing.Unit.Pixel(321.25985717773438D));
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(228.07870483398438D), Telerik.Reporting.Drawing.Unit.Pixel(22.677154541015625D));
			this.textBox10.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox10.Style.Font.Bold = true;
			this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox10.Value = "DESCRIPTION";
			// 
			// textBox11
			// 
			this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Pixel(328.81890869140625D), Telerik.Reporting.Drawing.Unit.Pixel(321.25985717773438D));
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Pixel(266.169677734375D), Telerik.Reporting.Drawing.Unit.Pixel(22.677154541015625D));
			this.textBox11.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(34)))), ((int)(((byte)(60)))));
			this.textBox11.Style.Font.Bold = true;
			this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.textBox11.Value = "INSTRUCTIONS";
			// 
			// TelerikDeliveryReport
			// 
			this.DataSource = this.ReportData;
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
			this.Name = "TelerikDeliveryReport";
			this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			this.Style.BackgroundColor = System.Drawing.Color.White;
			this.Width = Telerik.Reporting.Drawing.Unit.Pixel(635D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.PageFooterSection pageFooterSection1;
		private Telerik.Reporting.ObjectDataSource ReportData;
		private Telerik.Reporting.TextBox textBox23;
		private Telerik.Reporting.TextBox textBox16;
		private Telerik.Reporting.TextBox textBox22;
		private Telerik.Reporting.TextBox textBox21;
		private Telerik.Reporting.TextBox textBox20;
		private Telerik.Reporting.TextBox textBox19;
		private Telerik.Reporting.TextBox textBox18;
		private Telerik.Reporting.TextBox textBox17;
		private Telerik.Reporting.TextBox textBox6;
		private Telerik.Reporting.TextBox textBox5;
		private Telerik.Reporting.TextBox textBox2;
		private Telerik.Reporting.TextBox textBox3;
		private Telerik.Reporting.TextBox textBox4;
		private Telerik.Reporting.TextBox textBox1;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.TextBox currentTimeTextBox;
		public Telerik.Reporting.PictureBox MainLogo;
		private Telerik.Reporting.TextBox supplierAddress5DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress4DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress3DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress2DataTextBox;
		private Telerik.Reporting.TextBox supplierAddress1DataTextBox;
		private Telerik.Reporting.TextBox supplierNameDataTextBox;
		private Telerik.Reporting.TextBox consignmentNoDataTextBox;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.TextBox textBox15;
		private Telerik.Reporting.TextBox textBox14;
		private Telerik.Reporting.TextBox textBox13;
		private Telerik.Reporting.TextBox textBox12;
		private Telerik.Reporting.Shape shape5;
		private Telerik.Reporting.TextBox textBox8;
		private Telerik.Reporting.TextBox textBox7;
		private Telerik.Reporting.TextBox dateCreatedDataTextBox;
		private Telerik.Reporting.TextBox raisedByDataTextBox;
		private Telerik.Reporting.Shape shape2;
		private Telerik.Reporting.Shape shape3;
		private Telerik.Reporting.Shape shape7;
		private Telerik.Reporting.Shape shape8;
		private Telerik.Reporting.Shape shape6;
		private Telerik.Reporting.TextBox textBox9;
		private Telerik.Reporting.TextBox textBox10;
		private Telerik.Reporting.TextBox textBox11;
	}
}