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
using TaskPlatform.TaskInterface;
using TaskPlatform.PlatformLog;
using System.Reflection;
using TaskPlatform.Forms;
using DevExpress.XtraEditors;
using TaskPlatform.TaskDomain;
using TaskPlatform.PlanEngine;
using System.Diagnostics;
using System.IO;

namespace TaskPlatform.Controls
{
    /// <summary>
    /// 计划任务配置台和运行情况展示台
    /// </summary>
    public partial class TaskStage : UserControl
    {
        public TaskStage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 计划任务运行时域加载器
        /// </summary>
        public TaskDomain.DomainLoader Loader { get; set; }
        private string _taskName = "";
        private int execCount = 0;

        private void TaskStage_Paint(object sender, PaintEventArgs e)
        {
            if (Parent != null && !Parent.Size.IsEmpty)
            {
                this.Size = Parent.ClientSize;
            }
        }

        private void TaskStage_Load(object sender, EventArgs e)
        {
            txtTaskDescription.BackColor = txtTaskDescription.Parent.BackColor;
            txtRunLog.BackColor = txtRunLog.Parent.BackColor;
            txtRunningLog.BackColor = txtRunLog.BackColor;
            txtDomainName.BackColor = txtFilePath.BackColor = txtRunLog.BackColor;
            txtDomainName.Text = txtFilePath.Text = string.Empty;
            if (Loader == null)
            {
                XtraMessageBox.Show(this, "获取计划任务信息错误。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnRunOnce.Enabled = btnStartPlan.Enabled = btnStopPlan.Enabled = btnConfigPlan.Enabled = btnEditConfigs.Enabled = false;
            ThreadPool.QueueUserWorkItem(SetInfo);
        }

        private void SetInfo(object state)
        {
            _taskName = Loader.TaskOperator.TaskName();
            //string[] methods = Loader.TaskOperator.GetPublicMethodNames();
            this.BeginInvoke(new MethodInvoker(() =>
            {
                txtDomainName.Text = Loader.DomainName;
                txtFilePath.Text = Path.GetDirectoryName(Loader.FileName);
                gpcTaskInfo.Text = string.Format("{0}的运行信息", _taskName);
                gpcRunLog.Text = string.Format("{0}的运行日志", _taskName);
                txtTaskDescription.Text = "计划任务描述：" + Loader.TaskOperator.TaskDescription();
                btnStopPlan.Enabled = Loader.Plan != null;
                btnStartPlan.Enabled = (Loader.Plan != null) && (!btnStopPlan.Enabled);
                if (Loader.Plan != null && !Loader.Plan.Enable)
                {
                    btnStartPlan.Enabled = true;
                    btnStopPlan.Enabled = false;
                }
                btnRunOnce.Enabled = btnConfigPlan.Enabled = btnEditConfigs.Enabled = true;
                ckbScroll.Location = new Point(btnStopPlan.Location.X, ckbScroll.Location.Y);
                if (!Loader.IsMirrorTask)
                {
                    btnRemoveMirror.Caption = "清除镜像";
                }
                else
                {
                    txtDomainName.ForeColor = Color.Gray;
                }
            }));
        }

        private void btnRunOnce_Click(object sender, EventArgs e)
        {
            txtRunningLog.Clear();
            ThreadPool.QueueUserWorkItem(RunOnce, Loader);
        }

        private void RunOnce(object state = null)
        {
            try
            {
                TaskDomain.DomainLoader loader = state as TaskDomain.DomainLoader;
                RunTaskResult result = PlatformForm.Form.RunOnce(loader.DomainName);
                if (result.Success)
                {
                    if (txtRunningLog.IsHandleCreated)
                    {
                        txtRunningLog.BeginInvoke(new MethodInvoker(delegate()
                        {
                            XtraMessageBox.Show(this, "计划任务执行成功。\n\n执行结果：\n" + result.Result, "执行成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                    }
                }
                else
                {
                    if (txtRunningLog.IsHandleCreated)
                    {
                        txtRunningLog.BeginInvoke(new MethodInvoker(delegate()
                        {
                            XtraMessageBox.Show(this, "计划任务执行失败。\n\n请参考：\n" + result.Result, "执行错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                if (txtRunningLog.IsHandleCreated)
                {
                    txtRunningLog.BeginInvoke(new MethodInvoker(delegate()
                    {
                        Log.CustomWrite(ex.ToString(), "TaskPlatform.Controls--TaskStageException");
                        XtraMessageBox.Show(this, "计划任务执行异常。\n\n请参考：\n" + ex.ToString(), "执行错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            }
        }

        private void btnStartPlan_Click(object sender, EventArgs e)
        {
            PlatformForm.Form.StartPlan(Loader.Plan.PlanName);
            btnStopPlan.Enabled = true;
            btnStartPlan.Enabled = false;
        }

        private void btnStopPlan_Click(object sender, EventArgs e)
        {
            PlatformForm.Form.StopPlan(Loader.Plan.PlanName);
            btnStopPlan.Enabled = false;
            btnStartPlan.Enabled = true;
        }

        private void btnConfigPlan_Click(object sender, EventArgs e)
        {
            Plan configedplan = ConfigPlan(Loader);
            if (configedplan != null)
            {
                btnStartPlan.Enabled = !configedplan.Enable;
                btnStopPlan.Enabled = configedplan.Enable;
            }
            else
            {
                btnStartPlan.Enabled = btnStopPlan.Enabled = false;
            }
        }

        internal static Plan ConfigPlan(DomainLoader loader)
        {
            using (Forms.PlanConfigForm pcf = new Forms.PlanConfigForm())
            {
                PlanEngine.Plan plan = null;
                if (loader.Plan == null)
                {
                    plan = new PlanEngine.Plan();
                    plan.PlanName = loader.DomainName;
                    plan.PlanUnit = PlanEngine.PlanTimeUnit.Second;
                    plan.Type = PlanEngine.PlanType.Repeat;
                    plan.NoOverTime = true;
                    plan.Enable = true;
                }
                else
                    plan = loader.Plan;
                pcf.Plan = plan;
                pcf.TaskName = loader.TaskOperator.TaskName();
                if (pcf.ShowDialog() == DialogResult.OK)
                {
                    loader.Plan = pcf.Plan;
                    TaskPlatform.PlatformForm.PlansEngine.AddPlan(loader.Plan);
                    try
                    {
                        LoaclDataCacheObject item = new LoaclDataCacheObject();
                        item.Key = "Plan:" + loader.Plan.PlanName;
                        item.KeyForUpdate = item.Key;
                        item.Value = loader.Plan;
                        TaskPlatform.PlatformForm.PlanPlatformCache.Update(item);
                        // 如果是启用状态，就载入计划引擎
                        if (loader.Plan.Enable)
                        {
                            TaskPlatform.PlatformForm.PlansEngine.AddPlan(loader.Plan);
                        }
                        PlatformForm.Form.UpdateIcon(loader.Plan.PlanName, loader.Plan.Enable);

                    }
                    catch (Exception ex)
                    {
                        Log.CustomWrite(ex.ToString(), "TaskPlatform.Controls--TaskStage[ConfigPlan]Exception");
                    }
                }
            }
            return loader.Plan;
        }

        private void btnEditConfigs_Click(object sender, EventArgs e)
        {
            if (Loader.TaskOperator.SupportAdvancedFeatures)
            {
                List<TaskParameter> parameters = Loader.TaskOperator.GetParameters();
                if (parameters.Count > 0)
                {
                    // 支持高级功能并且有计划任务参数则直接使用高级参数配置
                    ShowTaskParametersEditor(parameters);
                }
                else
                {
                    ShowConfigEditor();
                }
            }
            else
            {
                ShowConfigEditor();
            }
        }

        private void ShowConfigEditor()
        {
            Forms.ConfigsEditor editor = new Forms.ConfigsEditor();
            editor.Configs = Loader.TaskOperator.UploadConfig();
            editor.ShowDialog();
            if (editor.Configs == null)
            {
                editor.Configs = new Dictionary<string, string>();
            }
            Loader.TaskOperator.DownloadConfig(editor.Configs);
            string key = "TaskConfig:" + Loader.DomainName;
            PlatformForm.PlanPlatformCache.Update(new LoaclDataCacheObject(key, Loader.TaskOperator.UploadConfig(), key));
            PlatformForm.Form.UpdateItemCaption(Loader);
            try
            {
                txtTaskDescription.Text = "计划任务描述：" + Loader.TaskOperator.TaskDescription();
            }
            catch { }
            editor.Dispose();
        }

        private void ShowTaskParametersEditor(List<TaskParameter> parameters)
        {
            Forms.TaskParametersEditor editor = new Forms.TaskParametersEditor();
            editor.Parameters = parameters;
            editor.ShowDialog();
            if (editor.Parameters == null)
            {
                editor.Parameters = new List<TaskParameter>();
            }
            Loader.TaskOperator.SetParameters(editor.Parameters);
            string key = "TaskParameters:" + Loader.DomainName;
            PlatformForm.PlanPlatformCache.Update(new LoaclDataCacheObject(key, Loader.TaskOperator.GetParameters(), key));
            PlatformForm.Form.UpdateItemCaption(Loader);
            try
            {
                txtTaskDescription.Text = "计划任务描述：" + Loader.TaskOperator.TaskDescription();
            }
            catch { }
            editor.Dispose();
        }

        public void ExecutedPlan(object sender, ExecutedPlanEventArgs e)
        {
            try
            {
                txtRunningLog.BeginInvoke(new MethodInvoker(() =>
                {
                    #region 如果是本任务计划的消息

                    if (e.PlanName == Loader.DomainName)
                    {
                        #region 处理执行结束后的返回消息
                        execCount++;
                        if (execCount > 20)
                        {
                            txtRunLog.Clear();
                            execCount = 0;
                        }
                        if (!e.Success)
                        {
                            lbLastRunTime.ForeColor = Color.Red;
                        }
                        else
                        {
                            lbLastRunTime.ForeColor = lbLastRunInfo.ForeColor;
                        }
                        lbLastRunTime.Text = e.ExecuteTime.ToString("yyyy-MM-dd HH:mm:ss");
                        int length = txtRunLog.TextLength;
                        txtRunLog.AppendText(string.Format("{0}".PadLeft(11, '=').PadRight(19, '='), lbLastRunTime.Text));
                        txtRunLog.AppendText(Environment.NewLine);
                        txtRunLog.AppendText(string.Format("执行结果[等待 {0} 实耗:{1}]：\n{2}", e.ProcessTraceTimeSpan.ToString("G"), e.ProcessTimeSpan.ToString("G"), e.RunResult));
                        txtRunLog.AppendText(Environment.NewLine);
                        if (!e.Success)
                        {
                            txtRunLog.Select(length, txtRunLog.TextLength - length);
                            txtRunLog.SelectionColor = Color.Red;
                        }
                        txtRunLog.Select(txtRunLog.TextLength, 0);
                        if (ckbScroll.Checked)
                            txtRunLog.ScrollToCaret();

                        #endregion
                    }

                    #endregion
                }));
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Controls--TaskStageException");
            }
        }

        public void ExecutePlan(object sender, ExecutingPlanEventArgs e)
        {
            if (txtRunningLog.IsHandleCreated)
            {
                if (e.PlanName == Loader.DomainName)
                {
                    txtRunningLog.BeginInvoke(new MethodInvoker(() =>
                    {
                        if (e.ExecuteType == ExecuteType.Before)
                        {
                            txtRunningLog.Clear();
                            txtRunningLog.AppendText(string.Format("======{0}======", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                            txtRunningLog.AppendText(Environment.NewLine);
                        }
                        else if (e.ExecuteType == ExecuteType.Executing)
                        {
                            txtRunningLog.AppendText(e.Information);
                            txtRunningLog.AppendText(Environment.NewLine);
                        }
                        else if (e.ExecuteType == ExecuteType.After)
                        {
                            txtRunningLog.AppendText(string.Format("======{0}======", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                    }));
                }
            }
        }

        /// <summary>
        /// 接收计划任务更新事件
        /// </summary>
        /// <param name="taskName"></param>
        public void TaskUpdate(string taskName)
        {
            if (txtRunningLog.IsHandleCreated)
            {
                if (taskName == Loader.DomainName)
                {
                    txtRunningLog.BeginInvoke(new MethodInvoker(() =>
                    {
                        _taskName = Loader.TaskOperator.TaskName();
                        gpcTaskInfo.Text = string.Format("{0}的运行信息", _taskName);
                        gpcRunLog.Text = string.Format("{0}的运行日志", _taskName);
                        txtTaskDescription.Text = "计划任务描述：" + Loader.TaskOperator.TaskDescription();
                    }));
                }
            }
        }

        private void taskRightMenuOther_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show(sender.ToString());
        }

        private void spliterContainer_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                spliterContainer.SplitterPosition = (spliterContainer.Width / 10) * 6;
            }
            catch { }
            try
            {
                txtFilePath.Width = txtTaskDescription.Width - txtFilePath.Location.X;
            }
            catch { }
        }

        private void btnUnload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PlatformForm.Form.UnLoadTaskFull(Loader.DomainName, true);
        }

        private void btnOpenDirectory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Process.Start("explorer", Path.GetDirectoryName(Loader.FileName));
            }
            catch (Exception ex)
            {
                PlatformForm.Form.ShowError(ex.ToString());
            }
        }

        private void btnSetAlarm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Loader == null)
                return;
            Forms.AlarmConfigForm form = new AlarmConfigForm();
            form.Loader = Loader;
            form.ShowDialog();
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Loader == null)
                return;
            List<string> mirrorList = MirrorService.GetTaskMirror(Loader.TaskName);
            int type = 1;
            if (Loader.IsMirrorTask || mirrorList.Count > 0)
            {
                switch (PlatformForm.Form.ShowDecide(Loader.IsMirrorTask ? string.Format("该任务为主任务[{0}]的镜像任务。\n\n是否同时重新加载主任务以及其它可能的镜像任务？", Loader.TaskName) : string.Format("该任务为主任务。并且存在以下[{0}]个镜像：\n\n{1}\n\n是否同时重新加载所有的镜像任务？", mirrorList.Count, string.Join("\n", mirrorList))))
                {
                    case DialogResult.Yes:
                        type = 2;
                        break;
                    case DialogResult.No:
                        type = 1;
                        break;
                    default:
                        type = 0;
                        break;
                }
            }
            if (type > 0)
            {
                PlatformForm.Form.ReloadTask(Loader.DomainName, true);
                if (type > 1)
                {
                    if (Loader.IsMirrorTask)
                    {
                        PlatformForm.Form.ReloadTask(Loader.TaskName, false);
                        mirrorList.Remove(Loader.MirrorName);
                    }
                    mirrorList.ForEach(mirrorName =>
                    {
                        PlatformForm.Form.ReloadTask(string.Concat(Loader.TaskName, "-", mirrorName), false);
                    });
                }
            }
        }

        private void btnCreateMirror_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Loader.IsMirrorTask)
            {
                PlatformForm.Form.ShowInfo(string.Format("该任务为主任务[{0}]的镜像任务。\n\n即将创建的镜像也将是该主任务的镜像任务。", Loader.TaskName));
            }
            ManageTextForm form = new ManageTextForm();
            form.Text = "请输入镜像名称";
            form.LabelText = "镜像名称";
            form.SuperTip = btnCreateMirror.SuperTip;
            form.ShowDialog();
            if (form.IsOK)
            {
                try
                {
                    MirrorService.AddTaskMirror(Loader.TaskName, form.StringText);
                    PlatformForm.Form.LoadTask(Loader.FileName, true);
                }
                catch (Exception ex)
                {
                    PlatformForm.Form.ShowError(ex.ToString());
                }
            }
        }

        private void btnRemoveMirror_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Loader.IsMirrorTask)
            {
                if (PlatformForm.Form.ShowConfirm(string.Format("将永久删除该镜像任务。不会删除文件和警报信息，不影响主任务和其他镜像任务。\n\n您确认要删除该任务的镜像[{0}]吗？", Loader.MirrorName)))
                {
                    try
                    {
                        MirrorService.RemoveTaskMirror(Loader.TaskName, Loader.MirrorName);
                        PlatformForm.Form.UnLoadTaskFull(Loader.DomainName, false, false);
                    }
                    catch (Exception ex)
                    {
                        PlatformForm.Form.ShowError(ex.ToString());
                    }
                }
            }
            else
            {
                List<string> mirrorList = MirrorService.GetTaskMirror(Loader.TaskName);
                if (mirrorList.Count < 1)
                {
                    PlatformForm.Form.ShowInfo("该任务并不存在镜像任务。");
                }
                else
                {
                    if (PlatformForm.Form.ShowConfirm("将永久删除该主任务的所有镜像任务。不会删除文件和警报信息，不影响主任务。\n\n您确认要清除该任务的所有镜像任务吗？"))
                    {
                        try
                        {
                            MirrorService.ClearTaskMirror(Loader.TaskName);
                            mirrorList.ForEach(mirrorName =>
                            {
                                PlatformForm.Form.UnLoadTaskFull(string.Concat(Loader.TaskName, "-", mirrorName), false, false);
                            });
                        }
                        catch (Exception ex)
                        {
                            PlatformForm.Form.ShowError(ex.ToString());
                        }
                    }
                }
            }
        }
    }
}
