namespace TaskPlatform.Forms
{
    partial class PlanConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanConfigForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.ckbType = new DevExpress.XtraEditors.CheckEdit();
            this.ckbEnable = new DevExpress.XtraEditors.CheckEdit();
            this.lbOnce = new DevExpress.XtraEditors.LabelControl();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineEnable = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineContinue = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineSpliter = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineRepeat = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineOnce = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lbOnceDate = new DevExpress.XtraEditors.LabelControl();
            this.deDateOnce = new DevExpress.XtraEditors.DateEdit();
            this.teTimeOnce = new DevExpress.XtraEditors.TimeEdit();
            this.lbOnceTime = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lbRepeat = new DevExpress.XtraEditors.LabelControl();
            this.lbSpan = new DevExpress.XtraEditors.LabelControl();
            this.seSpan = new DevExpress.XtraEditors.SpinEdit();
            this.lbRun = new DevExpress.XtraEditors.LabelControl();
            this.cmbPlanUnit = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.txtPlanName = new DevExpress.XtraEditors.TextEdit();
            this.lbContinue = new DevExpress.XtraEditors.LabelControl();
            this.lbStart = new DevExpress.XtraEditors.LabelControl();
            this.lbEnd = new DevExpress.XtraEditors.LabelControl();
            this.deStart = new DevExpress.XtraEditors.DateEdit();
            this.teStart = new DevExpress.XtraEditors.TimeEdit();
            this.teEnd = new DevExpress.XtraEditors.TimeEdit();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.ckbNoOver = new DevExpress.XtraEditors.CheckEdit();
            this.lbEnable = new DevExpress.XtraEditors.LabelControl();
            this.lbEnableStart = new DevExpress.XtraEditors.LabelControl();
            this.teStartEnableTime = new DevExpress.XtraEditors.TimeEdit();
            this.lbEnableEnd = new DevExpress.XtraEditors.LabelControl();
            this.teEndEnableTime = new DevExpress.XtraEditors.TimeEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDateOnce.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDateOnce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTimeOnce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seSpan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlanUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbNoOver.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStartEnableTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndEnableTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "计划名称：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(249, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 16;
            this.labelControl3.Text = "计划类型：";
            // 
            // ckbType
            // 
            this.ckbType.EditValue = true;
            this.ckbType.Location = new System.Drawing.Point(315, 9);
            this.ckbType.Name = "ckbType";
            this.ckbType.Properties.Caption = "重复执行";
            this.ckbType.Size = new System.Drawing.Size(75, 19);
            toolTipTitleItem1.Appearance.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Appearance.Options.UseImage = true;
            toolTipTitleItem1.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Text = "计划类型";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "      指示计划是只执行一次，还是重复执行。";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.ckbType.SuperTip = superToolTip1;
            this.ckbType.TabIndex = 4;
            this.ckbType.CheckedChanged += new System.EventHandler(this.ckbType_CheckedChanged);
            // 
            // ckbEnable
            // 
            this.ckbEnable.EditValue = true;
            this.ckbEnable.Location = new System.Drawing.Point(407, 9);
            this.ckbEnable.Name = "ckbEnable";
            this.ckbEnable.Properties.Caption = "已启用";
            this.ckbEnable.Size = new System.Drawing.Size(75, 19);
            toolTipTitleItem2.Appearance.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem2.Appearance.Options.UseImage = true;
            toolTipTitleItem2.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem2.Text = "是否启用";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "    指示计划是否启用。";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.ckbEnable.SuperTip = superToolTip2;
            this.ckbEnable.TabIndex = 5;
            // 
            // lbOnce
            // 
            this.lbOnce.Enabled = false;
            this.lbOnce.Location = new System.Drawing.Point(12, 43);
            this.lbOnce.Name = "lbOnce";
            this.lbOnce.Size = new System.Drawing.Size(48, 14);
            this.lbOnce.TabIndex = 17;
            this.lbOnce.Text = "执行一次";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineEnable,
            this.lineContinue,
            this.lineSpliter,
            this.lineRepeat,
            this.lineOnce});
            this.shapeContainer1.Size = new System.Drawing.Size(498, 329);
            this.shapeContainer1.TabIndex = 14;
            this.shapeContainer1.TabStop = false;
            // 
            // lineEnable
            // 
            this.lineEnable.Cursor = System.Windows.Forms.Cursors.Default;
            this.lineEnable.Name = "lineEnable";
            this.lineEnable.X1 = 63;
            this.lineEnable.X2 = 494;
            this.lineEnable.Y1 = 241;
            this.lineEnable.Y2 = 241;
            // 
            // lineContinue
            // 
            this.lineContinue.Cursor = System.Windows.Forms.Cursors.Default;
            this.lineContinue.Enabled = false;
            this.lineContinue.Name = "lineContinue";
            this.lineContinue.X1 = 63;
            this.lineContinue.X2 = 494;
            this.lineContinue.Y1 = 166;
            this.lineContinue.Y2 = 166;
            // 
            // lineSpliter
            // 
            this.lineSpliter.Cursor = System.Windows.Forms.Cursors.Default;
            this.lineSpliter.Enabled = false;
            this.lineSpliter.Name = "lineSpliter";
            this.lineSpliter.X1 = -1;
            this.lineSpliter.X2 = 499;
            this.lineSpliter.Y1 = 284;
            this.lineSpliter.Y2 = 284;
            // 
            // lineRepeat
            // 
            this.lineRepeat.Cursor = System.Windows.Forms.Cursors.Default;
            this.lineRepeat.Enabled = false;
            this.lineRepeat.Name = "lineRepeat";
            this.lineRepeat.X1 = 63;
            this.lineRepeat.X2 = 494;
            this.lineRepeat.Y1 = 103;
            this.lineRepeat.Y2 = 103;
            // 
            // lineOnce
            // 
            this.lineOnce.BorderColor = System.Drawing.SystemColors.GrayText;
            this.lineOnce.Enabled = false;
            this.lineOnce.Name = "lineOnce";
            this.lineOnce.X1 = 63;
            this.lineOnce.X2 = 494;
            this.lineOnce.Y1 = 51;
            this.lineOnce.Y2 = 51;
            // 
            // lbOnceDate
            // 
            this.lbOnceDate.Enabled = false;
            this.lbOnceDate.Location = new System.Drawing.Point(72, 69);
            this.lbOnceDate.Name = "lbOnceDate";
            this.lbOnceDate.Size = new System.Drawing.Size(60, 14);
            this.lbOnceDate.TabIndex = 18;
            this.lbOnceDate.Text = "执行日期：";
            // 
            // deDateOnce
            // 
            this.deDateOnce.EditValue = new System.DateTime(2012, 1, 13, 12, 48, 17, 0);
            this.deDateOnce.Enabled = false;
            this.deDateOnce.Location = new System.Drawing.Point(138, 66);
            this.deDateOnce.Name = "deDateOnce";
            this.deDateOnce.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deDateOnce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deDateOnce.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deDateOnce.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deDateOnce.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deDateOnce.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deDateOnce.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deDateOnce.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deDateOnce.Size = new System.Drawing.Size(117, 21);
            this.deDateOnce.TabIndex = 2;
            // 
            // teTimeOnce
            // 
            this.teTimeOnce.EditValue = new System.DateTime(2011, 12, 24, 0, 0, 0, 0);
            this.teTimeOnce.Enabled = false;
            this.teTimeOnce.Location = new System.Drawing.Point(327, 66);
            this.teTimeOnce.Name = "teTimeOnce";
            this.teTimeOnce.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.teTimeOnce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teTimeOnce.Size = new System.Drawing.Size(100, 21);
            this.teTimeOnce.TabIndex = 3;
            // 
            // lbOnceTime
            // 
            this.lbOnceTime.Location = new System.Drawing.Point(261, 69);
            this.lbOnceTime.Name = "lbOnceTime";
            this.lbOnceTime.Size = new System.Drawing.Size(60, 14);
            this.lbOnceTime.TabIndex = 19;
            this.lbOnceTime.Text = "执行时间：";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(140, 295);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确  定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(261, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取  消";
            // 
            // lbRepeat
            // 
            this.lbRepeat.Location = new System.Drawing.Point(12, 95);
            this.lbRepeat.Name = "lbRepeat";
            this.lbRepeat.Size = new System.Drawing.Size(48, 14);
            this.lbRepeat.TabIndex = 20;
            this.lbRepeat.Text = "重复执行";
            // 
            // lbSpan
            // 
            this.lbSpan.Location = new System.Drawing.Point(142, 129);
            this.lbSpan.Name = "lbSpan";
            this.lbSpan.Size = new System.Drawing.Size(36, 14);
            this.lbSpan.TabIndex = 21;
            this.lbSpan.Text = "每隔：";
            // 
            // seSpan
            // 
            this.seSpan.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.seSpan.Location = new System.Drawing.Point(184, 126);
            this.seSpan.Name = "seSpan";
            this.seSpan.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.seSpan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seSpan.Properties.IsFloatValue = false;
            this.seSpan.Properties.Mask.EditMask = "N00";
            this.seSpan.Properties.MaxValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.seSpan.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seSpan.Properties.NullText = "1";
            this.seSpan.Properties.ValidateOnEnterKey = true;
            this.seSpan.Size = new System.Drawing.Size(71, 21);
            this.seSpan.TabIndex = 0;
            // 
            // lbRun
            // 
            this.lbRun.Location = new System.Drawing.Point(320, 129);
            this.lbRun.Name = "lbRun";
            this.lbRun.Size = new System.Drawing.Size(36, 14);
            this.lbRun.TabIndex = 22;
            this.lbRun.Text = "执行。";
            // 
            // cmbPlanUnit
            // 
            this.cmbPlanUnit.Location = new System.Drawing.Point(261, 126);
            this.cmbPlanUnit.Name = "cmbPlanUnit";
            this.cmbPlanUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPlanUnit.Size = new System.Drawing.Size(53, 21);
            this.cmbPlanUnit.TabIndex = 1;
            // 
            // txtPlanName
            // 
            this.txtPlanName.Location = new System.Drawing.Point(72, 9);
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtPlanName.Properties.Appearance.Options.UseBackColor = true;
            this.txtPlanName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtPlanName.Properties.ReadOnly = true;
            this.txtPlanName.Size = new System.Drawing.Size(171, 19);
            this.txtPlanName.TabIndex = 13;
            this.txtPlanName.TabStop = false;
            // 
            // lbContinue
            // 
            this.lbContinue.Location = new System.Drawing.Point(12, 159);
            this.lbContinue.Name = "lbContinue";
            this.lbContinue.Size = new System.Drawing.Size(48, 14);
            this.lbContinue.TabIndex = 23;
            this.lbContinue.Text = "持续时间";
            // 
            // lbStart
            // 
            this.lbStart.Location = new System.Drawing.Point(72, 183);
            this.lbStart.Name = "lbStart";
            this.lbStart.Size = new System.Drawing.Size(60, 14);
            this.lbStart.TabIndex = 24;
            this.lbStart.Text = "开始时间：";
            // 
            // lbEnd
            // 
            this.lbEnd.Location = new System.Drawing.Point(72, 212);
            this.lbEnd.Name = "lbEnd";
            this.lbEnd.Size = new System.Drawing.Size(60, 14);
            this.lbEnd.TabIndex = 25;
            this.lbEnd.Text = "结束时间：";
            // 
            // deStart
            // 
            this.deStart.EditValue = new System.DateTime(2012, 1, 13, 12, 48, 57, 0);
            this.deStart.Enabled = false;
            this.deStart.Location = new System.Drawing.Point(138, 180);
            this.deStart.Name = "deStart";
            this.deStart.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStart.Size = new System.Drawing.Size(117, 21);
            this.deStart.TabIndex = 6;
            this.deStart.EnabledChanged += new System.EventHandler(this.deStart_EnabledChanged);
            // 
            // teStart
            // 
            this.teStart.EditValue = new System.DateTime(2011, 12, 24, 0, 0, 0, 0);
            this.teStart.Enabled = false;
            this.teStart.Location = new System.Drawing.Point(261, 180);
            this.teStart.Name = "teStart";
            this.teStart.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.teStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teStart.Size = new System.Drawing.Size(100, 21);
            this.teStart.TabIndex = 7;
            // 
            // teEnd
            // 
            this.teEnd.EditValue = new System.DateTime(2011, 12, 24, 0, 0, 0, 0);
            this.teEnd.Enabled = false;
            this.teEnd.Location = new System.Drawing.Point(261, 209);
            this.teEnd.Name = "teEnd";
            this.teEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.teEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teEnd.Size = new System.Drawing.Size(100, 21);
            this.teEnd.TabIndex = 10;
            // 
            // deEnd
            // 
            this.deEnd.EditValue = new System.DateTime(2012, 1, 13, 12, 49, 2, 0);
            this.deEnd.Enabled = false;
            this.deEnd.Location = new System.Drawing.Point(138, 209);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Size = new System.Drawing.Size(117, 21);
            this.deEnd.TabIndex = 9;
            // 
            // ckbNoOver
            // 
            this.ckbNoOver.EditValue = true;
            this.ckbNoOver.Enabled = false;
            this.ckbNoOver.Location = new System.Drawing.Point(367, 209);
            this.ckbNoOver.Name = "ckbNoOver";
            this.ckbNoOver.Properties.Caption = "无结束时间";
            this.ckbNoOver.Size = new System.Drawing.Size(89, 19);
            this.ckbNoOver.TabIndex = 8;
            this.ckbNoOver.CheckedChanged += new System.EventHandler(this.ckbNoOver_CheckedChanged);
            // 
            // lbEnable
            // 
            this.lbEnable.Location = new System.Drawing.Point(12, 234);
            this.lbEnable.Name = "lbEnable";
            this.lbEnable.Size = new System.Drawing.Size(48, 14);
            this.lbEnable.TabIndex = 23;
            this.lbEnable.Text = "启用时间";
            // 
            // lbEnableStart
            // 
            this.lbEnableStart.Location = new System.Drawing.Point(72, 258);
            this.lbEnableStart.Name = "lbEnableStart";
            this.lbEnableStart.Size = new System.Drawing.Size(60, 14);
            this.lbEnableStart.TabIndex = 27;
            this.lbEnableStart.Text = "开始时间：";
            // 
            // teStartEnableTime
            // 
            this.teStartEnableTime.EditValue = new System.DateTime(2011, 12, 24, 0, 0, 0, 0);
            this.teStartEnableTime.Enabled = false;
            this.teStartEnableTime.Location = new System.Drawing.Point(138, 255);
            this.teStartEnableTime.Name = "teStartEnableTime";
            this.teStartEnableTime.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.teStartEnableTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teStartEnableTime.Size = new System.Drawing.Size(105, 21);
            this.teStartEnableTime.TabIndex = 26;
            // 
            // lbEnableEnd
            // 
            this.lbEnableEnd.Location = new System.Drawing.Point(249, 258);
            this.lbEnableEnd.Name = "lbEnableEnd";
            this.lbEnableEnd.Size = new System.Drawing.Size(60, 14);
            this.lbEnableEnd.TabIndex = 29;
            this.lbEnableEnd.Text = "结束时间：";
            // 
            // teEndEnableTime
            // 
            this.teEndEnableTime.EditValue = new System.DateTime(2011, 12, 24, 0, 0, 0, 0);
            this.teEndEnableTime.Enabled = false;
            this.teEndEnableTime.Location = new System.Drawing.Point(315, 255);
            this.teEndEnableTime.Name = "teEndEnableTime";
            this.teEndEnableTime.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.teEndEnableTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teEndEnableTime.Size = new System.Drawing.Size(105, 21);
            this.teEndEnableTime.TabIndex = 28;
            // 
            // PlanConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(498, 329);
            this.Controls.Add(this.lbEnableEnd);
            this.Controls.Add(this.teEndEnableTime);
            this.Controls.Add(this.lbEnableStart);
            this.Controls.Add(this.teStartEnableTime);
            this.Controls.Add(this.lbEnable);
            this.Controls.Add(this.ckbNoOver);
            this.Controls.Add(this.teEnd);
            this.Controls.Add(this.deEnd);
            this.Controls.Add(this.teStart);
            this.Controls.Add(this.deStart);
            this.Controls.Add(this.lbEnd);
            this.Controls.Add(this.lbStart);
            this.Controls.Add(this.lbContinue);
            this.Controls.Add(this.txtPlanName);
            this.Controls.Add(this.cmbPlanUnit);
            this.Controls.Add(this.lbRun);
            this.Controls.Add(this.seSpan);
            this.Controls.Add(this.lbSpan);
            this.Controls.Add(this.lbRepeat);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbOnceTime);
            this.Controls.Add(this.teTimeOnce);
            this.Controls.Add(this.deDateOnce);
            this.Controls.Add(this.lbOnceDate);
            this.Controls.Add(this.lbOnce);
            this.Controls.Add(this.ckbEnable);
            this.Controls.Add(this.ckbType);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlanConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置计划";
            this.Load += new System.EventHandler(this.PlanConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ckbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDateOnce.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDateOnce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTimeOnce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seSpan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlanUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlanName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbNoOver.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStartEnableTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndEnableTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit ckbType;
        private DevExpress.XtraEditors.CheckEdit ckbEnable;
        private DevExpress.XtraEditors.LabelControl lbOnce;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineOnce;
        private DevExpress.XtraEditors.LabelControl lbOnceDate;
        private DevExpress.XtraEditors.DateEdit deDateOnce;
        private DevExpress.XtraEditors.TimeEdit teTimeOnce;
        private DevExpress.XtraEditors.LabelControl lbOnceTime;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineRepeat;
        private DevExpress.XtraEditors.LabelControl lbRepeat;
        private DevExpress.XtraEditors.LabelControl lbSpan;
        private DevExpress.XtraEditors.SpinEdit seSpan;
        private DevExpress.XtraEditors.LabelControl lbRun;
        private DevExpress.XtraEditors.ImageComboBoxEdit cmbPlanUnit;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineSpliter;
        private DevExpress.XtraEditors.TextEdit txtPlanName;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineContinue;
        private DevExpress.XtraEditors.LabelControl lbContinue;
        private DevExpress.XtraEditors.LabelControl lbStart;
        private DevExpress.XtraEditors.LabelControl lbEnd;
        private DevExpress.XtraEditors.DateEdit deStart;
        private DevExpress.XtraEditors.TimeEdit teStart;
        private DevExpress.XtraEditors.TimeEdit teEnd;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.CheckEdit ckbNoOver;
        private DevExpress.XtraEditors.LabelControl lbEnable;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineEnable;
        private DevExpress.XtraEditors.LabelControl lbEnableStart;
        private DevExpress.XtraEditors.TimeEdit teStartEnableTime;
        private DevExpress.XtraEditors.LabelControl lbEnableEnd;
        private DevExpress.XtraEditors.TimeEdit teEndEnableTime;
    }
}