using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.PlanEngine;
using DevExpress.XtraEditors.Controls;
using TaskPlatform.PlatformLog;

namespace TaskPlatform.Forms
{
    public partial class PlanConfigForm : XtraForm
    {
        public PlanConfigForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 要配置的计划
        /// </summary>
        public PlanEngine.Plan Plan { get; set; }
        /// <summary>
        /// 计划任务的名称
        /// </summary>
        public string TaskName { get; set; }

        private void PlanConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime timeNow = DateTime.Now;
                deDateOnce.DateTime = timeNow;
                teTimeOnce.Time = timeNow.AddMinutes(1);
                deStart.DateTime = timeNow;
                teStart.Time = timeNow;
                deEnd.DateTime = timeNow.AddMonths(1);
                teEnd.Time = timeNow;
                teStartEnableTime.Time = timeNow.Date;
                teEndEnableTime.Time = timeNow.Date.AddDays(1).AddSeconds(-1);
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("秒", PlanTimeUnit.Second));
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("分", PlanTimeUnit.Minute));
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("小时", PlanTimeUnit.Hour));
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("天", PlanTimeUnit.Day));
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("月", PlanTimeUnit.Month));
                cmbPlanUnit.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("年", PlanTimeUnit.Year));
                txtPlanName.BackColor = this.BackColor;
                if (Plan != null)
                {
                    txtPlanName.Text = Plan.PlanName;
                    this.Text = string.Format("配置{0}的执行计划{1}", TaskName, Plan.PlanName);
                    ckbEnable.Checked = Plan.Enable;
                    ckbType.Checked = Plan.Type == PlanType.Repeat;
                    Plan.ComputeTime(true);
                    deDateOnce.DateTime = Plan.NextRunTime;
                    teTimeOnce.Time = Plan.NextRunTime;
                    cmbPlanUnit.SelectedIndex = (int)Plan.PlanUnit;
                    seSpan.Value = Plan.Interval;
                    deStart.DateTime = Plan.PlanStartTime;
                    teStart.Time = Plan.PlanStartTime;
                    ckbNoOver.Checked = Plan.NoOverTime;
                    if (!ckbNoOver.Checked)
                        deEnd.DateTime = Plan.PlanEndTime;
                    teEnd.Time = Plan.PlanEndTime;
                    //teStartEnableTime.Time = Plan.StartEnableTime;
                    //teEndEnableTime.Time = Plan.EndEnableTime;
                }
                ckbType_CheckedChanged();
                ckbNoOver_CheckedChanged();
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Forms--PlanConfigFormException");
            }
        }

        private void ckbType_CheckedChanged(object sender = null, EventArgs e = null)
        {
            // 设置控件的可用状态和线的颜色
            lbOnce.Enabled = lbOnceDate.Enabled = lbOnceTime.Enabled = deDateOnce.Enabled = teTimeOnce.Enabled = !ckbType.Checked;
            lbRepeat.Enabled = lbSpan.Enabled = seSpan.Enabled = cmbPlanUnit.Enabled = lbRun.Enabled = lbContinue.Enabled = lbStart.Enabled = lbEnd.Enabled = deStart.Enabled = teStart.Enabled = deEnd.Enabled = teEnd.Enabled = ckbNoOver.Enabled = ckbType.Checked;
            lineOnce.BorderColor = ckbType.Checked ? Color.Gray : Color.Black;
            lineContinue.BorderColor = lineRepeat.BorderColor = ckbType.Checked ? Color.Black : Color.Gray;
        }

        private void ckbNoOver_CheckedChanged(object sender = null, EventArgs e = null)
        {
            deEnd.Enabled = teEnd.Enabled = !ckbNoOver.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Plan.Type = ckbType.Checked ? PlanType.Repeat : PlanType.Once;
                Plan.Enable = ckbEnable.Checked;
                // 重复执行
                if (ckbType.Checked)
                {
                    Plan.PlanUnit = (PlanTimeUnit)((ImageComboBoxItem)cmbPlanUnit.SelectedItem).Value;
                    Plan.Interval = (int)seSpan.Value;
                    Plan.PlanStartTime = deStart.DateTime.Date.AddHours(teStart.Time.Hour).AddMinutes(teStart.Time.Minute).AddSeconds(teStart.Time.Second);
                    Plan.NoOverTime = ckbNoOver.Checked;
                    Plan.NextRunTime = Plan.PlanStartTime;
                    if (ckbNoOver.Checked)
                        Plan.PlanEndTime = DateTime.MaxValue;
                    else
                        Plan.PlanEndTime = deEnd.DateTime.Date.AddHours(teEnd.Time.Hour).AddMinutes(teEnd.Time.Minute).AddSeconds(teEnd.Time.Second);
                }
                // 执行一次
                else
                {
                    Plan.NextRunTime = deDateOnce.DateTime.Date.AddHours(teTimeOnce.Time.Hour).AddMinutes(teTimeOnce.Time.Minute).AddSeconds(teTimeOnce.Time.Second);
                    teStartEnableTime.Time = DateTime.Now.Date;
                    teEndEnableTime.Time = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                }
                //Plan.StartEnableTime = teStartEnableTime.Time;
                //Plan.EndEnableTime = teEndEnableTime.Time;
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Forms--PlanConfigFormException");
            }
            DialogResult = DialogResult.OK;
        }

        private void deStart_EnabledChanged(object sender, EventArgs e)
        {
            lbEnable.Enabled = lineEnable.Enabled = lbEnableStart.Enabled = lbEnableEnd.Enabled = teStartEnableTime.Enabled = teEndEnableTime.Enabled = deStart.Enabled;
        }
    }
}
