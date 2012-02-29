namespace JobSystem.Mvc.Reports
{
	partial class ConsignmentNote
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsignmentNote));
			Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
			this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
			this.sqlDataSource2 = new Telerik.Reporting.SqlDataSource();
			this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
			this.itemNoCaptionTextBox = new Telerik.Reporting.TextBox();
			this.equipmentCaptionTextBox = new Telerik.Reporting.TextBox();
			this.instructionsCaptionTextBox = new Telerik.Reporting.TextBox();
			this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.labelsGroup = new Telerik.Reporting.Group();
			this.pageHeader = new Telerik.Reporting.PageHeaderSection();
			this.reportNameTextBox = new Telerik.Reporting.TextBox();
			this.pageFooter = new Telerik.Reporting.PageFooterSection();
			this.currentTimeTextBox = new Telerik.Reporting.TextBox();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.consignmentNoDataTextBox = new Telerik.Reporting.TextBox();
			this.mainLogoDataTextBox = new Telerik.Reporting.TextBox();
			this.nameDataTextBox = new Telerik.Reporting.TextBox();
			this.address1DataTextBox = new Telerik.Reporting.TextBox();
			this.address2DataTextBox = new Telerik.Reporting.TextBox();
			this.address3DataTextBox = new Telerik.Reporting.TextBox();
			this.address4DataTextBox = new Telerik.Reporting.TextBox();
			this.address5DataTextBox = new Telerik.Reporting.TextBox();
			this.telephoneDataTextBox = new Telerik.Reporting.TextBox();
			this.faxDataTextBox = new Telerik.Reporting.TextBox();
			this.wwwDataTextBox = new Telerik.Reporting.TextBox();
			this.reportFooter = new Telerik.Reporting.ReportFooterSection();
			this.detail = new Telerik.Reporting.DetailSection();
			this.itemNoDataTextBox = new Telerik.Reporting.TextBox();
			this.manufacturerDataTextBox = new Telerik.Reporting.TextBox();
			this.modelNoDataTextBox = new Telerik.Reporting.TextBox();
			this.descriptionDataTextBox = new Telerik.Reporting.TextBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// sqlDataSource1
			// 
			this.sqlDataSource1.ConnectionString = "JobSystemDevelopment";
			this.sqlDataSource1.Name = "sqlDataSource1";
			this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
			// 
			// sqlDataSource2
			// 
			this.sqlDataSource2.ConnectionString = "JobSystemDevelopment";
			this.sqlDataSource2.Name = "sqlDataSource2";
			this.sqlDataSource2.SelectCommand = resources.GetString("sqlDataSource2.SelectCommand");
			// 
			// labelsGroupHeader
			// 
			this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
			this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.itemNoCaptionTextBox,
            this.equipmentCaptionTextBox,
            this.instructionsCaptionTextBox});
			this.labelsGroupHeader.Name = "labelsGroupHeader";
			this.labelsGroupHeader.PrintOnEveryPage = true;
			// 
			// itemNoCaptionTextBox
			// 
			this.itemNoCaptionTextBox.CanGrow = true;
			this.itemNoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916716784238815D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.itemNoCaptionTextBox.Name = "itemNoCaptionTextBox";
			this.itemNoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8468828201293945D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.itemNoCaptionTextBox.StyleName = "Caption";
			this.itemNoCaptionTextBox.Value = "Item No";
			// 
			// equipmentCaptionTextBox
			// 
			this.equipmentCaptionTextBox.CanGrow = true;
			this.equipmentCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.equipmentCaptionTextBox.Name = "equipmentCaptionTextBox";
			this.equipmentCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8209371566772461D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.equipmentCaptionTextBox.StyleName = "Caption";
			this.equipmentCaptionTextBox.Value = "Equipment";
			// 
			// instructionsCaptionTextBox
			// 
			this.instructionsCaptionTextBox.CanGrow = true;
			this.instructionsCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.873854637145996D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.instructionsCaptionTextBox.Name = "instructionsCaptionTextBox";
			this.instructionsCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.instructionsCaptionTextBox.StyleName = "Caption";
			this.instructionsCaptionTextBox.Value = "Instructions";
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
			this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
			this.pageHeader.Name = "pageHeader";
			// 
			// reportNameTextBox
			// 
			this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.reportNameTextBox.Name = "reportNameTextBox";
			this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.708333015441895D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.reportNameTextBox.StyleName = "PageInfo";
			this.reportNameTextBox.Value = "ConsignmentNote";
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
			this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(6.48562479019165D);
			this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.consignmentNoDataTextBox,
            this.mainLogoDataTextBox,
            this.nameDataTextBox,
            this.address1DataTextBox,
            this.address2DataTextBox,
            this.address3DataTextBox,
            this.address4DataTextBox,
            this.address5DataTextBox,
            this.telephoneDataTextBox,
            this.faxDataTextBox,
            this.wwwDataTextBox});
			this.reportHeader.Name = "reportHeader";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D), Telerik.Reporting.Drawing.Unit.Cm(6.385624885559082D));
			this.titleTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "Consignment Note";
			// 
			// consignmentNoDataTextBox
			// 
			this.consignmentNoDataTextBox.CanGrow = true;
			this.consignmentNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.5D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
			this.consignmentNoDataTextBox.Name = "consignmentNoDataTextBox";
			this.consignmentNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.261246681213379D), Telerik.Reporting.Drawing.Unit.Cm(0.78552514314651489D));
			this.consignmentNoDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.consignmentNoDataTextBox.Style.Font.Bold = true;
			this.consignmentNoDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
			this.consignmentNoDataTextBox.StyleName = "Data";
			this.consignmentNoDataTextBox.Value = "=Fields.ConsignmentNo";
			// 
			// mainLogoDataTextBox
			// 
			this.mainLogoDataTextBox.CanGrow = true;
			this.mainLogoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.515641212463379D), Telerik.Reporting.Drawing.Unit.Cm(1.085625171661377D));
			this.mainLogoDataTextBox.Name = "mainLogoDataTextBox";
			this.mainLogoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.2456049919128418D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.mainLogoDataTextBox.StyleName = "Data";
			this.mainLogoDataTextBox.Value = "=Fields.MainLogo";
			// 
			// nameDataTextBox
			// 
			this.nameDataTextBox.CanGrow = true;
			this.nameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(2.4856250286102295D));
			this.nameDataTextBox.Name = "nameDataTextBox";
			this.nameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.nameDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.nameDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.nameDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.nameDataTextBox.StyleName = "Data";
			this.nameDataTextBox.Value = "=Fields.Name";
			// 
			// address1DataTextBox
			// 
			this.address1DataTextBox.CanGrow = true;
			this.address1DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335465431213379D), Telerik.Reporting.Drawing.Unit.Cm(2.8854246139526367D));
			this.address1DataTextBox.Name = "address1DataTextBox";
			this.address1DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.address1DataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.address1DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.address1DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address1DataTextBox.StyleName = "Data";
			this.address1DataTextBox.Value = "=Fields.Address1";
			// 
			// address2DataTextBox
			// 
			this.address2DataTextBox.CanGrow = true;
			this.address2DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(3.2850236892700195D));
			this.address2DataTextBox.Name = "address2DataTextBox";
			this.address2DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.address2DataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.address2DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.address2DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address2DataTextBox.StyleName = "Data";
			this.address2DataTextBox.Value = "=Fields.Address2";
			// 
			// address3DataTextBox
			// 
			this.address3DataTextBox.CanGrow = true;
			this.address3DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335465431213379D), Telerik.Reporting.Drawing.Unit.Cm(3.6846230030059814D));
			this.address3DataTextBox.Name = "address3DataTextBox";
			this.address3DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.address3DataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.address3DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.address3DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address3DataTextBox.StyleName = "Data";
			this.address3DataTextBox.Value = "=Fields.Address3";
			// 
			// address4DataTextBox
			// 
			this.address4DataTextBox.CanGrow = true;
			this.address4DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(4.0842223167419434D));
			this.address4DataTextBox.Name = "address4DataTextBox";
			this.address4DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.address4DataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.address4DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.address4DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address4DataTextBox.StyleName = "Data";
			this.address4DataTextBox.Value = "=Fields.Address4";
			// 
			// address5DataTextBox
			// 
			this.address5DataTextBox.CanGrow = true;
			this.address5DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(4.4838218688964844D));
			this.address5DataTextBox.Name = "address5DataTextBox";
			this.address5DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.address5DataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.address5DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.address5DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address5DataTextBox.StyleName = "Data";
			this.address5DataTextBox.Value = "=Fields.Address5";
			// 
			// telephoneDataTextBox
			// 
			this.telephoneDataTextBox.CanGrow = true;
			this.telephoneDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(4.8834209442138672D));
			this.telephoneDataTextBox.Name = "telephoneDataTextBox";
			this.telephoneDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.telephoneDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.telephoneDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.telephoneDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.telephoneDataTextBox.StyleName = "Data";
			this.telephoneDataTextBox.Value = "=Fields.Telephone";
			// 
			// faxDataTextBox
			// 
			this.faxDataTextBox.CanGrow = true;
			this.faxDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(5.2830204963684082D));
			this.faxDataTextBox.Name = "faxDataTextBox";
			this.faxDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.faxDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.faxDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.faxDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.faxDataTextBox.StyleName = "Data";
			this.faxDataTextBox.Value = "=Fields.Fax";
			// 
			// wwwDataTextBox
			// 
			this.wwwDataTextBox.CanGrow = true;
			this.wwwDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(5.682619571685791D));
			this.wwwDataTextBox.Name = "wwwDataTextBox";
			this.wwwDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277058601379395D), Telerik.Reporting.Drawing.Unit.Cm(0.39959919452667236D));
			this.wwwDataTextBox.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(26)))), ((int)(((byte)(88)))));
			this.wwwDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			this.wwwDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.wwwDataTextBox.StyleName = "Data";
			this.wwwDataTextBox.Value = "=Fields.Www";
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
            this.itemNoDataTextBox,
            this.manufacturerDataTextBox,
            this.modelNoDataTextBox,
            this.descriptionDataTextBox});
			this.detail.Name = "detail";
			// 
			// itemNoDataTextBox
			// 
			this.itemNoDataTextBox.CanGrow = true;
			this.itemNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.itemNoDataTextBox.Name = "itemNoDataTextBox";
			this.itemNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.8468828201293945D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.itemNoDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.itemNoDataTextBox.StyleName = "Data";
			this.itemNoDataTextBox.Value = "=Fields.ItemNo";
			// 
			// manufacturerDataTextBox
			// 
			this.manufacturerDataTextBox.CanGrow = true;
			this.manufacturerDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.manufacturerDataTextBox.Name = "manufacturerDataTextBox";
			this.manufacturerDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5000002384185791D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.manufacturerDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.manufacturerDataTextBox.StyleName = "Data";
			this.manufacturerDataTextBox.Value = "=Fields.Manufacturer";
			// 
			// modelNoDataTextBox
			// 
			this.modelNoDataTextBox.CanGrow = true;
			this.modelNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.modelNoDataTextBox.Name = "modelNoDataTextBox";
			this.modelNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7999999523162842D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.modelNoDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.modelNoDataTextBox.StyleName = "Data";
			this.modelNoDataTextBox.Value = "=Fields.ModelNo";
			// 
			// descriptionDataTextBox
			// 
			this.descriptionDataTextBox.CanGrow = true;
			this.descriptionDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.5D), Telerik.Reporting.Drawing.Unit.Cm(0.075308710336685181D));
			this.descriptionDataTextBox.Name = "descriptionDataTextBox";
			this.descriptionDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3209373950958252D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.descriptionDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
			this.descriptionDataTextBox.StyleName = "Data";
			this.descriptionDataTextBox.Value = "=Fields.Description";
			// 
			// ConsignmentNote
			// 
			this.DataSource = this.sqlDataSource2;
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
			this.Name = "ConsignmentNote";
			this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D);
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			this.Style.BackgroundColor = System.Drawing.Color.White;
			styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
			styleRule1.Style.BackgroundColor = System.Drawing.Color.Empty;
			styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
			styleRule1.Style.Font.Name = "Tahoma";
			styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
			styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
			styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
			styleRule2.Style.Color = System.Drawing.Color.White;
			styleRule2.Style.Font.Bold = true;
			styleRule2.Style.Font.Italic = false;
			styleRule2.Style.Font.Name = "Tahoma";
			styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
			styleRule2.Style.Font.Strikeout = false;
			styleRule2.Style.Font.Underline = false;
			styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
			styleRule3.Style.Color = System.Drawing.Color.Black;
			styleRule3.Style.Font.Name = "Tahoma";
			styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
			styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
			styleRule4.Style.Color = System.Drawing.Color.Black;
			styleRule4.Style.Font.Name = "Tahoma";
			styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
			styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
			this.Width = Telerik.Reporting.Drawing.Unit.Cm(19.69999885559082D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.SqlDataSource sqlDataSource1;
		private Telerik.Reporting.SqlDataSource sqlDataSource2;
		private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
		private Telerik.Reporting.TextBox itemNoCaptionTextBox;
		private Telerik.Reporting.TextBox equipmentCaptionTextBox;
		private Telerik.Reporting.TextBox instructionsCaptionTextBox;
		private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
		private Telerik.Reporting.Group labelsGroup;
		private Telerik.Reporting.PageHeaderSection pageHeader;
		private Telerik.Reporting.PageFooterSection pageFooter;
		private Telerik.Reporting.TextBox currentTimeTextBox;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.ReportHeaderSection reportHeader;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.TextBox consignmentNoDataTextBox;
		private Telerik.Reporting.TextBox mainLogoDataTextBox;
		private Telerik.Reporting.TextBox nameDataTextBox;
		private Telerik.Reporting.TextBox address1DataTextBox;
		private Telerik.Reporting.TextBox address2DataTextBox;
		private Telerik.Reporting.TextBox address3DataTextBox;
		private Telerik.Reporting.TextBox address4DataTextBox;
		private Telerik.Reporting.TextBox address5DataTextBox;
		private Telerik.Reporting.TextBox telephoneDataTextBox;
		private Telerik.Reporting.TextBox faxDataTextBox;
		private Telerik.Reporting.TextBox wwwDataTextBox;
		private Telerik.Reporting.ReportFooterSection reportFooter;
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.TextBox itemNoDataTextBox;
		private Telerik.Reporting.TextBox manufacturerDataTextBox;
		private Telerik.Reporting.TextBox modelNoDataTextBox;
		private Telerik.Reporting.TextBox descriptionDataTextBox;
		private Telerik.Reporting.TextBox reportNameTextBox;

	}
}