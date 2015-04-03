using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace UpdatePackConfig
{
    public class UpdatePackConfigTask : AbstractTask
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
            return "自动调配打包台";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "自动检查固定时间段内打包台状态，未工作的关闭其打包台";
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
            ShowRunningLog("=============本次自动调配打包台全部完成=============");
            runTaskResult.Result = "本次自动调配打包台结束";
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
            if (!CustomConfig.ContainsKey("超时时间限制"))
                CustomConfig.Add("超时时间限制", Task.Common.Parameters.MaxOverTimeHour.ToString());
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
                #region 线程分仓自动调配打包台
                var config = new Config
                 {
                    ReadConnectionString=tasksCommon.ConnStrRead,
                    WriteConnectionString=tasksCommon.ConnStrWrite,
                 };
                var wmsShippingConfig = new WmsShippingConfig
                {
                    WareId=tasksCommon.WareId,
                    IsSpare=2,
                    InvoiceType=1,
                };
                #region 按快递自动调配打包台
                var shipingList = mOrderService.GetShippingConfigInfoBLL(tasksCommon.ConnStrRead, wmsShippingConfig);
                if (shipingList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(shipingList[0].ExceptionStr))
                    {
                        #region 程序异常
                        runTaskResult.Success = false;
                        runTaskResult.Result = shipingList[0].ExceptionStr;
                        ShowRunningLog("<--【" + tasksCommon.DealFullName + "】获取快递列表发生异常，详情如下：" +
                                       shipingList[0].ExceptionStr + "-->");
                        WriteLog("<--【" + tasksCommon.DealFullName + "】获取快递列表发生异常，详情如下：" + shipingList[0].ExceptionStr +
                                 "-->");
                        SendEmailTo(
                            TaskName() + tasksCommon.DealFullName + "获取快递列表发生异常，详情如下：" + shipingList[0].ExceptionStr,
                            CustomConfig["EmailAddress"]);
                        #endregion
                    }
                    else
                    {
                        foreach (var shippingConfig in shipingList)
                        {
                            WmsPackConfig wmsPackConfig = new WmsPackConfig
                            {
                                WareId = tasksCommon.WareId,
                                ShippingId = shippingConfig.ShippingId,
                                PackStatue = 1,
                                OvertimeHour =Convert.ToInt32(CustomConfig["超时时间限制"]) ,
                            };
                            var overtimePackerInfo = mOrderService.GetOvertimePackerInfoBLL(tasksCommon.ConnStrWrite, wmsPackConfig);
                            if (overtimePackerInfo.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(overtimePackerInfo[0].ExceptionStr))
                                {
                                    #region 程序异常
                                    runTaskResult.Success = false;
                                    runTaskResult.Result = overtimePackerInfo[0].ExceptionStr;
                                    ShowRunningLog("<--【" + tasksCommon.DealFullName+shippingConfig.ShippingName + "】获取离岗超时工作人信息发生异常，详情如下：" +
                                                   overtimePackerInfo[0].ExceptionStr + "-->");
                                    WriteLog("<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】获取离岗超时工作人信息发生异常，详情如下：" +
                                             overtimePackerInfo[0].ExceptionStr +
                                             "-->");
                                    SendEmailTo(
                                        TaskName() + tasksCommon.DealFullName + shippingConfig.ShippingName + "获取离岗超时工作人信息发生异常，详情如下：" +
                                        overtimePackerInfo[0].ExceptionStr,
                                        CustomConfig["EmailAddress"]);
                                    #endregion
                                }
                                else
                                {
                                    var dealflag=mOrderService.UpdatePackConfigBLL(overtimePackerInfo, config);
                                    if (dealflag.DealFlag)
                                    {
                                        #region 处理成功
                                        runTaskResult.Success = true;
                                        runTaskResult.Result ="【"+ tasksCommon.DealFullName + shippingConfig.ShippingName + "】自动调配打包台已完成";
                                        ShowRunningLog("【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】自动调配打包台已完成");
                                        WriteLog("【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】自动调配打包台已完成");
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 处理失败发邮件通知
                                        runTaskResult.Success = false;
                                        runTaskResult.Result = dealflag.DealMsg;
                                        ShowRunningLog("<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】自动调配打包台失败，详情如下：" + dealflag.DealMsg + "-->");
                                        WriteLog("<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】自动调配打包台失败，详情如下：" + dealflag.DealMsg + "-->");
                                        SendEmailTo(TaskName() + tasksCommon.DealFullName + shippingConfig.ShippingName + "自动调配打包台失败，详情如下：" + dealflag.DealMsg, CustomConfig["EmailAddress"]);
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region 没有需要调配打包台的数据
                                runTaskResult.Success = true;
                                runTaskResult.Result = "<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】当前没有需要调配打包台的数据-->";
                                ShowRunningLog("<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】当前没有需要调配打包台的数据-->");
                                WriteLog("<--【" + tasksCommon.DealFullName + shippingConfig.ShippingName + "】当前没有需要调配打包台的数据-->");
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    #region 没有需要调配打包台的数据
                    runTaskResult.Success = true;
                    runTaskResult.Result = "<--【" + tasksCommon.DealFullName + "】当前没有需要调配打包台的数据-->";
                    ShowRunningLog("<--【" + tasksCommon.DealFullName + "】当前没有需要调配打包台的数据-->");
                    WriteLog("<--【" + tasksCommon.DealFullName + "】当前没有需要调配打包台的数据-->");
                    #endregion
                }
                #endregion
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
