using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.Commom;
using System.Text.RegularExpressions;
using System.IO;

namespace TaskPlatform.Forms
{
    public partial class TaskAlterForm : DevExpress.XtraEditors.XtraForm
    {
        public TaskAlterForm()
        {
            InitializeComponent();
        }

        private SystemTask _systemTask = null;

        public SystemTask SystemTask
        {
            get { return _systemTask; }
            set { _systemTask = value; SetText(); }
        }

        private void SetText()
        {
            if (_systemTask != null)
            {
                txtDisplayName.Text = _systemTask.DisplayName;
                txtConnectionString.Text = _systemTask.ConnectionString;
                txtSQL.Text = _systemTask.SQL;
                txtLuaScript.Text = _systemTask.LuaScript;
                gpcTask.Text = _systemTask.ToString();
                rbtnMySQL.Checked = _systemTask.DBHelperName == "MySQLDBHelper";
            }
        }

        private void TaskAlterForm_SizeChanged(object sender, EventArgs e)
        {
            SetControl();
        }

        private void SetControl()
        {
            splitContainerControl1.SplitterPosition = splitContainerControl1.Height / 2;
            btnOK.Left = this.Width / 4;
            btnCancle.Left = (int)(this.Width / 1.6);
        }

        private void TaskAlterForm_Load(object sender, EventArgs e)
        {
            SetControl();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                XtraMessageBox.Show("请输入显示名称", "错误");
            }
            else if (txtDisplayName.Text.Trim().Length != txtDisplayName.Text.Length || txtDisplayName.Text.Trim().Length > 15)
            {
                XtraMessageBox.Show("显示名称的开头和结尾不能有空格且不能超过15个字符", "错误");
            }
            else if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
            {
                XtraMessageBox.Show("请输入数据库连接字符串", "错误");
            }
            else if (string.IsNullOrWhiteSpace(txtSQL.Text))
            {
                XtraMessageBox.Show("请输入执行查询的SQL", "错误");
            }
            else if (string.IsNullOrWhiteSpace(txtLuaScript.Text))
            {
                XtraMessageBox.Show("请输入逻辑脚本", "错误");
            }
            else
            {
                ReSetSystemTask();
                DialogResult = DialogResult.OK;
            }
        }

        private void ReSetSystemTask()
        {
            _systemTask.DisplayName = txtDisplayName.Text;
            _systemTask.ConnectionString = txtConnectionString.Text;
            _systemTask.LuaScript = txtLuaScript.Text;
            _systemTask.SQL = txtSQL.Text;
            _systemTask.DBHelperName = rbtnMySQL.Checked ? "MySQLDBHelper" : "SQLServerDBHelper";
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}