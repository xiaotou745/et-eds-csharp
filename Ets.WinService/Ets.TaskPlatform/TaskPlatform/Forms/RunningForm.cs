using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace TaskPlatform.Forms
{
    public partial class RunningForm : XtraForm
    {
        public RunningForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 文本模板
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// 文本模板关键字
        /// </summary>
        public string TemplateKeywords { get; set; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public string TipText { get; set; }
        private void RunningForm_Load(object sender, EventArgs e)
        {
            lbTip.Text = Template.Replace(TemplateKeywords, (10 - count).ToString());
            if (!string.IsNullOrWhiteSpace(TipText))
            {
                this.toolTip1.SetToolTip(lbTip, TipText);
            }
            tmrExit.Start();
        }

        private void RunningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!start)
                Process.GetCurrentProcess().Kill();
        }
        int count = 0;
        bool start = false;
        private void tmrExit_Tick(object sender, EventArgs e)
        {
            count++;
            lbTip.Text = Template.Replace(TemplateKeywords, (10 - count).ToString());
            if (count >= 10)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tmrExit.Stop();
            start = true;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
