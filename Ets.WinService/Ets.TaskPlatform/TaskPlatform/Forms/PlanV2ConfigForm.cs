using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.PlanEngine.V2;
using System.Threading;
using TaskPlatform.PlanEngine;
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace TaskPlatform.Forms
{
    public partial class PlanV2ConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public PlanV2ConfigForm(PlanV2 plan)
        {
            InitializeComponent();
            this.Height = 590;
            _plan = plan;
            if (plan == null)
            {
                _safePlan = null;
            }
            else
            {
                _safePlan = plan.Clone();
            }
        }
        public PlanV2ConfigForm()
            : this(null)
        { }

        #region 字段和属性

        /// <summary>
        /// 要配置的执行计划
        /// </summary>
        private PlanV2 _plan;
        /// <summary>
        /// 要配置的执行计划
        /// </summary>
        public PlanV2 Plan
        {
            get { return _plan; }
        }

        /// <summary>
        /// 要配置的执行计划(安全，用在配置窗体内)
        /// </summary>
        private PlanV2 _safePlan;
        /// <summary>
        /// 复选框已触发事件
        /// </summary>
        private bool fired = false;

        private bool _isOK = false;
        /// <summary>
        /// 是否配置完成
        /// </summary>
        public bool IsOK
        {
            get { return _isOK; }
            set { _isOK = value; }
        }

        #endregion

        private void PlanV2ConfigForm_Load(object sender, EventArgs e)
        {
            if (_safePlan == null)
            {
                _safePlan = new PlanV2();
            }

            _safePlan.PropertiesChanged += new UpdatePropertyHandler(_safePlan_PropertyChanged);
            this.Enabled = true;
            ThreadPool.QueueUserWorkItem(InitData);
        }

        private void _safePlan_PropertyChanged()
        {
            txtPlanDescription.Text = _safePlan.PlanDescription;
        }

        private void InitData(object state)
        {
            DataTable table;

            #region 绑定计划类型

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<PlanType>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            luePlanType.Invoke(new MethodInvoker(delegate()
            {
                luePlanType.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定频率执行类型

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<PlanSpanType>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { "每" + type.GetEnumDescription(), type.ToString() }, true);
            });
            lueSpanType.Invoke(new MethodInvoker(delegate()
            {
                lueSpanType.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定月份

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<MonthOfYear>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            cbeMonthList.Invoke(new MethodInvoker(delegate()
            {
                cbeMonthList.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定天

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<DayOfMonth>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            cbeDayList.Invoke(new MethodInvoker(delegate()
            {
                cbeDayList.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定周序号

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<WeekNumber>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            cbeWeekNumberList.Invoke(new MethodInvoker(delegate()
            {
                cbeWeekNumberList.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定周的天

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<DayOfWeekV2>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            cbeWeekList.Invoke(new MethodInvoker(delegate()
            {
                cbeWeekList.Properties.DataSource = table;
            }));

            #endregion

            #region 绑定每天频率执行间隔类型

            table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            EnumDescription.GetEnumValues<DayFrequencyUnit>().ForEach(type =>
            {
                table.LoadDataRow(new object[] { type.GetEnumDescription(), type.ToString() }, true);
            });
            lueDaySpanUnit.Invoke(new MethodInvoker(delegate()
            {
                lueDaySpanUnit.Properties.DataSource = table;
            }));

            #endregion

            ThreadPool.QueueUserWorkItem(BindPlanV2);
        }

        private void BindPlanV2(object state)
        {
            txtPlanName.Invoke(new MethodInvoker(delegate()
            {
                txtPlanName.Text = _safePlan.PlanName;
                luePlanType.EditValue = _safePlan.Type.ToString();
                ckbEnable.Checked = _safePlan.Enable;
                deOnceDate.DateTime = _safePlan.ExecuteTime;
                teOnceTime.Time = _safePlan.ExecuteTime;
                lueSpanType.EditValue = _safePlan.FrequencyOfPlan.SpanType.ToString();
                seDaily.Value = seWeekly.Value = _safePlan.FrequencyOfPlan.SpanValue;
                var list = (from f in grpSpanTypeWeekly.Controls.Cast<Control>()
                            where f is CheckEdit
                            select (CheckEdit)f).ToList();
                var checkedList = (from f in _safePlan.FrequencyOfPlan.DayOfWeekList
                                   select f.GetEnumDescription()).ToList();
                list.ForEach(checkEdit =>
                {
                    checkEdit.Checked = checkedList.Contains(checkEdit.Text);
                });
                if (_safePlan.FrequencyOfDay.DayPlanType == PlanType.Once)
                {
                    ckbDayOnce.Checked = true;
                }
                else
                {
                    ckbDayRepeat.Checked = true;
                }
                teDayTime.Time = _safePlan.FrequencyOfDay.Time;
                seDaySpanValue.Value = _safePlan.FrequencyOfDay.Interval;
                lueDaySpanUnit.EditValue = _safePlan.FrequencyOfDay.Unit.ToString();
                teDayStartTime.Time = _safePlan.FrequencyOfDay.StartTime;
                teDayEndTime.Time = _safePlan.FrequencyOfDay.EndTime;
                dePlanStartTime.DateTime = _safePlan.ContinuousTime.StartTime;
                dePlanEndTime.DateTime = _safePlan.ContinuousTime.EndTime;
                ckbPlanNoEndTime.Checked = _safePlan.ContinuousTime.IsForever;
                txtPlanDescription.Text = _safePlan.PlanDescription;
            }));
        }

        /// <summary>
        /// 当执行类型改变时
        /// </summary>
        private void luePlanType_EditValueChanged(object sender, EventArgs e)
        {
            switch (luePlanType.EditValue.ToString())
            {
                case "Once":
                    grpOnce.Enabled = lbExecuteOnceTip.Visible = true;
                    grpPlanFrequency.Enabled = grpDayFrequency.Enabled = grpContinuousTime.Enabled = false;
                    break;
                case "Repeat":
                    grpOnce.Enabled = lbExecuteOnceTip.Visible = false;
                    grpPlanFrequency.Enabled = grpDayFrequency.Enabled = grpContinuousTime.Enabled = true;
                    break;
            }
            _safePlan.Type = (PlanType)Enum.Parse(typeof(PlanType), luePlanType.EditValue.ToString());
        }

        /// <summary>
        /// 当计划间隔类型改变时
        /// </summary>
        private void lueSpanType_EditValueChanged(object sender, EventArgs e)
        {
            switch (lueSpanType.EditValue.ToString())
            {
                case "Daily":
                    grpSpanTypeDaily.Visible = true;
                    grpSpanTypeWeekly.Visible = grpSpanTypeMonthly.Visible = false;
                    break;
                case "Weekly":
                    grpSpanTypeWeekly.Visible = true;
                    grpSpanTypeDaily.Visible = grpSpanTypeMonthly.Visible = false;
                    break;
                case "Monthly":
                    grpSpanTypeMonthly.Visible = true;
                    grpSpanTypeDaily.Visible = grpSpanTypeWeekly.Visible = false;
                    break;
            }
            _safePlan.FrequencyOfPlan.SpanType = (PlanSpanType)Enum.Parse(typeof(PlanSpanType), lueSpanType.EditValue.ToString());
        }

        /// <summary>
        /// 无结束日期选中改变时
        /// </summary>
        private void ckbPlanNoEndTime_CheckedChanged(object sender, EventArgs e)
        {
            dePlanEndTime.Enabled = !ckbPlanNoEndTime.Checked;
            _safePlan.ContinuousTime.IsForever = ckbPlanNoEndTime.Checked;
        }

        /// <summary>
        /// 当每月执行类型改变时
        /// </summary>
        private void AtTypeCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (fired)
                return;
            else
            {
                fired = true;
                if ((sender as CheckEdit) == ckbMonthlyDay)
                {
                    ckbMonthlyAt.Checked = !ckbMonthlyDay.Checked;
                }
                else
                {
                    ckbMonthlyDay.Checked = !ckbMonthlyAt.Checked;
                }
                if (ckbMonthlyAt.Checked)
                {
                    _safePlan.FrequencyOfPlan.AtTimeType = MonthExecuteType.Week;
                }
                else
                {
                    _safePlan.FrequencyOfPlan.AtTimeType = MonthExecuteType.Day;
                }
                cbeDayList.Enabled = ckbMonthlyDay.Checked;
                cbeWeekNumberList.Enabled = cbeWeekList.Enabled = ckbMonthlyAt.Checked;
                fired = false;
            }
        }

        /// <summary>
        /// 每天频率类型改变时
        /// </summary>
        private void DayFreType_CheckedChanged(object sender, EventArgs e)
        {
            if (fired)
                return;
            else
            {
                fired = true;
                if ((sender as CheckEdit) == ckbDayOnce)
                {
                    ckbDayRepeat.Checked = !ckbDayOnce.Checked;
                    _safePlan.FrequencyOfDay.DayPlanType = PlanType.Once;
                }
                else
                {
                    ckbDayOnce.Checked = !ckbDayRepeat.Checked;
                    _safePlan.FrequencyOfDay.DayPlanType = PlanType.Repeat;
                }
                teDayTime.Enabled = ckbDayOnce.Checked;
                seDaySpanValue.Enabled = lueDaySpanUnit.Enabled = teDayStartTime.Enabled = teDayEndTime.Enabled = ckbDayRepeat.Checked;
                fired = false;
            }
        }

        /// <summary>
        /// 计划名称改变时
        /// </summary>
        private void txtPlanName_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = txtPlanName.Text.Trim().Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _safePlan.PlanName = txtPlanName.Text.Trim();
            _isOK = true;
            _plan = _safePlan.Clone();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isOK = false;
            this.Close();
        }

        private void deOnceDateTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.ExecuteTime = deOnceDate.DateTime.Date.AddHours(teOnceTime.Time.Hour).AddMinutes(teOnceTime.Time.Minute).AddSeconds(teOnceTime.Time.Second);
        }

        private void seDaily_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfPlan.SpanValue = (int)seDaily.Value;
        }

        private void seWeekly_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfPlan.SpanValue = (int)seWeekly.Value;
        }

        private void ckbWeekDay_CheckedChanged(object sender, EventArgs e)
        {
            var checkList = (from f in grpSpanTypeWeekly.Controls.Cast<Control>()
                             where (f is CheckEdit) && f.Tag.ToString() == "WeekDay"
                             select f as CheckEdit).ToList();
            var list = (from f in checkList
                        where f.Checked
                        orderby int.Parse(f.Name.Replace("ckbWeek", "")) ascending
                        select (DayOfWeekV2)int.Parse(f.Name.Replace("ckbWeek", ""))).ToList();
            _safePlan.FrequencyOfPlan.DayOfWeekList = list;
        }

        private void cbeMonthList_Properties_EditValueChanged(object sender, EventArgs e)
        {
            List<MonthOfYear> monthOfYearList = new List<MonthOfYear>();
            Type type = typeof(MonthOfYear);
            foreach (CheckedListBoxItem item in cbeMonthList.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    monthOfYearList.Add((MonthOfYear)Enum.Parse(type, item.Value.ToString()));
                }
            }
            monthOfYearList = monthOfYearList.OrderBy(item => { return (int)item; }).ToList();
            _safePlan.FrequencyOfPlan.MonthOfYearList = monthOfYearList;
        }

        private void cbeDayList_EditValueChanged(object sender, EventArgs e)
        {
            List<DayOfMonth> dayOfMonthList = new List<DayOfMonth>();
            Type type = typeof(DayOfMonth);
            foreach (CheckedListBoxItem item in cbeDayList.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    dayOfMonthList.Add((DayOfMonth)Enum.Parse(type, item.Value.ToString()));
                }
            }
            dayOfMonthList = dayOfMonthList.OrderBy(item => { return (int)item; }).ToList();
            _safePlan.FrequencyOfPlan.DayOfMonthList = dayOfMonthList;
        }

        private void cbeWeekNumberList_EditValueChanged(object sender, EventArgs e)
        {
            List<WeekNumber> weekNumberList = new List<WeekNumber>();
            Type type = typeof(WeekNumber);
            foreach (CheckedListBoxItem item in cbeWeekNumberList.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    weekNumberList.Add((WeekNumber)Enum.Parse(type, item.Value.ToString()));
                }
            }
            weekNumberList = weekNumberList.OrderBy(item => { return (int)item; }).ToList();
            _safePlan.FrequencyOfPlan.WeekNumberList = weekNumberList;
        }

        private void cbeWeekList_EditValueChanged(object sender, EventArgs e)
        {
            List<DayOfWeekV2> dayOfWeekList = new List<DayOfWeekV2>();
            Type type = typeof(DayOfWeekV2);
            foreach (CheckedListBoxItem item in cbeWeekList.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    dayOfWeekList.Add((DayOfWeekV2)Enum.Parse(type, item.Value.ToString()));
                }
            }
            dayOfWeekList = dayOfWeekList.OrderBy(item => { return (int)item; }).ToList();
            _safePlan.FrequencyOfPlan.DayOfWeekListMonth = dayOfWeekList;
        }

        private void teDayTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfDay.Time = teDayTime.Time;
        }

        private void seDaySpanValue_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfDay.Interval = (int)seDaySpanValue.Value;
        }

        private void lueDaySpanUnit_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfDay.Unit = (DayFrequencyUnit)Enum.Parse(typeof(DayFrequencyUnit), lueDaySpanUnit.EditValue.ToString());
        }

        private void teDayStartTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfDay.StartTime = teDayStartTime.Time;
            DateTime dt = teDayStartTime.Time.Date.AddHours(teDayEndTime.Time.Hour).AddMinutes(teDayEndTime.Time.Minute).AddSeconds(teDayEndTime.Time.Second);
            if (dt < teDayStartTime.Time)
            {
                teDayEndTime.Time = teDayStartTime.Time;
            }
        }

        private void teDayEndTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.FrequencyOfDay.EndTime = teDayEndTime.Time;
        }

        private void dePlanStartTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.ContinuousTime.StartTime = dePlanStartTime.DateTime;
            dePlanEndTime.Properties.MinValue = dePlanStartTime.DateTime.Date;
        }

        private void dePlanEndTime_EditValueChanged(object sender, EventArgs e)
        {
            _safePlan.ContinuousTime.EndTime = dePlanEndTime.DateTime;
        }
    }
}