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
			this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
			this.labelsGroup = new Telerik.Reporting.Group();
			this.itemNoCaptionTextBox = new Telerik.Reporting.TextBox();
			this.manufacturerCaptionTextBox = new Telerik.Reporting.TextBox();
			this.modelNoCaptionTextBox = new Telerik.Reporting.TextBox();
			this.descriptionCaptionTextBox = new Telerik.Reporting.TextBox();
			this.pageHeader = new Telerik.Reporting.PageHeaderSection();
			this.reportNameTextBox = new Telerik.Reporting.TextBox();
			this.pageFooter = new Telerik.Reporting.PageFooterSection();
			this.currentTimeTextBox = new Telerik.Reporting.TextBox();
			this.pageInfoTextBox = new Telerik.Reporting.TextBox();
			this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
			this.titleTextBox = new Telerik.Reporting.TextBox();
			this.consignmentNoCaptionTextBox = new Telerik.Reporting.TextBox();
			this.consignmentNoDataTextBox = new Telerik.Reporting.TextBox();
			this.mainLogoCaptionTextBox = new Telerik.Reporting.TextBox();
			this.mainLogoDataTextBox = new Telerik.Reporting.TextBox();
			this.nameCaptionTextBox = new Telerik.Reporting.TextBox();
			this.nameDataTextBox = new Telerik.Reporting.TextBox();
			this.address1CaptionTextBox = new Telerik.Reporting.TextBox();
			this.address1DataTextBox = new Telerik.Reporting.TextBox();
			this.address2CaptionTextBox = new Telerik.Reporting.TextBox();
			this.address2DataTextBox = new Telerik.Reporting.TextBox();
			this.address3CaptionTextBox = new Telerik.Reporting.TextBox();
			this.address3DataTextBox = new Telerik.Reporting.TextBox();
			this.address4CaptionTextBox = new Telerik.Reporting.TextBox();
			this.address4DataTextBox = new Telerik.Reporting.TextBox();
			this.address5CaptionTextBox = new Telerik.Reporting.TextBox();
			this.address5DataTextBox = new Telerik.Reporting.TextBox();
			this.telephoneCaptionTextBox = new Telerik.Reporting.TextBox();
			this.telephoneDataTextBox = new Telerik.Reporting.TextBox();
			this.faxCaptionTextBox = new Telerik.Reporting.TextBox();
			this.faxDataTextBox = new Telerik.Reporting.TextBox();
			this.wwwCaptionTextBox = new Telerik.Reporting.TextBox();
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
            this.manufacturerCaptionTextBox,
            this.modelNoCaptionTextBox,
            this.descriptionCaptionTextBox});
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
			// itemNoCaptionTextBox
			// 
			this.itemNoCaptionTextBox.CanGrow = true;
			this.itemNoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.itemNoCaptionTextBox.Name = "itemNoCaptionTextBox";
			this.itemNoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.itemNoCaptionTextBox.StyleName = "Caption";
			this.itemNoCaptionTextBox.Value = "Item No";
			// 
			// manufacturerCaptionTextBox
			// 
			this.manufacturerCaptionTextBox.CanGrow = true;
			this.manufacturerCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.9932291507720947D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.manufacturerCaptionTextBox.Name = "manufacturerCaptionTextBox";
			this.manufacturerCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.manufacturerCaptionTextBox.StyleName = "Caption";
			this.manufacturerCaptionTextBox.Value = "Manufacturer";
			// 
			// modelNoCaptionTextBox
			// 
			this.modelNoCaptionTextBox.CanGrow = true;
			this.modelNoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.modelNoCaptionTextBox.Name = "modelNoCaptionTextBox";
			this.modelNoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.modelNoCaptionTextBox.StyleName = "Caption";
			this.modelNoCaptionTextBox.Value = "Model No";
			// 
			// descriptionCaptionTextBox
			// 
			this.descriptionCaptionTextBox.CanGrow = true;
			this.descriptionCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.873854637145996D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.descriptionCaptionTextBox.Name = "descriptionCaptionTextBox";
			this.descriptionCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.descriptionCaptionTextBox.StyleName = "Caption";
			this.descriptionCaptionTextBox.Value = "Description";
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
			this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(2.7058331966400146D);
			this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.consignmentNoCaptionTextBox,
            this.consignmentNoDataTextBox,
            this.mainLogoCaptionTextBox,
            this.mainLogoDataTextBox,
            this.nameCaptionTextBox,
            this.nameDataTextBox,
            this.address1CaptionTextBox,
            this.address1DataTextBox,
            this.address2CaptionTextBox,
            this.address2DataTextBox,
            this.address3CaptionTextBox,
            this.address3DataTextBox,
            this.address4CaptionTextBox,
            this.address4DataTextBox,
            this.address5CaptionTextBox,
            this.address5DataTextBox,
            this.telephoneCaptionTextBox,
            this.telephoneDataTextBox,
            this.faxCaptionTextBox,
            this.faxDataTextBox,
            this.wwwCaptionTextBox,
            this.wwwDataTextBox});
			this.reportHeader.Name = "reportHeader";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D), Telerik.Reporting.Drawing.Unit.Cm(2D));
			this.titleTextBox.StyleName = "Title";
			this.titleTextBox.Value = "ConsignmentNote";
			// 
			// consignmentNoCaptionTextBox
			// 
			this.consignmentNoCaptionTextBox.CanGrow = true;
			this.consignmentNoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.consignmentNoCaptionTextBox.Name = "consignmentNoCaptionTextBox";
			this.consignmentNoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.consignmentNoCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.consignmentNoCaptionTextBox.StyleName = "Caption";
			this.consignmentNoCaptionTextBox.Value = "Consignment No:";
			// 
			// consignmentNoDataTextBox
			// 
			this.consignmentNoDataTextBox.CanGrow = true;
			this.consignmentNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.76933711767196655D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.consignmentNoDataTextBox.Name = "consignmentNoDataTextBox";
			this.consignmentNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.consignmentNoDataTextBox.StyleName = "Data";
			this.consignmentNoDataTextBox.Value = "=Fields.ConsignmentNo";
			// 
			// mainLogoCaptionTextBox
			// 
			this.mainLogoCaptionTextBox.CanGrow = true;
			this.mainLogoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.48575758934021D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.mainLogoCaptionTextBox.Name = "mainLogoCaptionTextBox";
			this.mainLogoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.mainLogoCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.mainLogoCaptionTextBox.StyleName = "Caption";
			this.mainLogoCaptionTextBox.Value = "Main Logo:";
			// 
			// mainLogoDataTextBox
			// 
			this.mainLogoDataTextBox.CanGrow = true;
			this.mainLogoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.2021780014038086D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.mainLogoDataTextBox.Name = "mainLogoDataTextBox";
			this.mainLogoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.mainLogoDataTextBox.StyleName = "Data";
			this.mainLogoDataTextBox.Value = "=Fields.MainLogo";
			// 
			// nameCaptionTextBox
			// 
			this.nameCaptionTextBox.CanGrow = true;
			this.nameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.9185984134674072D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.nameCaptionTextBox.Name = "nameCaptionTextBox";
			this.nameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.nameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.nameCaptionTextBox.StyleName = "Caption";
			this.nameCaptionTextBox.Value = "Name:";
			// 
			// nameDataTextBox
			// 
			this.nameDataTextBox.CanGrow = true;
			this.nameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.6350188255310059D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.nameDataTextBox.Name = "nameDataTextBox";
			this.nameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.nameDataTextBox.StyleName = "Data";
			this.nameDataTextBox.Value = "=Fields.Name";
			// 
			// address1CaptionTextBox
			// 
			this.address1CaptionTextBox.CanGrow = true;
			this.address1CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.3514389991760254D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address1CaptionTextBox.Name = "address1CaptionTextBox";
			this.address1CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address1CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address1CaptionTextBox.StyleName = "Caption";
			this.address1CaptionTextBox.Value = "Address1:";
			// 
			// address1DataTextBox
			// 
			this.address1DataTextBox.CanGrow = true;
			this.address1DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.0678591728210449D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address1DataTextBox.Name = "address1DataTextBox";
			this.address1DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address1DataTextBox.StyleName = "Data";
			this.address1DataTextBox.Value = "=Fields.Address1";
			// 
			// address2CaptionTextBox
			// 
			this.address2CaptionTextBox.CanGrow = true;
			this.address2CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.7842793464660645D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address2CaptionTextBox.Name = "address2CaptionTextBox";
			this.address2CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address2CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address2CaptionTextBox.StyleName = "Caption";
			this.address2CaptionTextBox.Value = "Address2:";
			// 
			// address2DataTextBox
			// 
			this.address2DataTextBox.CanGrow = true;
			this.address2DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5006999969482422D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address2DataTextBox.Name = "address2DataTextBox";
			this.address2DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address2DataTextBox.StyleName = "Data";
			this.address2DataTextBox.Value = "=Fields.Address2";
			// 
			// address3CaptionTextBox
			// 
			this.address3CaptionTextBox.CanGrow = true;
			this.address3CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.2171201705932617D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address3CaptionTextBox.Name = "address3CaptionTextBox";
			this.address3CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address3CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address3CaptionTextBox.StyleName = "Caption";
			this.address3CaptionTextBox.Value = "Address3:";
			// 
			// address3DataTextBox
			// 
			this.address3DataTextBox.CanGrow = true;
			this.address3DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335403442382812D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address3DataTextBox.Name = "address3DataTextBox";
			this.address3DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address3DataTextBox.StyleName = "Data";
			this.address3DataTextBox.Value = "=Fields.Address3";
			// 
			// address4CaptionTextBox
			// 
			this.address4CaptionTextBox.CanGrow = true;
			this.address4CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.6499605178833D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address4CaptionTextBox.Name = "address4CaptionTextBox";
			this.address4CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address4CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address4CaptionTextBox.StyleName = "Caption";
			this.address4CaptionTextBox.Value = "Address4:";
			// 
			// address4DataTextBox
			// 
			this.address4DataTextBox.CanGrow = true;
			this.address4DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.36638069152832D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address4DataTextBox.Name = "address4DataTextBox";
			this.address4DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address4DataTextBox.StyleName = "Data";
			this.address4DataTextBox.Value = "=Fields.Address4";
			// 
			// address5CaptionTextBox
			// 
			this.address5CaptionTextBox.CanGrow = true;
			this.address5CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.08280086517334D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address5CaptionTextBox.Name = "address5CaptionTextBox";
			this.address5CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address5CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.address5CaptionTextBox.StyleName = "Caption";
			this.address5CaptionTextBox.Value = "Address5:";
			// 
			// address5DataTextBox
			// 
			this.address5DataTextBox.CanGrow = true;
			this.address5DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.799221038818359D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.address5DataTextBox.Name = "address5DataTextBox";
			this.address5DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.address5DataTextBox.StyleName = "Data";
			this.address5DataTextBox.Value = "=Fields.Address5";
			// 
			// telephoneCaptionTextBox
			// 
			this.telephoneCaptionTextBox.CanGrow = true;
			this.telephoneCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.515641212463379D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.telephoneCaptionTextBox.Name = "telephoneCaptionTextBox";
			this.telephoneCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.telephoneCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.telephoneCaptionTextBox.StyleName = "Caption";
			this.telephoneCaptionTextBox.Value = "Telephone:";
			// 
			// telephoneDataTextBox
			// 
			this.telephoneDataTextBox.CanGrow = true;
			this.telephoneDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.232061386108398D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.telephoneDataTextBox.Name = "telephoneDataTextBox";
			this.telephoneDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.telephoneDataTextBox.StyleName = "Data";
			this.telephoneDataTextBox.Value = "=Fields.Telephone";
			// 
			// faxCaptionTextBox
			// 
			this.faxCaptionTextBox.CanGrow = true;
			this.faxCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.948481559753418D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.faxCaptionTextBox.Name = "faxCaptionTextBox";
			this.faxCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.faxCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.faxCaptionTextBox.StyleName = "Caption";
			this.faxCaptionTextBox.Value = "Fax:";
			// 
			// faxDataTextBox
			// 
			this.faxDataTextBox.CanGrow = true;
			this.faxDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.664901733398438D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.faxDataTextBox.Name = "faxDataTextBox";
			this.faxDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.faxDataTextBox.StyleName = "Data";
			this.faxDataTextBox.Value = "=Fields.Fax";
			// 
			// wwwCaptionTextBox
			// 
			this.wwwCaptionTextBox.CanGrow = true;
			this.wwwCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.381322860717773D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.wwwCaptionTextBox.Name = "wwwCaptionTextBox";
			this.wwwCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.wwwCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.wwwCaptionTextBox.StyleName = "Caption";
			this.wwwCaptionTextBox.Value = "Www:";
			// 
			// wwwDataTextBox
			// 
			this.wwwDataTextBox.CanGrow = true;
			this.wwwDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.097743034362793D), Telerik.Reporting.Drawing.Unit.Cm(2.0529167652130127D));
			this.wwwDataTextBox.Name = "wwwDataTextBox";
			this.wwwDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66350376605987549D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
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
			this.itemNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.itemNoDataTextBox.StyleName = "Data";
			this.itemNoDataTextBox.Value = "=Fields.ItemNo";
			// 
			// manufacturerDataTextBox
			// 
			this.manufacturerDataTextBox.CanGrow = true;
			this.manufacturerDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.9932291507720947D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.manufacturerDataTextBox.Name = "manufacturerDataTextBox";
			this.manufacturerDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.manufacturerDataTextBox.StyleName = "Data";
			this.manufacturerDataTextBox.Value = "=Fields.Manufacturer";
			// 
			// modelNoDataTextBox
			// 
			this.modelNoDataTextBox.CanGrow = true;
			this.modelNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.modelNoDataTextBox.Name = "modelNoDataTextBox";
			this.modelNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
			this.modelNoDataTextBox.StyleName = "Data";
			this.modelNoDataTextBox.Value = "=Fields.ModelNo";
			// 
			// descriptionDataTextBox
			// 
			this.descriptionDataTextBox.CanGrow = true;
			this.descriptionDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.873854637145996D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
			this.descriptionDataTextBox.Name = "descriptionDataTextBox";
			this.descriptionDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8873958587646484D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
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
			this.Width = Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.SqlDataSource sqlDataSource1;
		private Telerik.Reporting.SqlDataSource sqlDataSource2;
		private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
		private Telerik.Reporting.TextBox itemNoCaptionTextBox;
		private Telerik.Reporting.TextBox manufacturerCaptionTextBox;
		private Telerik.Reporting.TextBox modelNoCaptionTextBox;
		private Telerik.Reporting.TextBox descriptionCaptionTextBox;
		private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
		private Telerik.Reporting.Group labelsGroup;
		private Telerik.Reporting.PageHeaderSection pageHeader;
		private Telerik.Reporting.TextBox reportNameTextBox;
		private Telerik.Reporting.PageFooterSection pageFooter;
		private Telerik.Reporting.TextBox currentTimeTextBox;
		private Telerik.Reporting.TextBox pageInfoTextBox;
		private Telerik.Reporting.ReportHeaderSection reportHeader;
		private Telerik.Reporting.TextBox titleTextBox;
		private Telerik.Reporting.TextBox consignmentNoCaptionTextBox;
		private Telerik.Reporting.TextBox consignmentNoDataTextBox;
		private Telerik.Reporting.TextBox mainLogoCaptionTextBox;
		private Telerik.Reporting.TextBox mainLogoDataTextBox;
		private Telerik.Reporting.TextBox nameCaptionTextBox;
		private Telerik.Reporting.TextBox nameDataTextBox;
		private Telerik.Reporting.TextBox address1CaptionTextBox;
		private Telerik.Reporting.TextBox address1DataTextBox;
		private Telerik.Reporting.TextBox address2CaptionTextBox;
		private Telerik.Reporting.TextBox address2DataTextBox;
		private Telerik.Reporting.TextBox address3CaptionTextBox;
		private Telerik.Reporting.TextBox address3DataTextBox;
		private Telerik.Reporting.TextBox address4CaptionTextBox;
		private Telerik.Reporting.TextBox address4DataTextBox;
		private Telerik.Reporting.TextBox address5CaptionTextBox;
		private Telerik.Reporting.TextBox address5DataTextBox;
		private Telerik.Reporting.TextBox telephoneCaptionTextBox;
		private Telerik.Reporting.TextBox telephoneDataTextBox;
		private Telerik.Reporting.TextBox faxCaptionTextBox;
		private Telerik.Reporting.TextBox faxDataTextBox;
		private Telerik.Reporting.TextBox wwwCaptionTextBox;
		private Telerik.Reporting.TextBox wwwDataTextBox;
		private Telerik.Reporting.ReportFooterSection reportFooter;
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.TextBox itemNoDataTextBox;
		private Telerik.Reporting.TextBox manufacturerDataTextBox;
		private Telerik.Reporting.TextBox modelNoDataTextBox;
		private Telerik.Reporting.TextBox descriptionDataTextBox;

	}
}