using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace DealChangeWmsOrder
{
    public class DealChangeWmsOrderTask : AbstractTask
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
            return "自动处理变更订单";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "多线程分仓库自动处理变更订单（缺货调仓、取消）";
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
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串（写）"]))
            {
                ShowRunningLog("数据库连接字符串（写）！");
                runTaskResult.Result = "数据库连接字符串（写）！";
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
            string strconWrite = CustomConfig["数据库连接字符串（写）"];
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, cc.Key, doneEvents[i]);
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
            ShowRunningLog("=============本次处理变更订单全部完成=============");
            runTaskResult.Result = "本次处理变更订单全部完成";
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
                CustomConfig.Add("数据库连接字符串(读)", Task.Common.Parameters.TestDBConnectionString0213);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionString0213);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
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
                #region 处理变更操作订单
                WmsOrderChange wmsOrderChange = new WmsOrderChange()
                    {
                        WareId=tasksCommon.WareId,
                        DealType=0,
                    };
                var wmsOrderChangeInfo = mOrderService.GetWmsOrderChangeInfoBLL(tasksCommon.ConnStrRead, wmsOrderChange);
                if (wmsOrderChangeInfo==null ||wmsOrderChangeInfo.Count == 0)
                {
                    #region 没有做变更操作的订单
                    runTaskResult.Success = true;
                    runTaskResult.Result = "<--" + tasksCommon.DealFullName + "中当前没有做变更操作的订单-->";
                    ShowRunningLog("<--" + tasksCommon.DealFullName + "中当前没有做变更操作的订单-->");
                    WriteLog("<--" + tasksCommon.DealFullName + "中当前没有做变更操作的订单-->");
                    #endregion
                }
                else
                {
                    if (!string.IsNullOrEmpty(wmsOrderChangeInfo[0].ExceptionStr))
                    {
                        #region 获取变更操作的订单失败,发邮件通知处理
                        runTaskResult.Success = false;
                        runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName + "获取变更操作的订单时失败：" + wmsOrderChangeInfo[0].ExceptionStr + "-->";
                        ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName + "获取变更操作的订单时失败：" + wmsOrderChangeInfo[0].ExceptionStr + "-->");
                        WriteLog(TaskName() + "<--" + tasksCommon.DealFullName + "获取变更操作的订单时失败：" + wmsOrderChangeInfo[0].ExceptionStr + "-->");
                        SendEmailTo(TaskName() + "<--" + tasksCommon.DealFullName + "获取变更操作的订单时失败：" + wmsOrderChangeInfo[0].ExceptionStr + "-->", CustomConfig["EmailAddress"]);
                        #endregion
                    }
                    else
                    {
                        #region 处理变更操作的订单
                        var dealResultInfo = mOrderService.DealChangeWmsOrderBLL(tasksCommon.ConnStrWrite, wmsOrderChangeInfo);
                        if (dealResultInfo.DealFlag)
                        {
                            runTaskResult.Success = true;
                            runTaskResult.Result = tasksCommon.DealFullName + "处理变更操作订单成功！";
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "处理变更操作订单成功！");
                            WriteLog("<--" + tasksCommon.DealFullName + "处理变更操作订单成功！");
                        }
                        else
                        {
                            runTaskResult.Success = true;
                            runTaskResult.Result = tasksCommon.DealFullName + "处理变更操作订单失败订单描述："+dealResultInfo.DealMsg;
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "处理变更操作订单失败订单描述：" + dealResultInfo.DealMsg);
                            WriteLog("<--" + tasksCommon.DealFullName + "处理变更操作订单失败订单描述：" + dealResultInfo.DealMsg);
                            SendEmailTo(TaskName() + tasksCommon.DealFullName + "处理变更操作订单失败订单描述：" + dealResultInfo.DealMsg, CustomConfig["EmailAddress"]);

                        }
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
