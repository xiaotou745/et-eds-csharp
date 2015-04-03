using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.Commom;
using DevExpress.Utils;

namespace TaskPlatform.Forms
{
    public partial class AlarmConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public AlarmConfigForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 计划任务运行时域
        /// </summary>
        public TaskDomain.DomainLoader Loader { get; set; }
        /// <summary>
        /// 是否配置所有警报
        /// </summary>
        public bool IsConfigAll
        {
            get
            {
                return Loader == null;
            }
        }
        /// <summary>
        /// 计划任务Key
        /// </summary>
        public string TaskKey
        {
            get
            {
                return IsConfigAll ? (_oldAlarmObject == null ? "%sys%\\All" : _oldAlarmObject.PlanName) : Loader.DomainName;
            }
        }
        /// <summary>
        /// 计划任务名称
        /// </summary>
        public string TaskName
        {
            get
            {
                return IsConfigAll ? (_oldAlarmObject == null ? "所有计划任务" : _oldAlarmObject.TaskName) : Loader.TaskOperator.TaskName();
            }
        }
        /// <summary>
        /// 任务可超时秒数
        /// </summary>
        public int TotalSeconds
        {
            get
            {
                return IsConfigAll ? 0 : (Loader.Plan == null ? 0 : Loader.Plan.TotalSeconds);
            }
        }
        AlarmObject _alarmObject;
        /// <summary>
        /// 配置后的警报信息
        /// </summary>
        public AlarmObject AlarmObject
        {
            get { return _alarmObject; }
        }

        AlarmObject _oldAlarmObject;
        /// <summary>
        /// 需要配置的警报信息
        /// </summary>
        public AlarmObject OldAlarmObject
        {
            get { return _oldAlarmObject; }
            set { _oldAlarmObject = value; }
        }

        private bool _isOK = false;
        /// <summary>
        /// 是否配置成功
        /// </summary>
        public bool IsOK
        {
            get { return _isOK; }
        }

        private void AlarmConfigForm_Load(object sender, EventArgs e)
        {
            _isOK = false;
            this.Text = string.Format("为{0}({1})配置警报", TaskName, TaskKey);
            _alarmObject = TaskPlatform.PlatformForm.AlarmObjects.Find(item =>
            {
                return item.PlanName == TaskKey;
            });
            if (_alarmObject == null)
            {
                _alarmObject = new AlarmObject()
                {
                    PlanName = TaskKey,
                    AlarmType = 4,
                    RunCount = 0,
                    AlarmWhenCancelCount = 0,
                    AlarmWhenFailCount = 0,
                    TaskName = TaskName
                };
                TaskPlatform.PlatformForm.AlarmObjects.Add(_alarmObject);
            }
            lbAlarmType.Text = AlarmObject.GetAlarmTypeString(_alarmObject);
            ckbEnable.Checked = _alarmObject.AlarmType != 4;
            txtTimeOut.Text = _alarmObject.TimeOutRatio.ToString("G");
            lbTimeOut.Text = string.Format("{0}秒", (int)(_alarmObject.TimeOutRatio * TotalSeconds));
            txtRunCount.Text = _alarmObject.RunCount.ToString();
            txtFailCount.Text = _alarmObject.AlarmWhenFailCount.ToString();
            txtCancelCount.Text = _alarmObject.AlarmWhenCancelCount.ToString();
            txtEmail.Text = _alarmObject.AlarmEmailAddress;
            SetEnable();
        }

        private void ckbEnable_CheckedChanged(object sender, EventArgs e)
        {
            SetEnable();
        }

        private void SetEnable()
        {
            txtRunCount.Enabled = txtFailCount.Enabled = txtCancelCount.Enabled = ckbEnable.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _isOK = false;
            if (!ckbEnable.Checked)
            {
                if (XtraMessageBox.Show(this, "关闭报警后，计划任务出现任何问题后，平台都将不发出警报。\n\n是否确定关闭报警？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
                _alarmObject.AlarmType = 4;
                _alarmObject.RunCount = 20;
                _alarmObject.AlarmWhenFailCount = 2;
                _alarmObject.AlarmWhenCancelCount = 3;
                _alarmObject.TimeOutRatio = decimal.Parse(txtTimeOut.Text.Trim());
                _alarmObject.AlarmEmailAddress = txtEmail.Text.Trim();
            }
            else
            {
                int runCount = 0, failCount = 0, cancelCount = 0;
                if (!int.TryParse(txtRunCount.Text.Trim(), out runCount))
                {
                    txtRunCount.Focus();
                    txtRunCount.SelectAll();
                    XtraMessageBox.Show(this, "运行次数填写不正确。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!int.TryParse(txtFailCount.Text.Trim(), out failCount))
                {
                    txtFailCount.Focus();
                    txtFailCount.SelectAll();
                    XtraMessageBox.Show(this, "失败次数填写不正确。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!int.TryParse(txtCancelCount.Text.Trim(), out cancelCount))
                {
                    txtCancelCount.Focus();
                    txtCancelCount.SelectAll();
                    XtraMessageBox.Show(this, "取消执行次数填写不正确。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    _alarmObject.TimeOutRatio = decimal.Parse(txtTimeOut.Text.Trim());
                    _alarmObject.AlarmEmailAddress = txtEmail.Text.Trim();
                    _alarmObject = SetAlarmObject(_alarmObject, runCount, failCount, cancelCount);
                }
            }
            PlatformForm.SaveAlterSystemCache();
            _isOK = true;
            DialogResult = DialogResult.OK;
        }

        internal static AlarmObject SetAlarmObject(AlarmObject alarmObject, int runCount, int failCount, int cancelCount)
        {
            if (runCount < 1)
            {
                alarmObject.AlarmType = 5;
                alarmObject.RunCount = runCount;
                alarmObject.AlarmWhenFailCount = 2;
                alarmObject.AlarmWhenCancelCount = 3;
            }
            else
            {
                if (failCount < 0)
                {
                    if (cancelCount < 0)
                    {
                        alarmObject.AlarmType = 5;
                    }
                    else
                    {
                        alarmObject.AlarmType = 3;
                    }
                }
                else
                {
                    if (cancelCount < 0)
                    {
                        alarmObject.AlarmType = 2;
                    }
                    else
                    {
                        alarmObject.AlarmType = 1;
                    }
                }
                alarmObject.RunCount = runCount;
                alarmObject.AlarmWhenFailCount = failCount;
                alarmObject.AlarmWhenCancelCount = cancelCount;
            }
            return alarmObject;
        }

        private void txtTimeOut_EditValueChanged(object sender, EventArgs e)
        {
            lbTimeOut.Text = string.Format("{0}秒", (int)(decimal.Parse(txtTimeOut.Text.Trim()) * TotalSeconds));
        }

        private void AlarmConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}