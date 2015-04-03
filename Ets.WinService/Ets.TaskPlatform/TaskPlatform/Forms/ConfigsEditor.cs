using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using TaskPlatform.PlatformLog;

namespace TaskPlatform.Forms
{
    public partial class ConfigsEditor : XtraForm
    {
        public ConfigsEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置要编辑的自定义配置项。
        /// </summary>
        public Dictionary<string, string> Configs { get; set; }

        private DataTable dataSource = null;
        DataTable dataSourceForItem = new DataTable();

        private void ConfigsEditor_Load(object sender, EventArgs e)
        {
            if (Configs == null)
            {
                Configs = new Dictionary<string, string>();
            }
            ThreadPool.QueueUserWorkItem(LoadData);
            //repositoryItemLookUpEdit1.EditValueChanged += new EventHandler(repositoryItemLookUpEdit1_EditValueChanged);
            //repositoryItemLookUpEdit1.QueryCloseUp += new CancelEventHandler(repositoryItemLookUpEdit1_QueryCloseUp);
            //repositoryItemLookUpEdit1.CloseUp += new DevExpress.XtraEditors.Controls.CloseUpEventHandler(repositoryItemLookUpEdit1_CloseUp);
        }

        void repositoryItemLookUpEdit1_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            //LookUpEdit lookUpEdit = sender as LookUpEdit;
            //string editValue = lookUpEdit.Text;
            //if (string.IsNullOrWhiteSpace(editValue))
            //    return;
            //if (!PlatformForm.AppSettings.ContainsValue(editValue) && !PlatformForm.ConnectionStrings.ContainsValue(editValue))
            //{
            //    DataRow dr = dataSourceForItem.NewRow();
            //    dr["DisplayName"] = editValue;
            //    dataSourceForItem.Rows.Add(dr);
            //    repositoryItemLookUpEdit1.DataSource = dataSourceForItem;
            //}
        }

        private void LoadData(object state)
        {
            try
            {
                if (Configs == null || Configs.Count() <= 0)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        Configs = new Dictionary<string, string>();
                        //XtraMessageBox.Show(this, "没有要编辑的项。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //DialogResult = DialogResult.Cancel;
                    }));
                }
                // 将字典转化为数据表格式
                // 以后应考虑使用对象List
                dataSource = new DataTable();
                dataSource.Columns.Add("Key");
                dataSource.Columns.Add("Value");
                dataSourceForItem.Columns.Add("DisplayName");
                foreach (var item in Configs)
                {
                    DataRow dr = dataSource.NewRow();
                    dr["Key"] = item.Key;
                    dr["Value"] = item.Value;
                    dataSource.Rows.Add(dr);
                    if (!string.IsNullOrWhiteSpace(item.Value) && !PlatformForm.AppSettings.ContainsValue(item.Value) && !PlatformForm.ConnectionStrings.ContainsValue(item.Value))
                    {
                        DataRow drItem = dataSourceForItem.NewRow();
                        drItem["DisplayName"] = item.Value;
                        dataSourceForItem.Rows.Add(drItem);
                    }
                }
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    gridView.DataSource = dataSource;
                }));
                foreach (var item in PlatformForm.AppSettings)
                {
                    DataRow dr = dataSourceForItem.NewRow();
                    dr["DisplayName"] = item.Value;
                    dataSourceForItem.Rows.Add(dr);
                }
                foreach (var item in PlatformForm.ConnectionStrings)
                {
                    DataRow dr = dataSourceForItem.NewRow();
                    dr["DisplayName"] = item.Value;
                    dataSourceForItem.Rows.Add(dr);
                }
                repositoryItemLookUpEdit1.DataSource = dataSourceForItem;
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Forms--ConfigsEditorException");
            }
        }

        void repositoryItemLookUpEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            string editValue = lookUpEdit.Text;
            if (string.IsNullOrWhiteSpace(editValue))
                return;
            if (!PlatformForm.AppSettings.ContainsValue(editValue) && !PlatformForm.ConnectionStrings.ContainsValue(editValue))
            {
                DataRow dr = dataSourceForItem.NewRow();
                dr["DisplayName"] = editValue;
                dataSourceForItem.Rows.Add(dr);
                repositoryItemLookUpEdit1.DataSource = dataSourceForItem;
            }
            string key = gridData.GetRowCellValue(gridData.GetSelectedRows()[0], "Key").ToString();
            dataSource.Select(string.Format("Key='{0}'", key))[0]["Value"] = editValue;
            gridView.DataSource = dataSource;
        }

        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Configs.Clear();
                // 清空字典并重装入
                foreach (DataRow item in dataSource.Rows)
                {
                    Configs.Add(item["Key"].ToString(), item["Value"].ToString());
                }
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Forms--ConfigsEditorException");
            }
            DialogResult = DialogResult.OK;
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NewCustomArgForm newCustomArg = new NewCustomArgForm();
            newCustomArg.ShowDialog();
            if (newCustomArg.IsOK)
            {
                var list = (from f in dataSource.Select()
                            where f["Key"].ToString() == newCustomArg.Key
                            select f).ToList();
                if (list.Count > 0)
                {
                    XtraMessageBox.Show(this, string.Format("键名 {0} 不允许为空。", newCustomArg.Key), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dataSource.LoadDataRow(new object[] { newCustomArg.Key, newCustomArg.Value }, true);
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridData.PostEditor();
            gridData.CloseEditor();
            DataRow row = gridData.GetFocusedDataRow();
            if (row != null)
            {
                if (XtraMessageBox.Show(this, "如果删除该自定义配置项，可能会对相应的计划任务产生致命性错误！！！\n\n确定要继续执行删除吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    dataSource.Rows.Remove(row);
                }
            }
        }
    }
}
