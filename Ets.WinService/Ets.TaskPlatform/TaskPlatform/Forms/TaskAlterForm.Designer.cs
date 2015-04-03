namespace TaskPlatform.Forms
{
    partial class TaskAlterForm
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
            this.gpcTask = new DevExpress.XtraEditors.GroupControl();
            this.rbtnMySQL = new System.Windows.Forms.RadioButton();
            this.rbtnSQLServer = new System.Windows.Forms.RadioButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtConnectionString = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDisplayName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.txtSQL = new System.Windows.Forms.RichTextBox();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.txtLuaScript = new System.Windows.Forms.RichTextBox();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gpcTask)).BeginInit();
            this.gpcTask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionString.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpcTask
            // 
            this.gpcTask.Controls.Add(this.rbtnMySQL);
            this.gpcTask.Controls.Add(this.rbtnSQLServer);
            this.gpcTask.Controls.Add(this.labelControl3);
            this.gpcTask.Controls.Add(this.txtConnectionString);
            this.gpcTask.Controls.Add(this.labelControl2);
            this.gpcTask.Controls.Add(this.txtDisplayName);
            this.gpcTask.Controls.Add(this.labelControl1);
            this.gpcTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpcTask.Location = new System.Drawing.Point(0, 0);
            this.gpcTask.Name = "gpcTask";
            this.gpcTask.Size = new System.Drawing.Size(469, 110);
            this.gpcTask.TabIndex = 0;
            this.gpcTask.Text = "任务名称";
            // 
            // rbtnMySQL
            // 
            this.rbtnMySQL.AutoSize = true;
            this.rbtnMySQL.BackColor = System.Drawing.Color.Transparent;
            this.rbtnMySQL.Location = new System.Drawing.Point(170, 59);
            this.rbtnMySQL.Name = "rbtnMySQL";
            this.rbtnMySQL.Size = new System.Drawing.Size(62, 18);
            this.rbtnMySQL.TabIndex = 5;
            this.rbtnMySQL.TabStop = true;
            this.rbtnMySQL.Text = "MySQL";
            this.rbtnMySQL.UseVisualStyleBackColor = false;
            // 
            // rbtnSQLServer
            // 
            this.rbtnSQLServer.AutoSize = true;
            this.rbtnSQLServer.BackColor = System.Drawing.Color.Transparent;
            this.rbtnSQLServer.Checked = true;
            this.rbtnSQLServer.Location = new System.Drawing.Point(78, 59);
            this.rbtnSQLServer.Name = "rbtnSQLServer";
            this.rbtnSQLServer.Size = new System.Drawing.Size(86, 18);
            this.rbtnSQLServer.TabIndex = 5;
            this.rbtnSQLServer.TabStop = true;
            this.rbtnSQLServer.Text = "SQL Server";
            this.rbtnSQLServer.UseVisualStyleBackColor = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "数据类型：";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(78, 84);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(379, 21);
            this.txtConnectionString.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 87);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "数  据 库：";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.Location = new System.Drawing.Point(78, 32);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(379, 21);
            this.txtDisplayName.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "显示名称：";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 116);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(469, 275);
            this.splitContainerControl1.SplitterPosition = 152;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.txtSQL);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(469, 152);
            this.groupControl2.TabIndex = 0;
            this.groupControl2.Text = "执行查询的SQL";
            // 
            // txtSQL
            // 
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Location = new System.Drawing.Point(2, 23);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(465, 127);
            this.txtSQL.TabIndex = 0;
            this.txtSQL.Text = "";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.txtLuaScript);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(469, 118);
            this.groupControl3.TabIndex = 0;
            this.groupControl3.Text = "逻辑脚本";
            // 
            // txtLuaScript
            // 
            this.txtLuaScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLuaScript.Location = new System.Drawing.Point(2, 23);
            this.txtLuaScript.Name = "txtLuaScript";
            this.txtLuaScript.Size = new System.Drawing.Size(465, 93);
            this.txtLuaScript.TabIndex = 0;
            this.txtLuaScript.Text = "";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(116, 397);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定(&O)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancle.Location = new System.Drawing.Point(278, 397);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消(&C)";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // TaskAlterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 429);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.gpcTask);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskAlterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改任务";
            this.Load += new System.EventHandler(this.TaskAlterForm_Load);
            this.SizeChanged += new System.EventHandler(this.TaskAlterForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.gpcTask)).EndInit();
            this.gpcTask.ResumeLayout(false);
            this.gpcTask.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionString.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gpcTask;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDisplayName;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private System.Windows.Forms.RichTextBox txtSQL;
        private System.Windows.Forms.RichTextBox txtLuaScript;
        private DevExpress.XtraEditors.TextEdit txtConnectionString;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private System.Windows.Forms.RadioButton rbtnMySQL;
        private System.Windows.Forms.RadioButton rbtnSQLServer;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}