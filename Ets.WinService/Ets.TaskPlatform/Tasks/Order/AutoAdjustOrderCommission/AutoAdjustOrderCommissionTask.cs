using Ets.Service.Provider.Order;
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
                string globalConfigModel = Ets.Dao.GlobalConfig.GlobalConfigDao.GlobalConfigGet.TimeSubsidies;
                if (string.IsNullOrEmpty(globalConfigModel))
                {
                    return null;
                }
                var globalConfigList = globalConfigModel.Split(';');
                #region 分钟累加，查库用
                string IntervalMinuteList = string.Empty;
                foreach (string globalConfigItem in globalConfigList)
                {
                    if (!globalConfigItem.Contains(','))
                    {
                        continue;
                    }

                    string minute = globalConfigItem.Split(',')[0];

                    IntervalMinuteList += minute + ",";
                }
                //如果配置库里不存在分钟，则返回
                if (string.IsNullOrEmpty(IntervalMinuteList))
                {
                    return null;
                }
                IntervalMinuteList = IntervalMinuteList.TrimEnd(',');
                config.IntervalMinuteList = IntervalMinuteList;
                #endregion
                #region 根据超时时间配置调整订单佣金

                ShowRunningLog("待调整佣金的订单获取中……");
                var orderList = new Ets.Dao.Order.OrderDao().GetOverTimeOrder(IntervalMinuteList);
                if (orderList == null || orderList.Count <= 0)
                {
                    return null;
                }
                #endregion
                #region 有需要调整佣金的订单
                ShowRunningLog("待调整佣金的订单获取完成，共计【" + orderList.Count + "】单");
                WriteLog("待调整佣金的订单获取完成，共计【" + orderList.Count + "】单");

                foreach (string item in globalConfigList)
                {
                    int tempIntervalMinute = Convert.ToInt32(item.Split(',')[0]);
                    config.IntervalMinute = tempIntervalMinute;
                    config.AdjustAmount = Convert.ToDecimal(item.Split(',')[1]);
                    int doCount = ETS.Util.ParseHelper.ToInt(item.Split(',')[2], 1);//执行次数
                    var order = orderList.Where(t => t.IntervalMinute == tempIntervalMinute && t.DealCount < doCount).ToList();
                    ///当前分钟对应订单不存在 
                    if (order.Count <= 0)
                    {
                        continue;
                    }
                    ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整处理中……");
                    AutoAdjustProvider autoAdjustProvider = new AutoAdjustProvider();
                    autoAdjustProvider.AutoAdjustOrderCommission(order, config.AdjustAmount);
                    // var dealResult = iOrderService.AdjustOrderCommission(config, order);
                }
                #endregion
                //                if (dealResult.DealFlag)
                //                {
                //                    #region 全部处理成功
                //                    ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整全部成功,共计【" + order.Count + "】单，订单Id集合为：(" + dealResult.SuccessId + ")");
                //                    WriteLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整全部成功,共计【" + order.Count + "】单，订单Id集合为：(" + dealResult.SuccessId + ")");
                //                    #endregion
                //                }
                //                else
                //                {
                //                    #region 部分处理成功
                //                    ShowRunningLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")");
                //                    WriteLog("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")");
                //                    SendEmailTo("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (order.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")", CustomConfig["EmailAddress"]);
                //                    #endregion
                //                }
                //            }
                //        }
                //        runTaskResult.Success = true;
                //        runTaskResult.Result = "本次订单调整佣金成功";
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 没有需要调整佣金的订单
                //        runTaskResult.Success = true;
                //        runTaskResult.Result = "当前没有需要调整订单佣金的订单";
                //        ShowRunningLog("当前没有需要调整订单佣金的订单");
                //        WriteLog("当前没有需要调整订单佣金的订单");
                //        #endregion
                //    }
                //    #endregion
                //}
                //else
                //{
                //    #region 未正常获取到超时时间配置信息
                //    if (globalConfigList == null || globalConfigList.Count == 0)
                //    {
                //        #region 没有获取到超时时间配置信息
                //        runTaskResult.Success = true;
                //        runTaskResult.Result = "没有获取到超时时间配置信息";
                //        ShowRunningLog("没有获取到超时时间配置信息");
                //        WriteLog("没有获取到超时时间配置信息");
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 获取到超时时间配置信息异常
                //        runTaskResult.Success = false;
                //        runTaskResult.Result = globalConfigList[0].ExceptionStr;
                //        ShowRunningLog("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr);
                //        WriteLog("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr);
                //        SendEmailTo("获取超时时间配置信息遇到异常，异常信息：" + globalConfigList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                //        #endregion
                //    }

                //}
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
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wang.xudan@etaostars.com;");
            return CustomConfig;
        }
    }
}
