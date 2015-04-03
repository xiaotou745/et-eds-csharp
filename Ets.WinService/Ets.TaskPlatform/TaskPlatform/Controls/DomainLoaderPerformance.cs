using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.Commom;
using TaskPlatform.TaskDomain;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraCharts;

namespace TaskPlatform.Controls
{
    public partial class DomainLoaderPerformance : DevExpress.XtraEditors.XtraUserControl
    {
        public DomainLoaderPerformance()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        ~DomainLoaderPerformance()
        {
            try
            {
                tmrWork.Dispose();
            }
            catch { }
            try
            {
                GC.Collect();
            }
            catch { }
        }

        Dictionary<string, List<SeriesPoint>> points = new Dictionary<string, List<SeriesPoint>>();
        Series selectedSeries;
        bool hasOneAlter = false;
        bool isInit = true;

        private void DomainLoaderPerformance_Paint(object sender, PaintEventArgs e)
        {
            if (this.Parent != null && !this.Parent.Size.IsEmpty)
            {
                this.Size = this.Parent.Size;
            }
        }

        private void DomainLoaderPerformance_Load(object sender, EventArgs e)
        {
            if (!AppDomain.MonitoringIsEnabled)
            {
                this.Enabled = false;
                XtraMessageBox.Show("平台未开启监测功能！", "警告");
            }
            else
            {
                chartPerformance.Series.Clear();
                ccbTaskList.Properties.Items.Add(new SystemTask(true) { SystemName = "{TaskPlatform}", DisplayName = "计划任务主平台" });
                ccbTaskList.Properties.Items[ccbTaskList.Properties.Items.Count - 1].CheckState = CheckState.Checked;
                PlatformForm.DomainLoaderList.ForEach(loader =>
                {
                    try
                    {
                        ccbTaskList.Properties.Items.Add(new SystemTask(true) { SystemName = loader.DomainName, DisplayName = loader.TaskOperator.TaskName() });
                        //ccbTaskList.Properties.Items[ccbTaskList.Properties.Items.Count - 1].CheckState = CheckState.Checked;
                    }
                    catch { }
                });
                isInit = false;
                RefreshChart();
                tmrWork.Interval = (int)speInterval.Value * 1000;
                tmrWork.Start();
            }
        }

        private void speInterval_Properties_ValueChanged(object sender, EventArgs e)
        {
            tmrWork.Interval = (int)speInterval.Value * 1000;
        }

        private void tmrWork_Tick(object sender, EventArgs e)
        {
            RefreshChartValue();
        }

        private void ccbTaskList_EditValueChanged(object sender, EventArgs e)
        {
            RefreshChart();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshChartValue();
        }

        private void RefreshChart()
        {
            if (isInit)
                return;
            Dictionary<string, Series> tmp = new Dictionary<string, Series>();
            foreach (Series s in chartPerformance.Series)
            {
                tmp.Add(((SystemTask)s.Tag).SystemName, s);
            }
            foreach (CheckedListBoxItem item in ccbTaskList.Properties.Items)
            {
                SystemTask task = item.Value as SystemTask;
                if (item.CheckState == CheckState.Checked && !tmp.ContainsKey(task.SystemName))
                {
                    Series serieItem = new Series(item.Value.ToString(), ViewType.Spline);
                    serieItem.Tag = task;
                    if (!points.ContainsKey(task.SystemName))
                    {
                        points.Add(task.SystemName, new List<SeriesPoint>());
                    }
                    serieItem.Visible = true;
                    chartPerformance.Series.Add(serieItem);
                }
                else if (item.CheckState != CheckState.Checked && tmp.ContainsKey(task.SystemName))
                {
                    chartPerformance.Series.Remove(tmp[task.SystemName]);
                    points.Remove(task.SystemName);
                }
            }
            RefreshChartValue();
        }

        private void RefreshChartValue()
        {
            DomainPerformance performance;
            foreach (Series item in chartPerformance.Series)
            {
                if (item == null)
                    continue;
                SystemTask task = (SystemTask)item.Tag;
                if (task == null)
                    continue;
                if (task.SystemName == "{TaskPlatform}")
                {
                    performance = new DomainPerformance();
                }
                else
                {
                    DomainLoader loader = (from f in PlatformForm.DomainLoaderList
                                           where f.DomainName == task.SystemName
                                           select f).FirstOrDefault();
                    if (loader == null)
                        continue;
                    performance = loader.TaskOperator.GetDomain().CreateInstanceFromAndUnwrap(TaskObject.AssemblyLocation, TaskObject.PerformanceTypeName) as DomainPerformance;
                }
                SeriesPoint point = new SeriesPoint(DateTime.Now, performance.MonitoringSurvivedMemorySize / 1024);
                point.Tag = point.DateTimeArgument;
                point.Argument = string.Concat(point.DateTimeArgument.Minute.ToString("D2"), ":", point.DateTimeArgument.Second.ToString("D2"));
                item.Points.Add(point);
                List<SeriesPoint> sps = points[task.SystemName];
                sps.Add(point);
                var removePoints = (from f in sps
                                    where (DateTime)f.Tag < DateTime.Now.AddMinutes(0 - ((int)speTimeSpan.Value))
                                    select f).ToList();
                removePoints.ForEach(p =>
                {
                    item.Points.Remove(p);
                    sps.Remove(p);
                });
                points[task.SystemName] = sps;
            }
        }

        private void chartPerformance_VisibleChanged(object sender, EventArgs e)
        {
            if (this.chartPerformance.Series.Count > 0)
            {
                selectedSeries = chartPerformance.Series[0];
                chartPerformance.SetObjectSelection(selectedSeries);
            }
        }

        private void chartPerformance_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            ShowSelectedTaskInfo(e);
        }

        private void chartPerformance_ObjectSelected(object sender, HotTrackEventArgs e)
        {
            ShowSelectedTaskInfo(e, true);
        }

        private void ShowSelectedTaskInfo(HotTrackEventArgs args, bool showInfo = false)
        {
            if (!(args.Object is Series))
            {
                args.Cancel = true;
            }
            else
            {
                selectedSeries = (args.Object as Series) ?? selectedSeries;
            }
            if (showInfo && !hasOneAlter)
            {
                hasOneAlter = true;
                try
                {
                    DomainPerformance performance;
                    SystemTask task = selectedSeries.Tag as SystemTask;
                    if (task == null)
                        return;
                    if (task.SystemName == "{TaskPlatform}")
                    {
                        performance = new DomainPerformance();
                    }
                    else
                    {
                        DomainLoader loader = (from f in PlatformForm.DomainLoaderList
                                               where f.DomainName == task.SystemName
                                               select f).FirstOrDefault();
                        if (loader == null)
                            return;
                        performance = loader.TaskOperator.GetDomain().CreateInstanceFromAndUnwrap(TaskObject.AssemblyLocation, TaskObject.PerformanceTypeName) as DomainPerformance;
                    }
                    alcTaskInfo.Show(PlatformForm.Form, string.Concat(task.DisplayName, "的性能指标"), string.Concat("标识：", performance.ID, "\n内存占用(KB):", performance.MonitoringSurvivedMemorySize / 1024, "\n总计已分配(KB)：", performance.MonitoringTotalAllocatedMemorySize, "\nCPU时间：", performance.MonitoringTotalProcessorTime.ToString("G")));
                }
                catch
                {
                    hasOneAlter = false;
                }
            }
        }

        private void alcTaskInfo_FormClosing(object sender, DevExpress.XtraBars.Alerter.AlertFormClosingEventArgs e)
        {
            hasOneAlter = false;
        }
    }
}
