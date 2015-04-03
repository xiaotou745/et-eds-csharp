namespace TaskPlatform.Forms
{
    partial class RunningForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunningForm));
            this.lbTip = new System.Windows.Forms.Label();
            this.tmrExit = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // lbTip
            // 
            this.lbTip.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTip.Location = new System.Drawing.Point(0, 0);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(435, 118);
            this.lbTip.TabIndex = 0;
            this.lbTip.Text = "端口为[TaskPlatform]的计划任务已在运行";
            this.lbTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrExit
            // 
            this.tmrExit.Interval = 1000;
            this.tmrExit.Tick += new System.EventHandler(this.tmrExit_Tick);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(99, 134);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "仍要启动";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(260, 134);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出进程";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // RunningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 169);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbTip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RunningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "已在运行";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunningForm_FormClosing);
            this.Load += new System.EventHandler(this.RunningForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrExit;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        public DevExpress.XtraEditors.SimpleButton btnStart;

    }
}