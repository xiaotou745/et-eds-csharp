namespace TaskPlatform.Controls
{
    partial class DeadAlarmManager
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
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TaskCommonKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TaskKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TaskName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AlarmType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ConfigAlarm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribtnConfigAlarm = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.AlarmOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribtnDeleteAlarm = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigAlarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnDeleteAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.gridData;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ribtnDeleteAlarm,
            this.ribtnConfigAlarm});
            this.gridControl.Size = new System.Drawing.Size(489, 321);
            this.gridControl.TabIndex = 1;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridData});
            // 
            // gridData
            // 
            this.gridData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TaskCommonKey,
            this.TaskKey,
            this.TaskName,
            this.AlarmType,
            this.ConfigAlarm,
            this.AlarmOperation});
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
            // AlarmType
            // 
            this.AlarmType.Caption = "警报类型";
            this.AlarmType.FieldName = "AlarmType";
            this.AlarmType.Name = "AlarmType";
            this.AlarmType.OptionsColumn.AllowEdit = false;
            this.AlarmType.Visible = true;
            this.AlarmType.VisibleIndex = 3;
            // 
            // ConfigAlarm
            // 
            this.ConfigAlarm.Caption = "配置警报";
            this.ConfigAlarm.ColumnEdit = this.ribtnConfigAlarm;
            this.ConfigAlarm.FieldName = "ConfigAlarm";
            this.ConfigAlarm.Name = "ConfigAlarm";
            this.ConfigAlarm.Visible = true;
            this.ConfigAlarm.VisibleIndex = 4;
            // 
            // ribtnConfigAlarm
            // 
            this.ribtnConfigAlarm.AutoHeight = false;
            this.ribtnConfigAlarm.Caption = "配置警报";
            this.ribtnConfigAlarm.Name = "ribtnConfigAlarm";
            this.ribtnConfigAlarm.Click += new System.EventHandler(this.ribtnConfigAlarm_Click);
            // 
            // AlarmOperation
            // 
            this.AlarmOperation.Caption = "警报";
            this.AlarmOperation.ColumnEdit = this.ribtnDeleteAlarm;
            this.AlarmOperation.FieldName = "Alarm";
            this.AlarmOperation.Name = "AlarmOperation";
            this.AlarmOperation.Visible = true;
            this.AlarmOperation.VisibleIndex = 5;
            // 
            // ribtnDeleteAlarm
            // 
            this.ribtnDeleteAlarm.AutoHeight = false;
            this.ribtnDeleteAlarm.Caption = "删除警报";
            this.ribtnDeleteAlarm.Name = "ribtnDeleteAlarm";
            this.ribtnDeleteAlarm.Click += new System.EventHandler(this.ribtnDeleteAlarm_Click);
            // 
            // DeadAlarmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl);
            this.Name = "DeadAlarmManager";
            this.Size = new System.Drawing.Size(489, 321);
            this.Load += new System.EventHandler(this.DeadAlarmManager_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DeadAlarmManager_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnConfigAlarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribtnDeleteAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridData;
        private DevExpress.XtraGrid.Columns.GridColumn TaskCommonKey;
        private DevExpress.XtraGrid.Columns.GridColumn TaskKey;
        private DevExpress.XtraGrid.Columns.GridColumn TaskName;
        private DevExpress.XtraGrid.Columns.GridColumn AlarmType;
        private DevExpress.XtraGrid.Columns.GridColumn AlarmOperation;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit ribtnDeleteAlarm;
        private DevExpress.XtraGrid.Columns.GridColumn ConfigAlarm;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit ribtnConfigAlarm;

    }
}
