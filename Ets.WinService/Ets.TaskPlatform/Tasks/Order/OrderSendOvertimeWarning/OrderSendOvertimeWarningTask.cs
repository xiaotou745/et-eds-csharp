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

namespace OrderSendOvertimeWarning
{
    /// <summary>
    /// 订单发货超时报警发邮件（订单审核通过超过24小时）
    /// </summary>
    public class OrderSendOvertimeWarningTask : AbstractTask
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
            return "订单发货超时预警";
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "订单流程到仓库后超过24小时未发货发出预警邮件通知操作人员发货";
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
            ShowRunningLog("=============本次超时订单预警结束=============");
            runTaskResult.Result = "本次超时订单预警结束";
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
                CustomConfig.Add("数据库连接字符串", "Server=192.168.11.21;Database=jiuxianweb;User ID=select_limit;password=Only_in_jx_select;Pooling=false;");
            if (!CustomConfig.ContainsKey("北京仓邮件地址"))
                CustomConfig.Add("北京仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("广州仓邮件地址"))
                CustomConfig.Add("广州仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("上海仓邮件地址"))
                CustomConfig.Add("上海仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("武汉仓邮件地址"))
                CustomConfig.Add("武汉仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("天津仓邮件地址"))
                CustomConfig.Add("天津仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("成都仓邮件地址"))
                CustomConfig.Add("成都仓邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("抄送邮件地址"))
                CustomConfig.Add("抄送邮件地址", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "关");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "开");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "关");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "关");
            if (!CustomConfig.ContainsKey("超时时间配置"))
                CustomConfig.Add("超时时间配置", "24");
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
            erp_order erpOrder = new erp_order();
            IList<erp_order> orderlist = new List<erp_order>();
            #endregion
            try
            {
                #region 查询发货超时订单

                Config config = new Config
                    {
                        Limit = CustomConfig["超时时间配置"].ToInt(),
                        ReadConnectionString = tasksCommon.ConnStr,
                    };
                StringBuilder sbHtmlBody = new StringBuilder(Task.Common.Parameters.SystemMsgForEWS);
                erpOrder.ware_id = tasksCommon.WareId;
                ShowRunningLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】发货超时订单获取中-->");
                WriteLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】发货超时订单获取中-->");
                orderlist = mOrderService.GetErpOvertimeOrderInfoBLL(config, erpOrder);
                if (orderlist == null)
                    return;
                if (orderlist.Count > 0 && string.IsNullOrEmpty(orderlist[0].ExceptionStr))
                {
                    #region 查到发货超时订单,发邮件预警
                    sbHtmlBody.AppendLine("<br />各位好：<br />&nbsp;&nbsp;&nbsp;&nbsp; WMS系统【" + tasksCommon.DealFullName.Substring(2, 3) + "】中共有 <strong>" + orderlist.Count + "</strong> 个订单已超过【"+config.Limit+"】小时未出库，请找相关人员尽快处理了。客户可以尽快收到商品，非常感谢！<br />");
                    Dictionary<string,string> dicHead  =new Dictionary<string,string>();
                    dicHead.Add("channel_ordersn","订单号");
                    dicHead.Add("add_time","下单时间");
                    dicHead.Add("order_duration","时长（小时）");
                    dicHead.Add("opt_statusName","操作状态");
                    if (orderlist.Count >Task.Common.Parameters.MaxCountForEWS)
                    {
                        sbHtmlBody.AppendLine(Task.Common.Parameters.SystemMsgAttachment);
                    }
                    sbHtmlBody.Append(SendEmailCommon.CreateHtmlBody(dicHead,orderlist).ToString());
                    string emailTitle = "【"+tasksCommon.DealFullName.Substring(2, 3) + "】订单发货超时预警";
                    string emailAddress = CustomConfig[tasksCommon.DealFullName.Substring(2, 3) + "邮件地址"];
                    string copyEmailAddress = CustomConfig["抄送邮件地址"];
                    string attachName = string.Format("【{0}】{1}{2}.xls", tasksCommon.DealFullName.Substring(2, 3), "发货超时订单_", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //数据大于500条发附件
                    if (orderlist.Count > Task.Common.Parameters.MaxCountForEWS)
                    {
                        SendEmailTo(sbHtmlBody.ToString(), emailAddress, emailTitle, copyEmailAddress, true, attachName, dicHead, orderlist);
                    }
                    else
                    {
                        SendEmailTo(sbHtmlBody.ToString(), emailAddress, emailTitle, copyEmailAddress, true);
                    }
                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName.Substring(2, 3) + "】发货超时订单获取完成,共计:{" + orderlist.Count + "}单";
                    ShowRunningLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】发货超时订单获取完成,共计:{" + orderlist.Count + "}单");
                    WriteLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】发货超时订单获取完成,共计:{" + orderlist.Count + "}单");
                    #endregion
                }
                else
                {
                    if(orderlist.Count > 0)
                    {
                        ShowRunningLog(orderlist[0].ExceptionStr);
                        runTaskResult.Success = false;
                        runTaskResult.Result = orderlist[0].ExceptionStr;
                        SendEmailTo(TaskName() + orderlist[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        #region 没有需要预警的订单
                        runTaskResult.Success = true;
                        runTaskResult.Result = "<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要预警的发货超时订单-->";
                        ShowRunningLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要预警的发货超时订单-->");
                        WriteLog("<--【" + tasksCommon.DealFullName.Substring(2, 3) + "】当前没有需要预警的发货超时订单-->");
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
