namespace TaskPlatform.Controls
{
    partial class TasksManager
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            this.gpcQuery = new DevExpress.XtraEditors.GroupControl();
            this.btnAllAlarmConfig = new DevExpress.XtraEditors.SimpleButton();
            this.txtTaskKey = new DevExpress.XtraEditors.TextEdit();
            this.txtTaskName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lbLastRunInfo = new DevExpress.XtraEditors.LabelControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnReBuild = new DevExpress.XtraEditors.SimpleButton();
            this.btnAlter = new DevExpress.XtraEditors.SimpleButton();
            this.btnCreateTask = new DevExpress.XtraEditors.SimpleButton();
            this.gpcTasks = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TaskCommonKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TaskKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TaskName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TimeOut = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AlarmType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TaskDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PlanOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribtnConfigPlan = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.AlarmOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribtnConfigAlarm = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.OpenOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribtnOpen = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.FileName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnExportToExcel = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportToPDF = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportToHtml = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportToMht = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportToRtf = new DevExpress.XtraBars.BarButtonItem();
            this.btnOpenDir = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gpcQuery)).BeginInit();
            this.gpcQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpcTasks)).BeginInit();
            this.gpcTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigAlarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // gpcQuery
            // 
            this.gpcQuery.Controls.Add(this.btnAllAlarmConfig);
            this.gpcQuery.Controls.Add(this.txtTaskKey);
            this.gpcQuery.Controls.Add(this.txtTaskName);
            this.gpcQuery.Controls.Add(this.labelControl1);
            this.gpcQuery.Controls.Add(this.lbLastRunInfo);
            this.gpcQuery.Controls.Add(this.btnRefresh);
            this.gpcQuery.Controls.Add(this.btnReBuild);
            this.gpcQuery.Controls.Add(this.btnAlter);
            this.gpcQuery.Controls.Add(this.btnCreateTask);
            this.gpcQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpcQuery.Location = new System.Drawing.Point(0, 0);
            this.gpcQuery.Name = "gpcQuery";
            this.gpcQuery.Size = new System.Drawing.Size(427, 79);
            this.gpcQuery.TabIndex = 0;
            this.gpcQuery.Text = "查询区";
            // 
            // btnAllAlarmConfig
            // 
            this.btnAllAlarmConfig.Location = new System.Drawing.Point(309, 26);
            this.btnAllAlarmConfig.Name = "btnAllAlarmConfig";
            this.btnAllAlarmConfig.Size = new System.Drawing.Size(96, 20);
            toolTipTitleItem1.Appearance.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Appearance.Options.UseImage = true;
            toolTipTitleItem1.Image = global::TaskPlatform.Properties.Resources.Messages;
            toolTipTitleItem1.Text = "配置所有警报";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "所配置的参数将应用到所有的计划任务上。";
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = "建议：初始化报警时使用。";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipSeparatorItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.btnAllAlarmConfig.SuperTip = superToolTip1;
            this.btnAllAlarmConfig.TabIndex = 15;
            this.btnAllAlarmConfig.Text = "配置所有警报";
            this.btnAllAlarmConfig.Click += new System.EventHandler(this.btnAllAlarmConfig_Click);
            // 
            // txtTaskKey
            // 
            this.txtTaskKey.Location = new System.Drawing.Point(279, 50);
            this.txtTaskKey.Name = "txtTaskKey";
            this.txtTaskKey.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtTaskKey.Properties.Appearance.Options.UseBackColor = true;
            this.txtTaskKey.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTaskKey.Size = new System.Drawing.Size(138, 19);
            this.txtTaskKey.TabIndex = 14;
            this.txtTaskKey.TabStop = false;
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(73, 50);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtTaskName.Properties.Appearance.Options.UseBackColor = true;
            this.txtTaskName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTaskName.Size = new System.Drawing.Size(138, 19);
            this.txtTaskName.TabIndex = 14;
            this.txtTaskName.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(217, 52);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "任务Key：";
            // 
            // lbLastRunInfo
            // 
            this.lbLastRunInfo.Location = new System.Drawing.Point(7, 52);
            this.lbLastRunInfo.Name = "lbLastRunInfo";
            this.lbLastRunInfo.Size = new System.Drawing.Size(60, 14);
            this.lbLastRunInfo.TabIndex = 4;
            this.lbLastRunInfo.Text = "任务名称：";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(233, 26);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(65, 20);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新任务";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnReBuild
            // 
            this.btnReBuild.Location = new System.Drawing.Point(157, 26);
            this.btnReBuild.Name = "btnReBuild";
            this.btnReBuild.Size = new System.Drawing.Size(65, 20);
            this.btnReBuild.TabIndex = 0;
            this.btnReBuild.Text = "重建任务";
            this.btnReBuild.Click += new System.EventHandler(this.btnReBuild_Click);
            // 
            // btnAlter
            // 
            this.btnAlter.Location = new System.Drawing.Point(81, 26);
            this.btnAlter.Name = "btnAlter";
            this.btnAlter.Size = new System.Drawing.Size(65, 20);
            this.btnAlter.TabIndex = 0;
            this.btnAlter.Text = "修改任务";
            this.btnAlter.Click += new System.EventHandler(this.btnAlter_Click);
            // 
            // btnCreateTask
            // 
            this.btnCreateTask.Location = new System.Drawing.Point(5, 26);
            this.btnCreateTask.Name = "btnCreateTask";
            this.btnCreateTask.Size = new System.Drawing.Size(65, 20);
            this.btnCreateTask.TabIndex = 0;
            this.btnCreateTask.Text = "创建任务";
            this.btnCreateTask.Click += new System.EventHandler(this.btnCreateTask_Click);
            // 
            // gpcTasks
            // 
            this.gpcTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpcTasks.Controls.Add(this.labelControl2);
            this.gpcTasks.Controls.Add(this.gridControl);
            this.gpcTasks.Location = new System.Drawing.Point(0, 85);
            this.gpcTasks.Name = "gpcTasks";
            this.gpcTasks.Size = new System.Drawing.Size(427, 175);
            this.gpcTasks.TabIndex = 1;
            this.gpcTasks.Text = "计划任务列表";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(91, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "表格右键可操作";
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(2, 23);
            this.gridControl.MainView = this.gridData;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ribtnConfigPlan,
            this.ribtnConfigAlarm,
            this.ribtnOpen});
            this.gridControl.Size = new System.Drawing.Size(423, 150);
            this.gridControl.TabIndex = 0;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridData});
            this.gridControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridControl_MouseClick);
            // 
            // gridData
            // 
            this.gridData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TaskCommonKey,
            this.TaskKey,
            this.TaskName,
            this.Status,
            this.TimeOut,
            this.AlarmType,
            this.TaskDescription,
            this.PlanOperation,
            this.AlarmOperation,
            this.OpenOperation,
            this.FileName});
            this.gridData.GridControl = this.gridControl;
            this.gridData.GroupPanelText = "拖动列头至此以进行分组";
            this.gridData.Name = "gridData";
            // 
            // TaskCommonKey
            // 
            this.TaskCommonKey.Caption = "通用Key";
            this.TaskCommonKey.FieldName = "TaskCommonKey";
            this.TaskCommonKey.Name = "TaskCommonKey";
            this.TaskCommonKey.OptionsColumn.AllowEdit = false;
            this.TaskCommonKey.Visible = true;
            this.TaskCommonKey.VisibleIndex = 0;
            // 
            // TaskKey
            // 
            this.TaskKey.Caption = "任务Key";
            this.TaskKey.FieldName = "TaskKey";
            this.TaskKey.Name = "TaskKey";
            this.TaskKey.OptionsColumn.AllowEdit = false;
            this.TaskKey.Visible = true;
            this.TaskKey.VisibleIndex = 1;
            // 
            // TaskName
            // 
            this.TaskName.Caption = "任务名称";
            this.TaskName.FieldName = "TaskName";
            this.TaskName.Name = "TaskName";
            this.TaskName.OptionsColumn.AllowEdit = false;
            this.TaskName.Visible = true;
            this.TaskName.VisibleIndex = 2;
            // 
            // Status
            // 
            this.Status.Caption = "运行状态";
            this.Status.FieldName = "Status";
            this.Status.Name = "Status";
            this.Status.OptionsColumn.AllowEdit = false;
            this.Status.Visible = true;
            this.Status.VisibleIndex = 3;
            // 
            // TimeOut
            // 
            this.TimeOut.Caption = "当前超时限制";
            this.TimeOut.FieldName = "TimeOut";
            this.TimeOut.Name = "TimeOut";
            this.TimeOut.OptionsColumn.AllowEdit = false;
            this.TimeOut.Visible = true;
            this.TimeOut.VisibleIndex = 4;
            // 
            // AlarmType
            // 
            this.AlarmType.Caption = "警报类型";
            this.AlarmType.FieldName = "AlarmType";
            this.AlarmType.Name = "AlarmType";
            this.AlarmType.OptionsColumn.AllowEdit = false;
            this.AlarmType.Visible = true;
            this.AlarmType.VisibleIndex = 5;
            // 
            // TaskDescription
            // 
            this.TaskDescription.Caption = "任务描述";
            this.TaskDescription.FieldName = "TaskDescription";
            this.TaskDescription.Name = "TaskDescription";
            this.TaskDescription.OptionsColumn.AllowEdit = false;
            this.TaskDescription.Visible = true;
            this.TaskDescription.VisibleIndex = 6;
            // 
            // PlanOperation
            // 
            this.PlanOperation.Caption = "计划";
            this.PlanOperation.ColumnEdit = this.ribtnConfigPlan;
            this.PlanOperation.FieldName = "Plan";
            this.PlanOperation.Name = "PlanOperation";
            this.PlanOperation.Visible = true;
            this.PlanOperation.VisibleIndex = 7;
            // 
            // ribtnConfigPlan
            // 
            this.ribtnConfigPlan.AutoHeight = false;
            this.ribtnConfigPlan.Caption = "配置计划";
            this.ribtnConfigPlan.Name = "ribtnConfigPlan";
            this.ribtnConfigPlan.Click += new System.EventHandler(this.ribtnConfigPlan_Click);
            // 
            // AlarmOperation
            // 
            this.AlarmOperation.Caption = "警报";
            this.AlarmOperation.ColumnEdit = this.ribtnConfigAlarm;
            this.AlarmOperation.FieldName = "Alarm";
            this.AlarmOperation.Name = "AlarmOperation";
            this.AlarmOperation.Visible = true;
            this.AlarmOperation.VisibleIndex = 8;
            // 
            // ribtnConfigAlarm
            // 
            this.ribtnConfigAlarm.AutoHeight = false;
            this.ribtnConfigAlarm.Caption = "配置警报";
            this.ribtnConfigAlarm.Name = "ribtnConfigAlarm";
            this.ribtnConfigAlarm.Click += new System.EventHandler(this.ribtnConfigAlarm_Click);
            // 
            // OpenOperation
            // 
            this.OpenOperation.Caption = "打开";
            this.OpenOperation.ColumnEdit = this.ribtnOpen;
            this.OpenOperation.FieldName = "Open";
            this.OpenOperation.Name = "OpenOperation";
            this.OpenOperation.Visible = true;
            this.OpenOperation.VisibleIndex = 9;
            // 
            // ribtnOpen
            // 
            this.ribtnOpen.AutoHeight = false;
            this.ribtnOpen.Name = "ribtnOpen";
            this.ribtnOpen.Click += new System.EventHandler(this.ribtnOpen_Click);
            // 
            // FileName
            // 
            this.FileName.Caption = "文件路径";
            this.FileName.FieldName = "FileName";
            this.FileName.Name = "FileName";
            this.FileName.OptionsColumn.AllowEdit = false;
            this.FileName.Visible = true;
            this.FileName.VisibleIndex = 10;
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExportToExcel,
            this.btnExportToPDF,
            this.btnExportToHtml,
            this.btnExportToMht,
            this.btnExportToRtf,
            this.btnOpenDir});
            this.barManager.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(427, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 263);
            this.barDockControlBottom.Size = new System.Drawing.Size(427, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 263);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(427, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 263);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Caption = "导出到Excel";
            this.btnExportToExcel.Id = 0;
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToExcel_ItemClick);
            // 
            // btnExportToPDF
            // 
            this.btnExportToPDF.Caption = "导出到PDF";
            this.btnExportToPDF.Id = 1;
            this.btnExportToPDF.Name = "btnExportToPDF";
            this.btnExportToPDF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToPDF_ItemClick);
            // 
            // btnExportToHtml
            // 
            this.btnExportToHtml.Caption = "导出到Html";
            this.btnExportToHtml.Id = 2;
            this.btnExportToHtml.Name = "btnExportToHtml";
            this.btnExportToHtml.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToHtml_ItemClick);
            // 
            // btnExportToMht
            // 
            this.btnExportToMht.Caption = "导出到Mht";
            this.btnExportToMht.Id = 3;
            this.btnExportToMht.Name = "btnExportToMht";
            this.btnExportToMht.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToMht_ItemClick);
            // 
            // btnExportToRtf
            // 
            this.btnExportToRtf.Caption = "导出到Rtf";
            this.btnExportToRtf.Id = 4;
            this.btnExportToRtf.Name = "btnExportToRtf";
            this.btnExportToRtf.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToRtf_ItemClick);
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Caption = "打开计划任务目录";
            this.btnOpenDir.Id = 5;
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenDir_ItemClick);
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnOpenDir),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.btnExportToExcel, "导出到Excel", true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToPDF),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToHtml),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToMht),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToRtf)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Title = "请选择保存文件的路径";
            // 
            // TasksManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpcTasks);
            this.Controls.Add(this.gpcQuery);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "TasksManager";
            this.Size = new System.Drawing.Size(427, 263);
            this.Load += new System.EventHandler(this.TasksManager_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TasksManager_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.gpcQuery)).EndInit();
            this.gpcQuery.ResumeLayout(false);
            this.gpcQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpcTasks)).EndInit();
            this.gpcTasks.ResumeLayout(false);
            this.gpcTasks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigAlarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gpcQuery;
        private DevExpress.XtraEditors.GroupControl gpcTasks;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridData;
        private DevExpress.XtraEditors.SimpleButton btnCreateTask;
        private DevExpress.XtraEditors.SimpleButton btnAlter;
        private DevExpress.XtraEditors.SimpleButton btnReBuild;
        private DevExpress.XtraEditors.LabelControl lbLastRunInfo;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn TaskKey;
        private DevExpress.XtraGrid.Columns.GridColumn TaskName;
        private DevExpress.XtraGrid.Columns.GridColumn Status;
        private DevExpress.XtraGrid.Columns.GridColumn TaskDescription;
        private DevExpress.XtraGrid.Columns.GridColumn TaskCommonKey;
        private DevExpress.XtraGrid.Columns.GridColumn PlanOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit ribtnConfigPlan;
        private DevExpress.XtraGrid.Columns.GridColumn AlarmOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit ribtnConfigAlarm;
        private DevExpress.XtraEditors.TextEdit txtTaskKey;
        private DevExpress.XtraEditors.TextEdit txtTaskName;
        private DevExpress.XtraGrid.Columns.GridColumn TimeOut;
        private DevExpress.XtraEditors.SimpleButton btnAllAlarmConfig;
        private DevExpress.XtraGrid.Columns.GridColumn AlarmType;
        private DevExpress.XtraGrid.Columns.GridColumn OpenOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit ribtnOpen;
        private DevExpress.XtraGrid.Columns.GridColumn FileName;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarButtonItem btnExportToExcel;
        private DevExpress.XtraBars.BarButtonItem btnExportToPDF;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private DevExpress.XtraBars.BarButtonItem btnExportToHtml;
        private DevExpress.XtraBars.BarButtonItem btnExportToMht;
        private DevExpress.XtraBars.BarButtonItem btnExportToRtf;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraBars.BarButtonItem btnOpenDir;
    }
}
