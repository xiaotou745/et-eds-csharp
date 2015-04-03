using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using TaskPlatform.Commom;
using System.Resources;
using System.Collections;
using TaskPlatform.TaskInterface;

namespace TaskPlatform.Forms
{
    public partial class SystemTaskSelector : DevExpress.XtraEditors.XtraForm
    {
        public SystemTaskSelector()
        {
            InitializeComponent();
        }

        Dictionary<string, SystemTask> _taskList = new Dictionary<string, SystemTask>();

        private SystemTask _systemTask = null;
        /// <summary>
        /// 获取选中的计划任务
        /// </summary>
        public SystemTask SystemTask
        {
            get { return _systemTask; }
        }

        private List<SystemTask> _selectedSystemTasks = new List<SystemTask>();
        /// <summary>
        /// 获取选中的计划任务
        /// </summary>
        public List<SystemTask> SelectedSystemTasks
        {
            get { return _selectedSystemTasks; }
        }

        private void SystemTaskSelector_Load(object sender, EventArgs e)
        {
            string[] folders = Directory.GetDirectories(PlatformForm.SystemTaskFilesPath);
            PropertyAccess<SystemTask> propertyAccess = new PropertyAccess<SystemTask>();
            foreach (string folder in folders)
            {
                try
                {
                    string name = folder.Substring(folder.LastIndexOf('\\') + 5);
                    SystemTask task = new Commom.SystemTask();
                    task.SystemName = name;
                    using (ResourceReader resourceReader = new ResourceReader(string.Format("{0}\\{1}.resources", folder, name)))
                    {
                        IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            try
                            {
                                propertyAccess.SetValue(task, enumerator.Key.ToString(), enumerator.Value);
                            }
                            catch { }
                        }
                    }
                    _taskList.Add(name, task);
                    lstbTasks.Items.Add(task);
                }
                catch { }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstbTasks.Items.Count > 0)
            {
                _systemTask = lstbTasks.SelectedItem as SystemTask;
                foreach (object obj in lstbTasks.SelectedItems)
                {
                    _selectedSystemTasks.Add(obj as SystemTask);
                }
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}