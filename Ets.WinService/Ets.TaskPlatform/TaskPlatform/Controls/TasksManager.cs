using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TaskPlatform.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.Commom;
using DevExpress.XtraNavBar;
using TaskPlatform.TaskDomain;
using System.IO;
using System.Diagnostics;

namespace TaskPlatform.Controls
{
    public partial class TasksManager : UserControl
    {
        public TasksManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 计划任务链接集合
        /// </summary>
        public DevExpress.XtraNavBar.NavLinkCollection TaskItems { get; set; }

        private void TasksManager_Paint(object sender, PaintEventArgs e)
        {
            if (Parent != null && !Parent.Size.IsEmpty)
            {
                this.Size = Parent.Size;
            }
        }

        private void TasksManager_Load(object sender, EventArgs e)
        {
            gpcTasks.Location = new Point(gpcQuery.Location.X, gpcTasks.Location.Y);
            gpcTasks.Width = gpcQuery.Width;
            ThreadPool.QueueUserWorkItem(LoadTasks);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTasks(null);
        }

        public void LoadTasks(object state)
        {
            DataTable table = new DataTable();
            table.Columns.Add("TaskCommonKey");
            table.Columns.Add("TaskKey");
            table.Columns.Add("TaskName");
            table.Columns.Add("Status");
            table.Columns.Add("TimeOut");
            table.Columns.Add("AlarmType");
            table.Columns.Add("TaskDescription");
            table.Columns.Add("Plan");
            table.Columns.Add("Alarm");
            table.Columns.Add("Open");
            table.Columns.Add("FileName");
            TaskPlatform.PlatformForm.DomainLoaderList.ForEach(item =>
            {
                try
                {
                    string taskKey = txtTaskKey.Text.Trim().ToLower();
                    if (taskKey.Length < 1 || item.DomainName.ToLower().Contains(taskKey))
                    {
                        string taskName = txtTaskName.Text.Trim().ToLower();
                        if (taskName.Length < 1 || item.TaskOperator.TaskName().ToLower().Contains(taskName))
                        {
                            int index = item.DomainName.IndexOf('-');
                            if (index < 0)
                            {
                                index = item.DomainName.IndexOf('_');
                            }
                            string taskCommonKey = item.DomainName;
                            if (index >= 0)
                            {
                                taskCommonKey = item.DomainName.Substring(0, index);
                            }

                            AlarmObject alarmObject = TaskPlatform.PlatformForm.AlarmObjects.Find(ao => { return ao.PlanName == item.DomainName; });
                            int timeOut = (int)((item.Plan == null ? 0 : item.Plan.TotalSeconds) * (alarmObject == null ? 1 : alarmObject.TimeOutRatio));
                            table.LoadDataRow(new object[] { taskCommonKey, item.DomainName, item.TaskOperator.TaskName(), item.Plan == null ? "未配置" : (item.Plan.Enable ? "已启用" : "已停用"), timeOut, AlarmObject.GetAlarmTypeString(alarmObject), item.TaskOperator.TaskDescription(), "配置计划", "配置警报", "打开", item.FileName }, true);
                        }
                    }
                }
                catch { }
            });
            gridControl.BeginInvoke(new Action(() =>
            {
                gridControl.DataSource = table;
            }));
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            TaskCreateForm taskCreateForm = new TaskCreateForm();
            taskCreateForm.ShowDialog();
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            //if (!PlatformForm.SafeMode)
            //{
            //    XtraMessageBox.Show("只有在安全模式下才能执行该操作。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            SystemTaskSelector selector = new SystemTaskSelector();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                TaskAlterForm alterForm = new TaskAlterForm();
                alterForm.SystemTask = selector.SystemTask;
                string oldName = alterForm.SystemTask.ToString();
                if (alterForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TaskBuilder builder = new TaskBuilder();
                        builder.ResourcesBuilder(alterForm.SystemTask);
                        XtraMessageBox.Show(string.Format("修改任务【{0}】为【{1}】成功。请重新加载任务。", oldName, selector.SystemTask), "提示");
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(string.Format("修改任务【{0}】为【{1}】时失败。请参考：\n{2}", oldName, selector.SystemTask, ex.ToString()), "修改错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            //}
        }

        private void btnReBuild_Click(object sender, EventArgs e)
        {
            //if (!PlatformForm.SafeMode)
            //{
            //    XtraMessageBox.Show("只有在安全模式下才能执行该操作。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            SystemTaskSelector selector = new SystemTaskSelector();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                selector.SelectedSystemTasks.ForEach(systemTask =>
                {
                    try
                    {
                        TaskBuilder taskBuilder = new TaskBuilder();
                        taskBuilder.ReBuild(systemTask);
                        XtraMessageBox.Show(string.Format("重建任务【{0}】成功。请重新加载任务。", systemTask), "提示");
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(string.Format("重建任务【{0}】时失败。请参考：\n{1}", systemTask, ex.ToString()), "重建错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                });
            }
            //}
        }

        private void ribtnConfigPlan_Click(object sender, EventArgs e)
        {
            DataRow row = gridData.GetFocusedDataRow();
            if (row == null)
                return;
            DomainLoader loader = TaskPlatform.PlatformForm.DomainLoaderList.Find(item => { return item.DomainName == row["TaskKey"].ToString(); });
            if (loader == null)
                return;
            TaskStage.ConfigPlan(loader);
            LoadTasks(null);
        }

        private void ribtnConfigAlarm_Click(object sender, EventArgs e)
        {
            DataRow row = gridData.GetFocusedDataRow();
            if (row == null)
                return;
            DomainLoader loader = TaskPlatform.PlatformForm.DomainLoaderList.Find(item => { return item.DomainName == row["TaskKey"].ToString(); });
            if (loader == null)
                return;
            Forms.AlarmConfigForm form = new AlarmConfigForm();
            form.Loader = loader;
            form.ShowDialog();
            if (form.IsOK)
            {
                LoadTasks(null);
            }
        }

        private void ribtnOpen_Click(object sender, EventArgs e)
        {
            DataRow row = gridData.GetFocusedDataRow();
            if (row == null)
                return;
            DomainLoader loader = TaskPlatform.PlatformForm.DomainLoaderList.Find(item => { return item.DomainName == row["TaskKey"].ToString(); });
            if (loader == null)
                return;
            TaskPlatform.PlatformForm.Form.OpenTaskStage(loader);
        }

        private void btnAllAlarmConfig_Click(object sender, EventArgs e)
        {
            if (PlatformForm.SafeMode)
            {
                XtraMessageBox.Show(this, "安全模式下不允许执行该操作。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            Forms.AlarmConfigForm form = new AlarmConfigForm();
            form.ShowDialog();
            if (form.IsOK)
            {
                TaskPlatform.PlatformForm.AlarmObjects.ForEach(item =>
                {
                    if (item.PlanName != form.AlarmObject.PlanName)
                    {
                        item.AlarmEmailAddress = form.AlarmObject.AlarmEmailAddress;
                        item.AlarmType = form.AlarmObject.AlarmType;
                        item.AlarmWhenCancelCount = form.AlarmObject.AlarmWhenCancelCount;
                        item.AlarmWhenFailCount = form.AlarmObject.AlarmWhenFailCount;
                        item.DateTime1 = form.AlarmObject.DateTime1;
                        item.DateTime2 = form.AlarmObject.DateTime2;
                        item.DateTime3 = form.AlarmObject.DateTime3;
                        item.DateTime4 = form.AlarmObject.DateTime4;
                        item.Field1 = form.AlarmObject.Field1;
                        item.Field2 = form.AlarmObject.Field2;
                        item.Field3 = form.AlarmObject.Field3;
                        item.Field4 = form.AlarmObject.Field4;
                        item.RunCount = form.AlarmObject.RunCount;
                        item.TimeOutRatio = form.AlarmObject.TimeOutRatio;
                    }
                });
                TaskPlatform.PlatformForm.SaveAlterSystemCache();
                LoadTasks(null);
            }
        }

        private void btnExportToExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveFileDialog.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName.ToLower().EndsWith(".xls"))
                {
                    gridControl.ExportToXls(saveFileDialog.FileName);
                }
                else
                {
                    gridControl.ExportToXlsx(saveFileDialog.FileName);
                }
            }
        }

        private void btnExportToPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveFileDialog.Filter = "PDF文件(*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gridControl.ExportToPdf(saveFileDialog.FileName);
            }
        }

        private void btnExportToHtml_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveFileDialog.Filter = "Html文件(*.html)|*.html";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gridControl.ExportToHtml(saveFileDialog.FileName);
            }
        }

        private void btnExportToMht_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveFileDialog.Filter = "Mht文件(*.mht)|*.mht";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gridControl.ExportToMht(saveFileDialog.FileName);
            }
        }

        private void btnExportToRtf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveFileDialog.Filter = "Rtf文件(*.rtf)|*.rtf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gridControl.ExportToRtf(saveFileDialog.FileName);
            }
        }

        private void gridControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu.ShowPopup(MousePosition);
            }
        }

        private void btnOpenDir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataRow row = gridData.GetFocusedDataRow();
                if (row == null)
                {
                    PlatformForm.Form.ShowWarning("请先选择一个计划任务。");
                }
                else
                {
                    Process.Start("explorer", Path.GetDirectoryName(row["FileName"].ToString()));
                }
            }
            catch (Exception ex)
            {
                PlatformForm.Form.ShowError(ex.ToString());
            }
        }
    }
}
