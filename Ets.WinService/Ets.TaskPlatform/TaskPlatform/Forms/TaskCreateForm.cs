using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraWizard;
using System.Data.SqlClient;
using System.Diagnostics;
using TaskPlatform.Commom;
using System.IO;
using MySql.Data.MySqlClient;

namespace TaskPlatform.Forms
{
    public partial class TaskCreateForm : XtraForm
    {
        public TaskCreateForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        private void TaskCreateForm_Load(object sender, EventArgs e)
        {
            errPro.SetIconAlignment(txtSystemName, ErrorIconAlignment.MiddleRight);
            errPro.SetIconAlignment(txtDisplayName, ErrorIconAlignment.MiddleRight);
            errPro.SetIconAlignment(cmbExecuteFrom, ErrorIconAlignment.MiddleRight);
            gpcFile.Location = gpcTaskPlatform.Location = gpcMachine.Location = gpcDataBase.Location;
            foreach (var item in TaskPlatform.PlatformForm.ConnectionStrings)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    cmbDataBaseConnectionString.Properties.Items.Add(item.Value);
                }
            }
            txtDBTestResult.BackColor = txtFinishInfo.BackColor = txtLuaScript.BackColor = txtSQL.BackColor = txtSQL.Parent.BackColor;
        }

        bool isPreClick = false;
        private WizardPage PrePage { set; get; }
        private WizardPage CurrentPage { set; get; }
        private WizardPage NextPage { set; get; }
        private int ExecuteFromSelectedIndex = 0;
        bool dbOK = false;
        string executeFrom = string.Empty;
        bool isCreated = false;

        private void wpgName_PageValidating(object sender, DevExpress.XtraWizard.WizardPageValidatingEventArgs e)
        {
            if (isPreClick)
            {
                return;
            }
            e.Valid = false;
            Regex notWholePattern = new Regex(@"^[0-9a-zA-Z\$]+$");
            int temp = 0;
            if (string.IsNullOrEmpty(txtSystemName.Text))
            {
                errPro.SetError(txtSystemName, "请输入系统名称");
            }
            else if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                errPro.SetError(txtDisplayName, "请输入显示名称");
            }
            else if (!notWholePattern.IsMatch(txtSystemName.Text))
            {
                errPro.SetError(txtSystemName, "系统名称只能输入字母或者数字");
            }
            else if (txtDisplayName.Text.Contains("\\") || txtDisplayName.Text.Contains("\""))
            {
                errPro.SetError(txtDisplayName, "显示名称中不能包含\\或\"字符");
            }
            else if (txtSystemName.Text.Trim().Length != txtSystemName.Text.Length || txtSystemName.Text.Trim().Length > 15)
            {
                errPro.SetError(txtSystemName, "系统名称的开头和结尾不能有空格且不能超过15个字符");
            }
            else if (txtDisplayName.Text.Trim().Length != txtDisplayName.Text.Length || txtDisplayName.Text.Trim().Length > 15)
            {
                errPro.SetError(txtDisplayName, "显示名称的开头和结尾不能有空格且不能超过15个字符");
            }
            else if (Directory.Exists(PlatformForm.SystemTaskFilesPath + "\\Sys-" + txtSystemName.Text.Trim()))
            {
                errPro.SetError(txtSystemName, "该名称已被占用");
            }
            else if (int.TryParse(txtSystemName.Text[0].ToString(), out temp))
            {
                errPro.SetError(txtSystemName, "系统名称不能以数字开头");
            }
            else
            {
                e.Valid = true;
            }
        }

        bool needValid = true;

        private void wpgExecuteFrom_PageValidating(object sender, WizardPageValidatingEventArgs e)
        {
            if (!needValid)
            {
                e.Valid = true;
                needValid = true;
                return;
            }
            if (isPreClick)
                return;
            e.Valid = false;
            if (!cmbExecuteFrom.Properties.Items.Contains(cmbExecuteFrom.SelectedItem))
            {
                errPro.SetError(cmbExecuteFrom, "请在指定的选项中选择");
            }
            else
            {
                switch (cmbExecuteFrom.SelectedItem.ToString())
                {
                    case "数据库查询":
                        btnTestDB.PerformClick();
                        if (dbOK)
                        {
                            e.Valid = true;
                            //NextPage = null;
                        }
                        else
                        {
                            errPro.SetError(cmbDataBaseConnectionString, "数据库测试失败");
                            return;
                        }
                        break;
                    case "平台资源":

                        break;
                    case "平台所在主机资源":

                        break;
                    case "打开(执行)文件":
                        if (File.Exists(txtFileName.Text))
                        {
                            e.Valid = true;
                        }
                        else
                        {
                            errPro.SetError(txtFileName, "请指定文件路径");
                            return;
                        }
                        break;
                    default:
                        cmbExecuteFrom.SelectedIndex = 0;
                        break;
                }
            }
            executeFrom = cmbExecuteFrom.SelectedItem.ToString();
            if (cmbExecuteFrom.SelectedItem.ToString() == "数据库查询")
            {
                NextPage = wpgExecuteFrom;
            }
            else if (cmbExecuteFrom.SelectedItem.ToString() == "打开(执行)文件")
            {
                needValid = false;
                NextPage = wpgFinish;
                wclCreateTask.SelectedPage = wpgFinish;
            }
        }

        private void ClearAllError()
        {
            errPro.ClearErrors();
        }

        private void wclCreateTask_PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            if (isCreated)
            {
                XtraMessageBox.Show("计划任务已经创建，不能返回到上一步了。", "提示");
                e.Handled = true;
            }
            isPreClick = true;
        }

        private void wclCreateTask_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            ClearAllError();
            isPreClick = false;
            if (!object.Equals(e.Page, welcomeWizardPage1) && !object.Equals(e.Page, completionWizardPage1))
            {
                PrePage = (WizardPage)e.Page;
                //if ((WizardPage)e.Page == wpgName)
                //{
                //    wclCreateTask.SelectedPage = wpgExecuteFrom;
                //    e.Handled = true;
                //}
            }
        }

        private void wclCreateTask_SelectedPageChanged(object sender, WizardPageChangedEventArgs e)
        {
            needValid = true;
            if (!object.Equals(e.Page, welcomeWizardPage1) && !object.Equals(e.Page, completionWizardPage1))
            {
                CurrentPage = (WizardPage)e.Page;
                if (CurrentPage == wpgFinish)
                {
                    wclCreateTask.NextText = "创建(&I)";
                    SetFinishInfo();
                }
                else
                {
                    wclCreateTask.NextText = "下一步(&N)";
                }
            }
        }

        private void SetFinishInfo()
        {
            txtFinishInfo.Clear();
            txtFinishInfo.AppendText("系统名称：");
            txtFinishInfo.AppendText(txtSystemName.Text);
            txtFinishInfo.AppendText(Environment.NewLine);
            txtFinishInfo.AppendText("显示名称：");
            txtFinishInfo.AppendText(txtDisplayName.Text);
            txtFinishInfo.AppendText(Environment.NewLine);
            txtFinishInfo.AppendText("逻辑来源：");
            txtFinishInfo.AppendText(cmbExecuteFrom.SelectedItem.ToString());
            txtFinishInfo.AppendText(Environment.NewLine);
            if (cmbExecuteFrom.SelectedItem.ToString() == "数据库查询")
            {
                txtFinishInfo.AppendText("连接字符串字符串：");
                txtFinishInfo.AppendText(cmbDataBaseConnectionString.SelectedItem.ToString());
                txtFinishInfo.AppendText(Environment.NewLine);
            }
            else if (cmbExecuteFrom.SelectedItem.ToString() == "打开(执行)文件")
            {
                txtFinishInfo.AppendText("文件路径：");
                txtFinishInfo.AppendText(txtFileName.Text);
                txtFinishInfo.AppendText(Environment.NewLine);
            }
        }

        private void cmbExecuteFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbExecuteFrom.Properties.Items.Contains(cmbExecuteFrom.SelectedItem))
            {
                cmbExecuteFrom.SelectedIndex = ExecuteFromSelectedIndex;
            }
            else
            {
                ExecuteFromSelectedIndex = cmbExecuteFrom.SelectedIndex;
            }
            switch (cmbExecuteFrom.SelectedItem.ToString())
            {
                case "数据库查询":
                    gpcDataBase.Visible = true;
                    gpcTaskPlatform.Visible = gpcMachine.Visible = gpcFile.Visible = false;
                    break;
                case "平台资源":
                    gpcTaskPlatform.Visible = true;
                    gpcDataBase.Visible = gpcMachine.Visible = gpcFile.Visible = false;
                    break;
                case "平台所在主机资源":
                    gpcMachine.Visible = true;
                    gpcDataBase.Visible = gpcTaskPlatform.Visible = gpcFile.Visible = false;
                    break;
                case "打开(执行)文件":
                    gpcFile.Visible = true;
                    gpcDataBase.Visible = gpcTaskPlatform.Visible = gpcMachine.Visible = false;
                    break;
                default:
                    cmbExecuteFrom.SelectedIndex = 0;
                    break;
            }
        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {
            dbOK = false;
            try
            {
                txtDBTestResult.Clear();
                if (rbtnMySQL.Checked)
                {
                    using (MySqlConnection connection = new MySqlConnection(cmbDataBaseConnectionString.Text))
                    {
                        connection.Open();
                        dbOK = true;
                        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(cmbDataBaseConnectionString.Text);
                        txtDBTestResult.AppendText("驱动程序：");
                        txtDBTestResult.AppendText(builder.ApplicationName);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("数据库地址：");
                        txtDBTestResult.AppendText(builder.Server);
                        txtDBTestResult.AppendText("       服务器版本：");
                        txtDBTestResult.AppendText(connection.ServerVersion);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("登录数据库：");
                        txtDBTestResult.AppendText(builder.Database);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("登录用户名：");
                        txtDBTestResult.AppendText(builder.UserID);
                        txtDBTestResult.AppendText("       登录密码：");
                        txtDBTestResult.AppendText(builder.Password);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("客户端标识：");
                        txtDBTestResult.AppendText(connection.WorkstationId);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("测试状态：可连接");
                        connection.Close();
                    }
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(cmbDataBaseConnectionString.Text))
                    {
                        connection.Open();
                        dbOK = true;
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cmbDataBaseConnectionString.Text);
                        txtDBTestResult.AppendText("驱动程序：");
                        txtDBTestResult.AppendText(builder.ApplicationName);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("数据库地址：");
                        txtDBTestResult.AppendText(builder.DataSource);
                        txtDBTestResult.AppendText("       服务器版本：");
                        txtDBTestResult.AppendText(connection.ServerVersion);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("登录数据库：");
                        txtDBTestResult.AppendText(builder.InitialCatalog);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("登录用户名：");
                        txtDBTestResult.AppendText(builder.UserID);
                        txtDBTestResult.AppendText("       登录密码：");
                        txtDBTestResult.AppendText(builder.Password);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("客户端标识：");
                        txtDBTestResult.AppendText(connection.WorkstationId);
                        txtDBTestResult.AppendText(Environment.NewLine);
                        txtDBTestResult.AppendText("测试状态：可连接");
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                txtDBTestResult.Text = ex.ToString();
            }
        }

        private void wpgDBQuery_PageValidating(object sender, WizardPageValidatingEventArgs e)
        {
            if (cmbExecuteFrom.SelectedItem.ToString() == "数据库查询")
            {
                if (string.IsNullOrWhiteSpace(txtSQL.Text))
                {
                    e.Valid = false;
                    e.ErrorText = "请设置查询语句";
                }
            }
        }

        private void wpgDBExecutedScripts_PageValidating(object sender, WizardPageValidatingEventArgs e)
        {
            if (cmbExecuteFrom.SelectedItem.ToString() == "数据库查询")
            {
                if (string.IsNullOrWhiteSpace(txtLuaScript.Text))
                {
                    e.Valid = false;
                    e.ErrorText = "请设置逻辑脚本";
                }
            }
        }

        private void labelControl5_DoubleClick(object sender, EventArgs e)
        {
            XtraForm form = new XtraForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(600, 500);
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            RichTextBox txt = new RichTextBox();
            txt.Text = "----" + labelControl5.SuperTip.ToString().Replace("/--", Environment.NewLine + "--双横线为注释" + Environment.NewLine + "--").Replace("/详情", Environment.NewLine + Environment.NewLine + "--详情");
            txt.LinkClicked += (s, args) =>
            {
                try
                {
                    Process.Start(args.LinkText);
                }
                catch { }
            };
            form.Controls.Add(txt);
            txt.Dock = DockStyle.Fill;
            form.ShowInTaskbar = false;
            form.ShowIcon = false;
            form.ShowDialog();
        }

        private void wpgFinish_PageValidating(object sender, WizardPageValidatingEventArgs e)
        {
            if (isPreClick)
                return;
            try
            {
                SystemTask systemTask = new SystemTask();
                systemTask.SystemName = txtSystemName.Text;
                systemTask.DisplayName = txtDisplayName.Text;
                if (cmbExecuteFrom.SelectedItem.ToString() == "数据库查询")
                {
                    systemTask.CreateTaskTemplateName = "CreateTaskTemplate";
                    systemTask.DBHelperName = rbtnMySQL.Checked ? "MySQLDBHelper" : "SQLServerDBHelper";
                    systemTask.ConnectionString = cmbDataBaseConnectionString.Text;
                    systemTask.SQL = txtSQL.Text;
                    systemTask.LuaScript = txtLuaScript.Text;
                }
                else if (cmbExecuteFrom.SelectedItem.ToString() == "打开(执行)文件")
                {
                    systemTask.CreateTaskTemplateName = "OpenFileTaskTemplate";
                    systemTask.FileName = txtFileName.Text;
                    systemTask.OpenFileVerb = txtVerb.Text;
                    systemTask.OpenFileArgs = txtArgs.Text;
                }
                TaskBuilder taskBuilder = new TaskBuilder();
                taskBuilder.Build(systemTask);
                isCreated = true;
            }
            catch (Exception ex)
            {
                e.Valid = false;
                XtraMessageBox.Show(ex.ToString(), "创建错误");
            }
        }

        private void gpcDataBase_VisibleChanged(object sender, EventArgs e)
        {
            gpcDataBaseType.Visible = gpcDataBase.Visible;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = fileDialog.FileName;
            }
        }

        private void wclCreateTask_SelectedPageChanging(object sender, WizardPageChangingEventArgs e)
        {
            if (cmbExecuteFrom.SelectedItem.ToString() == "打开(执行)文件" && !isCreated)
            {
                e.Page = wpgFinish;
            }
        }
    }
}
