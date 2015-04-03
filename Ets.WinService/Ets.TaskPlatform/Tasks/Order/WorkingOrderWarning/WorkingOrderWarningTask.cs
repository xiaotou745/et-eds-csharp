using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace WorkingOrderWarning
{
    public class WorkingOrderWarningTask : AbstractTask
    {
        private static IOrderService mOrderService
        {
            get
            {
                return new OrderService();
            }
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "WMS未领取工单提醒";
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "工单流转到WMS后针对各个仓库进行邮件提醒";
        }
        /// <summary>
        /// 服务实现
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 验证服务配置
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串"]))
            {
                ShowRunningLog("请填写数据库连接字符串！");
                runTaskResult.Result = "请填数据库连接字符串！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            if (string.IsNullOrWhiteSpace(CustomConfig["EmailAddress"]))
            {
                ShowRunningLog("请填写邮件地址！");
                runTaskResult.Result = "请填写邮件地址！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            #endregion
            #region 对象声明
            string strcon = CustomConfig["数据库连接字符串"];
            int mreCount = 0;
            #endregion
            #region 线程开关控制及线程调用
            foreach (var mm in CustomConfig)
            {
                if (mm.Key.Contains("-") && mm.Value == "开")
                {
                    ++mreCount;
                }
            }
            ManualResetEvent[] doneEvents = new ManualResetEvent[mreCount];
            try
            {
                int i = 0;
                foreach (var cc in CustomConfig)
                {
                    if (!cc.Key.Contains("-"))
                    {
                        continue;
                    }
                    if (cc.Value == "关")
                    {
                        continue;
                    }
                    string[] key = cc.Key.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    int threadIndex = Convert.ToInt32(key[0]);
                    int wareId = Convert.ToInt32(key[0]);
                    doneEvents[i] = new ManualResetEvent(false);
                    TasksCommon tasksCommon = new TasksCommon(wareId, strcon, threadIndex, cc.Key,
                                                                doneEvents[i]);
                    ThreadPool.QueueUserWorkItem(ThreadPoolCallback, tasksCommon);
                    i++;
                }
            }
            catch (Exception ex)
            {
                runTaskResult.Result = ex.ToString();
                runTaskResult.Success = false;
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
            }
            WaitHandle.WaitAll(doneEvents);
            ShowRunningLog("=============本次WMS未领取工单提醒结束=============");
            runTaskResult.Result = "本次WMS未领取工单提醒结束";
            runTaskResult.Success = true;
            Thread.Sleep(3000);
            return runTaskResult;
            #endregion
        }
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串"))
                CustomConfig.Add("数据库连接字符串", "Server=58.83.215.67;Database=jxworkorder; User=cong_wms_jxerp;Password=cong_jxwms_read;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;default command timeout=300;Connection Timeout=120;Connection Reset=true;");
            if (!CustomConfig.ContainsKey("北京仓邮件地址"))
                CustomConfig.Add("北京仓邮件地址", "houlili@jiuxian.com;guoxia@jiuxian.com");
            if (!CustomConfig.ContainsKey("广州仓邮件地址"))
                CustomConfig.Add("广州仓邮件地址", "jiangjiahui@jiuxian.com;lizu@jiuxian.com;yinchangqi@jiuxian.com");
            if (!CustomConfig.ContainsKey("上海仓邮件地址"))
                CustomConfig.Add("上海仓邮件地址", "chenmingsheng@jiuxian.com;chenjie@jiuxian.com;wangyumeng@jiuxian.com");
            if (!CustomConfig.ContainsKey("武汉仓邮件地址"))
                CustomConfig.Add("武汉仓邮件地址", "dongxin@jiuxian.com;liaojunzhou@jiuxian.com;2880323086@qq.com");
            if (!CustomConfig.ContainsKey("天津仓邮件地址"))
                CustomConfig.Add("天津仓邮件地址", "sunweiwei@jiuxian.com;zhangyuli@jiuxian.com;zhangwenhong@jiuxian.com");
            if (!CustomConfig.ContainsKey("成都仓邮件地址"))
                CustomConfig.Add("成都仓邮件地址", "changyujiang@jiuxian.com");
            if (!CustomConfig.ContainsKey("抄送邮件地址"))
                CustomConfig.Add("抄送邮件地址", "wangchaoyang@jiuxian.com,jiweiping@jiuxian.com,sunyinyin@jiuxian.com");
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangchaoyang@jiuxian.com");
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "开");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "开");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "开");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "开");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "开");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "开");
            if (!CustomConfig.ContainsKey("工单查询时间/天"))
                CustomConfig.Add("工单查询时间/天", "1");
            return CustomConfig;
        }

        /// <summary>
        /// 线程操作
        /// </summary>
        /// <param name="threadContext"></param>
        private void ThreadPoolCallback(object threadContext)
        {
            #region 声明对象
            RunTaskResult runTaskResult = new RunTaskResult();
            TasksCommon tasksCommon = (TasksCommon)threadContext;
            WorkingOrderDto workingOrder = new WorkingOrderDto();
            IList<WorkingOrderDto> workingOrderlist = new List<WorkingOrderDto>();
            #endregion
            try
            {
                #region 查询未处理

                Config config = new Config
                {
                    Limit = CustomConfig["工单查询时间/天"].ToInt(),
                    ReadConnectionString = tasksCommon.ConnStr,
                };
                StringBuilder sbHtmlBody = new StringBuilder(Task.Common.Parameters.SystemMsgForEWS);
                workingOrder.WareId = tasksCommon.WareId;
                ShowRunningLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单获取中-->");
                WriteLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单获取中-->");
                workingOrderlist = mOrderService.GetWorkingOrderInfoBLL(config, workingOrder);
                if (workingOrderlist == null)
                    return;
                if (workingOrderlist.Count > 0 && string.IsNullOrEmpty(workingOrderlist[0].ExceptionStr))
                {
                    #region 查到未领取工单,发邮件提醒
                    sbHtmlBody.AppendLine("【" + tasksCommon.DealFullName.Substring(2, 3) + "】中共有 <strong>" + workingOrderlist.Count + "</strong> 个<strong>未领取</strong>工单，请找相关人员尽快处理了。非常感谢！<br />");
                    Dictionary<string, string> dicHead = new Dictionary<string, string>();
                    dicHead.Add("WorkerderSn", "工单号");
                    dicHead.Add("ChannelOrdersn", "订单号");
                    dicHead.Add("CreateTime", "工单创建时间");
                    dicHead.Add("UpdateTime", "最后一次处理时间");
                    sbHtmlBody.Append(SendEmailCommon.CreateHtmlBody(dicHead, workingOrderlist).ToString());
                    string emailTitle = "【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单提醒";
                    string emailAddress = CustomConfig[tasksCommon.DealFullName.Substring(2, 3) + "邮件地址"];
                    string copyEmailAddress = CustomConfig["抄送邮件地址"];
                    SendEmailTo(sbHtmlBody.ToString(), emailAddress, emailTitle, copyEmailAddress, true);
                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单获取完成,共计:{" + workingOrderlist.Count + "}单";
                    ShowRunningLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单获取完成,共计:{" + workingOrderlist.Count + "}单");
                    WriteLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】WMS未领取工单获取完成,共计:{" + workingOrderlist.Count + "}单");
                    #endregion
                }
                else
                {
                    if (workingOrderlist.Count > 0)
                    {
                        ShowRunningLog(workingOrderlist[0].ExceptionStr);
                        runTaskResult.Success = false;
                        runTaskResult.Result = workingOrderlist[0].ExceptionStr;
                        SendEmailTo(TaskName() + workingOrderlist[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        #region 没有需要提醒的订单
                        runTaskResult.Success = true;
                        runTaskResult.Result = "<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要提醒的WMS未领取工单-->";
                        ShowRunningLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要提醒的WMS未领取工单-->");
                        WriteLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要提醒的WMS未领取工单-->");
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region 捕获到异常,发邮件通知处理
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                #endregion
            }
            finally
            {
                #region 线程结束
                tasksCommon.DoneEvent.Set();
                #endregion
            }

        }
    }
}
