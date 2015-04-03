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

namespace AutoAdjustOrderCommission
{
    public class AutoAdjustOrderCommissionTask : AbstractTask
    {
        IOrderService iOrderService = new OrderService();
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "自动调整订单佣金";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "按照时间配置开始进行自动调整订单佣金";
        }
        /// <summary>
        /// 服务方法
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            #region 对象声明和验证
            var runTaskResult = new RunTaskResult();
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串（读）"]))
            {
                ShowRunningLog("数据库连接字符串（读）！");
                runTaskResult.Result = "数据库连接字符串（读）！";
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
            string strConRead = CustomConfig["数据库连接字符串（读）"];
            string strConWrite = CustomConfig["数据库连接字符串（写）"];
            var config = new Config
            {
                ReadConnectionString = strConRead,
                WriteConnectionString = strConWrite
            };
            #endregion
            try
            {
                //var globalConfigModel = new GlobalConfigModel() { KeyName = "TimeSubsidies" };
                string globalConfigModel = GlobalConfigDao.GlobalConfigGet.CommissionRatio;
                var globalConfigList = iOrderService.GetGlobalConfigInfo(config, globalConfigModel);
                if (globalConfigList != null && globalConfigList.Count > 0 && globalConfigList[0].DealFlag)
                {
                    #region 根据超时时间配置调整订单佣金
                    string IntervalMinuteList = "";
                    var value = globalConfigList[0].Value.Split(';');
                    for (int i = 0; i < value.Length; i++)
                    {
                        IntervalMinuteList += value[i].Split(',')[0].ToString() + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(IntervalMinuteList))
                    {
                        IntervalMinuteList = IntervalMinuteList.Remove(IntervalMinuteList.Length - 1, 1);
                    }
                    config.IntervalMinuteList = IntervalMinuteList;
                    ShowRunningLog("待调整佣金的订单获取中……");
                    var orderList = iOrderService.GetOverTimeOrder(config);
                    if (orderList != null && orderList.Count > 0)
                    {
                        #region 有需要调整佣金的订单
                        ShowRunningLog("待调整佣金的订单获取完成，共计【" + orderList.Count + "】单");
                        WriteLog("待调整佣金的订单获取完成，共计【" + orderList.Count + "】单");
                        for (int i = 0; i < value.Length; i++)
                        {
                            int tempIntervalMinute = Convert.ToInt32(value[i].Split(',')[0]);
                            config.IntervalMinute = tempIntervalMinute;
                            config.AdjustAmount = Convert.ToDecimal(value[i].Split(',')[1]);
                            var order = orderList.Where(t => t.IntervalMinute == tempIntervalMinute).ToList();
                            if (order.Count > 0)
                            {
                                ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整处理中……");
                                var dealResult = iOrderService.AdjustOrderCommission(config, order);
                                if (dealResult.DealFlag)
                                {
                                    #region 全部处理成功
                                    ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整全部成功,共计【" + order.Count + "】单，订单Id集合为：(" + dealResult.SuccessId + ")");
                                    WriteLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整全部成功,共计【" + order.Count + "】单，订单Id集合为：(" + dealResult.SuccessId + ")");
                                    #endregion
                                }
                                else
                                {
                                    #region 部分处理成功
                                    ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")");
                                    WriteLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")");
                                    SendEmailTo("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")", CustomConfig["EmailAddress"]);
                                    #endregion
                                }
                            }
                        }
                        runTaskResult.Success = true;
                        runTaskResult.Result = "本次订单调整佣金成功";
                        #endregion
                    }
                    else
                    {
                        #region 没有需要调整佣金的订单
                        runTaskResult.Success = true;
                        runTaskResult.Result = "当前没有需要调整订单佣金的订单";
                        ShowRunningLog("当前没有需要调整订单佣金的订单");
                        WriteLog("当前没有需要调整订单佣金的订单");
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region 未正常获取到超时时间配置信息
                    if (globalConfigList == null || globalConfigList.Count == 0)
                    {
                        #region 没有获取到超时时间配置信息
                        runTaskResult.Success = true;
                        runTaskResult.Result = "没有获取到超时时间配置信息";
                        ShowRunningLog("没有获取到超时时间配置信息");
                        WriteLog("没有获取到超时时间配置信息");
                        #endregion
                    }
                    else
                    {
                        #region 获取到超时时间配置信息异常
                        runTaskResult.Success = false;
                        runTaskResult.Result = globalConfigList[0].ExceptionStr;
                        ShowRunningLog("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr);
                        WriteLog("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr);
                        SendEmailTo("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region 异常处理
                runTaskResult.Result = ex.ToString();
                runTaskResult.Success = false;
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
                #endregion
            }
            ShowRunningLog("=============本次自动调整订单佣金全部完成=============");
            runTaskResult.Result = "本次自动调整订单佣金结束";
            return runTaskResult;
        }
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> UploadConfig()
        {
#if DEBUG
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）", Task.Common.Parameters.TestDBConnectionStringRead);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionStringWrite);
#else
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）", Task.Common.Parameters.OnLineDBConnectionStringRead);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.OnLineDBConnectionStringWrite);
#endif
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wang.xudan@etaostars.com;");
            return CustomConfig;
        }
    }
}
