using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TaskPlatform.Commom;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.IO;

namespace TaskPlatform.Controls
{
    public partial class CacheManager : UserControl
    {
        public CacheManager()
        {
            InitializeComponent();
        }

        private void CacheManager_Paint(object sender, PaintEventArgs e)
        {
            if (this.Parent != null && !this.Parent.Size.IsEmpty)
            {
                this.Size = this.Parent.Size;
                gpcAllKeys.Height = gpcAllKeys.Parent.Height - gpcAllKeys.Top;
            }
        }

        private Dictionary<string, LocalDataCacheContainer> cacheContainerList = new Dictionary<string, LocalDataCacheContainer>();
        private string currentKey = string.Empty;

        private void CacheManager_Load(object sender, EventArgs e)
        {
            this.popupMenu1.LinksPersistInfo.Clear();
            XtraMessageBox.Show(this, "请认真阅读下一个弹窗，否则后果自负！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (XtraMessageBox.Show(this, "该功能尚在测试中，目前性能上不稳定。\r\n如果您确实需要使用该功能，请自行将LoaclDataCache目录全部备份到安全区域，以免数据丢失。\r\n如果您没有遵照提示而导致数据丢失，后果，自负！！！！\r\n\r\n您备份好LoaclDataCache目录了吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                this.Enabled = false;
                return;
            }
            else
            {
                foreach (var key in LoaclDataCacheManager.GetLoaclDataCacheManagerNameList())
                {
                    cacheContainerList.Add(Path.GetFileNameWithoutExtension(key), new LocalDataCacheContainer(Path.GetFileNameWithoutExtension(key)));
                    BarButtonItem item = new BarButtonItem();
                    item.Caption = Path.GetFileName(key);
                    item.Tag = Path.GetFileNameWithoutExtension(key);
                    item.ItemClick += new ItemClickEventHandler(item_ItemClick);
                    this.popupMenu1.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(item));
                }
                lstbAllKeys.Items.Clear();
                currentKey = btnDefault.Tag.ToString();
                ThreadPool.QueueUserWorkItem(LoadData);
            }
        }

        private void item_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshData(e.Item.Tag.ToString());
        }

        private void RefreshData(string key)
        {
            if (currentKey == key)
                return;
            currentKey = key;
            lstbAllKeys.Items.Clear();
            ThreadPool.QueueUserWorkItem(LoadData);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            CacheManager_Load(null, null);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="state"></param>
        private void LoadData(object state)
        {
            if (cacheContainerList[currentKey].Count <= 0)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    XtraMessageBox.Show(this, "没有缓存项。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
                return;
            }
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                btnDelete.Enabled = true;
            }));
            foreach (string key in cacheContainerList[currentKey].Keys)
            {
                // 不能使用BeginInvoke。会导致key值被另一次循环修改。
                this.Invoke(new MethodInvoker(delegate()
                {
                    lstbAllKeys.Items.Add(key);
                }));
            }
        }

        private void lstbAllKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstbAllKeys.Items.Count > 0 && lstbAllKeys.SelectedItem != null)
            {
                ppgValue.SelectedObject = cacheContainerList[currentKey][lstbAllKeys.SelectedItem.ToString()] as LoaclDataCacheObject;
            }
            else
            {
                ppgValue.SelectedObject = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show(this, "删除该项可能导致平台的不稳定。\r\n\r\n您是否继续删除该项？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            LoaclDataCacheObject ldc = ppgValue.SelectedObject as LoaclDataCacheObject;
            cacheContainerList[currentKey].Delete(ldc);
            lstbAllKeys.Items.Remove(ldc.Key);
        }

        private void ppgValue_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveChanges();
        }

        private void ppgValue_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            try
            {
                gpcItemppv.Text = e.NewSelection.Label + "的属性";
                gpcItemppv.Tag = e.NewSelection.Label;
                ppgItemProperty.SelectedObject = e.NewSelection.Value;
            }
            catch { }
        }

        private void ppgItemProperty_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveChanges();
        }

        /// <summary>
        /// 保存所做的更改。
        /// </summary>
        private void SaveChanges()
        {
            LoaclDataCacheObject ldc = ppgValue.SelectedObject as LoaclDataCacheObject;
            cacheContainerList[currentKey].Update(ldc.Key, ldc);
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            RefreshData(btnDefault.Tag.ToString());
        }
    }
}
