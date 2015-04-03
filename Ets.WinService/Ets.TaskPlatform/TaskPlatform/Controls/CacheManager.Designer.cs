namespace TaskPlatform.Controls
{
    partial class CacheManager
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gpcAllKeys = new DevExpress.XtraEditors.GroupControl();
            this.lstbAllKeys = new DevExpress.XtraEditors.ListBoxControl();
            this.gpcOperations = new DevExpress.XtraEditors.GroupControl();
            this.btnReload = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.gpcAllValues = new DevExpress.XtraEditors.GroupControl();
            this.ppgValue = new System.Windows.Forms.PropertyGrid();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.btnDefault = new DevExpress.XtraEditors.DropDownButton();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gpcItemppv = new DevExpress.XtraEditors.GroupControl();
            this.ppgItemProperty = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.gpcAllKeys)).BeginInit();
            this.gpcAllKeys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstbAllKeys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpcOperations)).BeginInit();
            this.gpcOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpcAllValues)).BeginInit();
            this.gpcAllValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpcItemppv)).BeginInit();
            this.gpcItemppv.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpcAllKeys
            // 
            this.gpcAllKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpcAllKeys.Controls.Add(this.lstbAllKeys);
            this.gpcAllKeys.Location = new System.Drawing.Point(0, 51);
            this.gpcAllKeys.Name = "gpcAllKeys";
            this.gpcAllKeys.Size = new System.Drawing.Size(192, 246);
            this.gpcAllKeys.TabIndex = 0;
            this.gpcAllKeys.Text = "所有键(Key)";
            // 
            // lstbAllKeys
            // 
            this.lstbAllKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstbAllKeys.Location = new System.Drawing.Point(2, 23);
            this.lstbAllKeys.Name = "lstbAllKeys";
            this.lstbAllKeys.Size = new System.Drawing.Size(188, 221);
            this.lstbAllKeys.TabIndex = 0;
            this.lstbAllKeys.SelectedIndexChanged += new System.EventHandler(this.lstbAllKeys_SelectedIndexChanged);
            // 
            // gpcOperations
            // 
            this.gpcOperations.Controls.Add(this.btnReload);
            this.gpcOperations.Controls.Add(this.btnDelete);
            this.gpcOperations.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpcOperations.Location = new System.Drawing.Point(0, 0);
            this.gpcOperations.Name = "gpcOperations";
            this.gpcOperations.Size = new System.Drawing.Size(418, 58);
            this.gpcOperations.TabIndex = 0;
            this.gpcOperations.Text = "操作";
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(38, 26);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(65, 20);
            this.btnReload.TabIndex = 0;
            this.btnReload.Text = "刷  新(&D)";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(135, 26);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 20);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "删  除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gpcAllValues
            // 
            this.gpcAllValues.Controls.Add(this.ppgValue);
            this.gpcAllValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpcAllValues.Location = new System.Drawing.Point(0, 0);
            this.gpcAllValues.Name = "gpcAllValues";
            this.gpcAllValues.Size = new System.Drawing.Size(204, 235);
            this.gpcAllValues.TabIndex = 1;
            this.gpcAllValues.Text = "所有属性(Value)";
            // 
            // ppgValue
            // 
            this.ppgValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgValue.Location = new System.Drawing.Point(2, 23);
            this.ppgValue.Name = "ppgValue";
            this.ppgValue.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ppgValue.Size = new System.Drawing.Size(200, 210);
            this.ppgValue.TabIndex = 0;
            this.ppgValue.ToolbarVisible = false;
            this.ppgValue.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ppgValue_PropertyValueChanged);
            this.ppgValue.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.ppgValue_SelectedGridItemChanged);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.btnDefault);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.gpcAllKeys);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel2.Controls.Add(this.gpcOperations);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(615, 297);
            this.splitContainerControl1.SplitterPosition = 192;
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.DropDownControl = this.popupMenu1;
            this.btnDefault.Location = new System.Drawing.Point(3, 22);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(187, 23);
            this.btnDefault.TabIndex = 2;
            this.btnDefault.Tag = "6DDB8DB2-85FF-41F8-9311-7EF924C389EE";
            this.btnDefault.Text = "默认容器";
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 2;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(615, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 297);
            this.barDockControlBottom.Size = new System.Drawing.Size(615, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 297);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(615, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 297);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "缓存容器：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 58);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(418, 239);
            this.panelControl1.TabIndex = 1;
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gpcAllValues);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.gpcItemppv);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(414, 235);
            this.splitContainerControl2.SplitterPosition = 204;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // gpcItemppv
            // 
            this.gpcItemppv.Controls.Add(this.ppgItemProperty);
            this.gpcItemppv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpcItemppv.Location = new System.Drawing.Point(0, 0);
            this.gpcItemppv.Name = "gpcItemppv";
            this.gpcItemppv.Size = new System.Drawing.Size(205, 235);
            this.gpcItemppv.TabIndex = 2;
            this.gpcItemppv.Text = "的属性";
            // 
            // ppgItemProperty
            // 
            this.ppgItemProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgItemProperty.Location = new System.Drawing.Point(2, 23);
            this.ppgItemProperty.Name = "ppgItemProperty";
            this.ppgItemProperty.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ppgItemProperty.Size = new System.Drawing.Size(201, 210);
            this.ppgItemProperty.TabIndex = 0;
            this.ppgItemProperty.ToolbarVisible = false;
            this.ppgItemProperty.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ppgItemProperty_PropertyValueChanged);
            // 
            // CacheManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "CacheManager";
            this.Size = new System.Drawing.Size(615, 297);
            this.Load += new System.EventHandler(this.CacheManager_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CacheManager_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.gpcAllKeys)).EndInit();
            this.gpcAllKeys.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstbAllKeys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpcOperations)).EndInit();
            this.gpcOperations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpcAllValues)).EndInit();
            this.gpcAllValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpcItemppv)).EndInit();
            this.gpcItemppv.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gpcAllKeys;
        private DevExpress.XtraEditors.ListBoxControl lstbAllKeys;
        private DevExpress.XtraEditors.GroupControl gpcOperations;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.GroupControl gpcAllValues;
        private System.Windows.Forms.PropertyGrid ppgValue;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SimpleButton btnReload;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl gpcItemppv;
        private System.Windows.Forms.PropertyGrid ppgItemProperty;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DropDownButton btnDefault;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
    }
}
