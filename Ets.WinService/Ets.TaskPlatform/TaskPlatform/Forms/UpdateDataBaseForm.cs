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

namespace TaskPlatform.Forms
{
    public partial class UpdateDataBaseForm : XtraForm
    {
        public UpdateDataBaseForm()
        {
            InitializeComponent();
        }

        private bool _isOK = false;
        /// <summary>
        /// 是否已修复成功
        /// </summary>
        public bool IsOK
        {
            get { return _isOK; }
        }

        private bool _canClose = false;

        private void UpdateDataBaseForm_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(UpdateDataBase);
        }

        private void UpdateDataBase(object state)
        {
            bool hasError = false;
            try
            {
                SetInfo("正在准备升级默认缓存……");
                List<string> keys = TaskPlatform.PlatformForm.PlanPlatformCache.Keys.Cast<string>().ToList();
                Thread.Sleep(1000);
                SetInfo("正在升级默认缓存……");
                foreach (string key in keys)
                {
                    try
                    {
                        SetInfo("正在升级键：" + key);
                        TaskPlatform.PlatformForm.PlanPlatformCache.Update(TaskPlatform.PlatformForm.PlanPlatformCache[key]);
                        Thread.Sleep(200);
                    }
                    catch (Exception ee)
                    {
                        hasError = true;
                        SetInfo(ee.ToString());
                    }
                }
                SetInfo("正在准备升级警报缓存……");
                keys = TaskPlatform.PlatformForm.AlertSystemCache.Keys.Cast<string>().ToList();
                Thread.Sleep(1000);
                SetInfo("正在升级警报缓存……");
                foreach (string key in keys)
                {
                    try
                    {
                        SetInfo("正在升级键：" + key);
                        TaskPlatform.PlatformForm.AlertSystemCache.Update(TaskPlatform.PlatformForm.AlertSystemCache[key]);
                        Thread.Sleep(200);
                    }
                    catch (Exception ee)
                    {
                        hasError = true;
                        SetInfo(ee.ToString());
                    }
                }
                SetInfo("升级成功完成。");
                SetInfo(Environment.NewLine);
                Thread.Sleep(2000);
                _isOK = true;
                _canClose = true;
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new Action(() =>
                {
                    XtraMessageBox.Show(this, "数据升级遇到问题。\r\n\r\n" + ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
                _isOK = false;
                _canClose = true;
            }
            if (!hasError)
            {
                //this.Invoke(new Action(() =>
                //{
                //    this.Close();
                //}));
            }
        }

        private void UpdateDataBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_canClose)
                {
                    if (!_isOK && XtraMessageBox.Show(this, "数据升级尚未完成，是否结束本次升级？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch { }
        }

        private void SetInfo(string content)
        {
            try
            {
                txtInfo.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        txtInfo.AppendText(Environment.NewLine);
                        txtInfo.AppendText(content);
                    }
                    catch { }
                }));
            }
            catch { }
        }

        private void txtInfo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtInfo.ScrollToCaret();
            }
            catch { }
        }
    }
}
