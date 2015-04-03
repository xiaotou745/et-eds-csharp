using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskPlatform.Commom;
using TaskPlatform.TaskDomain;
using DevExpress.XtraEditors;
using TaskPlatform.Forms;

namespace TaskPlatform.Controls
{
    public partial class DeadAlarmManager : UserControl
    {
        public DeadAlarmManager()
        {
            InitializeComponent();
        }

        private void DeadAlarmManager_Paint(object sender, PaintEventArgs e)
        {
            if (Parent != null && !Parent.Size.IsEmpty)
            {
                this.Size = Parent.Size;
            }
        }

        private void DeadAlarmManager_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ribtnDeleteAlarm_Click(object sender, EventArgs e)
        {
            DataRow row = gridData.GetFocusedDataRow();
            if (row == null)
                return;
            if (XtraMessageBox.Show(this, "与该警报对应的任务可能被误卸载或还没有载入平台。\n删除警报后可能导致遗忘掉对该任务的处理。\n\n是否继续删除警报？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            var alarmObject = (from alarm in TaskPlatform.PlatformForm.AlarmObjects
                               where alarm.PlanName == row["TaskKey"].ToString()
                               select alarm).FirstOrDefault();
            if (alarmObject != null)
            {
                TaskPlatform.PlatformForm.AlarmObjects.Remove(alarmObject);
                TaskPlatform.PlatformForm.SaveAlterSystemCache();
                LoadData();
            }
        }

        private void ribtnConfigAlarm_Click(object sender, EventArgs e)
        {
            DataRow row = gridData.GetFocusedDataRow();
            if (row == null)
                return;
            var alarmObject = (from alarm in TaskPlatform.PlatformForm.AlarmObjects
                               where alarm.PlanName == row["TaskKey"].ToString()
                               select alarm).FirstOrDefault();
            if (alarmObject != null)
            {
                AlarmConfigForm form = new AlarmConfigForm();
                form.OldAlarmObject = alarmObject;
                form.ShowDialog();
                if (form.IsOK)
                {
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("TaskCommonKey");
            table.Columns.Add("TaskKey");
            table.Columns.Add("TaskName");
            table.Columns.Add("AlarmType");
            table.Columns.Add("ConfigAlarm");
            table.Columns.Add("Alarm");
            TaskPlatform.PlatformForm.AlarmObjects.ForEach(item =>
            {
                try
                {
                    if (item.PlanName != "%sys%\\All")
                    {
                        DomainLoader loaderObject = TaskPlatform.PlatformForm.DomainLoaderList.Find(loader => { return loader.DomainName == item.PlanName; });
                        if (loaderObject == null)
                        {
                            int index = item.PlanName.IndexOf('-');
                            if (index < 0)
                            {
                                index = item.PlanName.IndexOf('_');
                            }
                            string taskCommonKey = item.PlanName;
                            if (index >= 0)
                            {
                                taskCommonKey = item.PlanName.Substring(0, index);
                            }
                            table.LoadDataRow(new object[] { taskCommonKey, item.PlanName, item.TaskName, AlarmObject.GetAlarmTypeString(item), "配置警报", "删除警报" }, true);
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
    }
}
