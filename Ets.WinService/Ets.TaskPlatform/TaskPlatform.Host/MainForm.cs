using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TaskPlatform.Host
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            timerProtect.Start();
        }

        private void timerProtect_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("NeedStart.tpf"))
                {
                    Open();
                }
            }
            catch { }
            try
            {
                File.Delete("NeedStart.tpf");
            }
            catch { }
        }

        private static void Open()
        {
            try
            {
                string path = File.ReadAllText("NeedStart.tpf").Trim('\\');
                Process.Start(path + "\\TaskPlatform.exe");
            }
            catch { }
            try
            {
                File.Delete("NeedStart.tpf");
            }
            catch { }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
