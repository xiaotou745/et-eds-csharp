using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Common;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace UnDistributionShelfCodeOrderWarning
{
    public class UnDistributionShelfCodeOrderWarningTask: AbstractTask
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
            return "订单分配拣货位预警";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "对在自动分配拣货位时发现没有找打拣货位的订单发预警预警进行通知";
        }
        /// <summary>
        /// 服务方法
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 验证服务配置
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串(读)"]))
            {
                ShowRunningLog("数据库连接字符串(读)！");
                runTaskResult.Result = "数据库连接字符串(读)！";
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
            string strconRead = CustomConfig["数据库连接字符串(读)"];
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, cc.Key, doneEvents[i]);
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
            ShowRunningLog("=============本次订单获取未分配到拣货位的订单全部完成=============");
            runTaskResult.Result = "本次获取未分配到拣货位的订单结束";
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
            if (!CustomConfig.ContainsKey("数据库连接字符串(读)"))
                CustomConfig.Add("数据库连接字符串(读)", Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
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
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "开");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "关");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "关");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "关");
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
            #endregion
            try
            {
                #region 声明对象
                IList<WmsReceiptTaskDetailDto> unDistributionShelfCodeWarningOrderList = new List<WmsReceiptTaskDetailDto>();
                var erpOrder = new erp_order
                    {
                        ware_id=tasksCommon.WareId
                    };
                StringBuilder sbHtmlBody = new StringBuilder(Task.Common.Parameters.SystemMsgForEWS);
                #endregion
                #region 获取需要分配拣货位的订单
                ShowRunningLog("<--" + tasksCommon.DealFullName + "未分配到拣货位的订单获取中-->");
                unDistributionShelfCodeWarningOrderList = mOrderService.GetUnDistributionShelfCodeWarningOrderByThreadBLL(tasksCommon.ConnStrRead, erpOrder);
                #endregion
                if (unDistributionShelfCodeWarningOrderList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(unDistributionShelfCodeWarningOrderList[0].ExceptionStr))
                    {
                        #region 程序异常
                        runTaskResult.Success = false;
                        runTaskResult.Result = unDistributionShelfCodeWarningOrderList[0].ExceptionStr;
                        ShowRunningLog("<--" + tasksCommon.DealFullName + "获取未分配到拣货位的订单时异常，详情如下：" + unDistributionShelfCodeWarningOrderList[0].ExceptionStr + "-->");
                        WriteLog("<--" + tasksCommon.DealFullName + "获取未分配到拣货位的订单时异常，详情如下：" + unDistributionShelfCodeWarningOrderList[0].ExceptionStr + "-->");
                        SendEmailTo(TaskName() + tasksCommon.DealFullName + "获取未分配到拣货位的订单时异常，详情如下：" + unDistributionShelfCodeWarningOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        #endregion
                    }
                    var orderQty = unDistributionShelfCodeWarningOrderList.Sum(t => t.OrderQty);
                    #region 查到未分配到拣货位的订单,发邮件预警
                    sbHtmlBody.AppendLine("<br />各位好：<br />&nbsp;&nbsp;&nbsp;&nbsp; WMS系统【" + tasksCommon.DealFullName.Substring(2, 3) + "】中共有 <strong>" + unDistributionShelfCodeWarningOrderList.Count + "</strong> 个商品没有匹配货位，影响【" + orderQty + "】个订单，请迅速对商品进行匹配货位并补货，否则不能打印订单，造成订单延误。并关注【未生成拣货路径查询】界面查询详情。<br />");
                    Dictionary<string, string> dicHead = new Dictionary<string, string>();
                    dicHead.Add("GoodsName", "商品名称");
                    dicHead.Add("BarCode", "商品条码");
                    dicHead.Add("GoodsSn", "商品编码");
                    dicHead.Add("OrderQty", "未生成任务订单数");
                    dicHead.Add("GoodsQty", "商品数量");
                    if (unDistributionShelfCodeWarningOrderList.Count > Task.Common.Parameters.MaxCountForEWS)
                    {
                        sbHtmlBody.AppendLine(Task.Common.Parameters.SystemMsgAttachment);
                    }
                    sbHtmlBody.Append(SendEmailCommon.CreateHtmlBody(dicHead, unDistributionShelfCodeWarningOrderList).ToString());
                    string emailTitle = "【" + tasksCommon.DealFullName.Substring(2, 3) + "】未分配到拣货位的订单预警";
                    string emailAddress = CustomConfig[tasksCommon.DealFullName.Substring(2, 3) + "邮件地址"];
                    string copyEmailAddress = CustomConfig["抄送邮件地址"];
                    string attachName = string.Format("【{0}】{1}{2}.xls", tasksCommon.DealFullName.Substring(2, 3), "未分配到拣货位的订单_", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //数据大于500条发附件
                    if (unDistributionShelfCodeWarningOrderList.Count > Task.Common.Parameters.MaxCountForEWS)
                    {
                        SendEmailTo(sbHtmlBody.ToString(), emailAddress, emailTitle, copyEmailAddress, true, attachName, dicHead, unDistributionShelfCodeWarningOrderList);
                    }
                    else
                    {
                        SendEmailTo(sbHtmlBody.ToString(), emailAddress, emailTitle, copyEmailAddress, true);
                    }
                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName.Substring(2, 3) + "】未分配到拣货位的订单获取完成,共计:{" + unDistributionShelfCodeWarningOrderList.Count + "}单";
                    ShowRunningLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】未分配到拣货位的订单获取完成,共计:{" + unDistributionShelfCodeWarningOrderList.Count + "}单");
                    WriteLog("【" + tasksCommon.DealFullName.Substring(2, 3) + "】未分配到拣货位的订单获取完成,共计:{" + unDistributionShelfCodeWarningOrderList.Count + "}单");
                    #endregion
                }
                else
                {
                    #region 没有未分配到拣货位的订单
                    runTaskResult.Success = true;
                    runTaskResult.Result = "<--" + tasksCommon.DealFullName + "当前没有未分配到拣货位的订单-->";
                    ShowRunningLog("<--" + tasksCommon.DealFullName + "当前没有未分配到拣货位的订单-->");
                    WriteLog("<--" + tasksCommon.DealFullName + "当前没有未分配到拣货位的订单-->");
                    #endregion
                }
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
