namespace TaskPlatform.Controls
{
    partial class TaskParameterEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKey = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lueValueType = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.ckbCanChangeValueType = new DevExpress.XtraEditors.CheckEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.grpOptions = new DevExpress.XtraEditors.GroupControl();
            this.txtOptions = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueValueType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbCanChangeValueType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpOptions)).BeginInit();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOptions.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(4, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "参数Key：";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(66, 4);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(277, 21);
            this.txtKey.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(4, 34);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(56, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "值 类 型：";
            // 
            // lueValueType
            // 
            this.lueValueType.Location = new System.Drawing.Point(66, 31);
            this.lueValueType.Name = "lueValueType";
            this.lueValueType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.lueValueType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueValueType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ValueType", "参数值类型")});
            this.lueValueType.Properties.DisplayMember = "ValueType";
            this.lueValueType.Properties.NullText = "[未选择]";
            this.lueValueType.Properties.ValueMember = "ValueTypeKey";
            this.lueValueType.Size = new System.Drawing.Size(277, 21);
            this.lueValueType.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(4, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(56, 14);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "可 修 改：";
            // 
            // ckbCanChangeValueType
            // 
            this.ckbCanChangeValueType.Location = new System.Drawing.Point(66, 58);
            this.ckbCanChangeValueType.Name = "ckbCanChangeValueType";
            this.ckbCanChangeValueType.Properties.Caption = "平台可修改参数值类型";
            this.ckbCanChangeValueType.Size = new System.Drawing.Size(154, 19);
            this.ckbCanChangeValueType.TabIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.ckbCanChangeValueType);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.lueValueType);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.txtKey);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(346, 85);
            this.panelControl1.TabIndex = 4;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtValue);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 85);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(346, 35);
            this.panelControl2.TabIndex = 5;
            // 
            // txtValue
            // 
            this.txtValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtValue.Location = new System.Drawing.Point(66, 6);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(277, 22);
            this.txtValue.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(3, 8);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(56, 14);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "参 数 值：";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.txtOptions);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOptions.Location = new System.Drawing.Point(0, 120);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(346, 101);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.Text = "候选值列表";
            // 
            // txtOptions
            // 
            this.txtOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOptions.Location = new System.Drawing.Point(2, 23);
            this.txtOptions.Name = "txtOptions";
            this.txtOptions.Size = new System.Drawing.Size(342, 76);
            this.txtOptions.TabIndex = 0;
            this.txtOptions.EditValueChanged += new System.EventHandler(this.txtOptions_EditValueChanged);
            // 
            // TaskParameterEditor
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.MaximumSize = new System.Drawing.Size(346, 221);
            this.Name = "TaskParameterEditor";
            this.Size = new System.Drawing.Size(346, 221);
            this.Load += new System.EventHandler(this.TaskParameterEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueValueType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbCanChangeValueType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpOptions)).EndInit();
            this.grpOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtOptions.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtKey;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit lueValueType;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit ckbCanChangeValueType;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.GroupControl grpOptions;
        private DevExpress.XtraEditors.MemoEdit txtOptions;
        private System.Windows.Forms.TextBox txtValue;

    }
}
