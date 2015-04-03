using Ets.Service.IProvider.Order;
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
        IAutoAdjustProvider iAutoAdjustProvider = new AutoAdjustProvider();
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
            if (string.IsNullOrWhiteSpace(CustomConfig["EmailAddress"]))
            {
                LogCommon("请填写邮件地址！");
                runTaskResult.Result = "请填写邮件地址！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            #endregion
            try
            {
                string globalConfigModel = Ets.Dao.GlobalConfig.GlobalConfigDao.GlobalConfigGet.TimeSubsidies;
                if (string.IsNullOrEmpty(globalConfigModel))
                {
                    LogCommonWithWaring("未获取到公共配置！");
                    runTaskResult.Result = "未获取到公共配置！";
                    runTaskResult.Success = false;
                    return runTaskResult;
                }
                #region 超时分钟数拼接，查库用
                var globalConfigList = globalConfigModel.Split(';');
                string IntervalMinuteList = string.Empty;//超时分钟数串
                foreach (string globalConfigItem in globalConfigList)
                {
                    string minute = globalConfigItem.Split(',')[0];
                    IntervalMinuteList += minute + ",";
                }
                if(IntervalMinuteList.Length>0)
                {
                    IntervalMinuteList = IntervalMinuteList.TrimEnd(',');
                }
                #endregion
                LogCommon("待调整佣金的订单获取中……");
                var orderList =iAutoAdjustProvider.GetOverTimeOrder(IntervalMinuteList);
                if (orderList == null || orderList.Count <= 0)
                {
                    LogCommon("当前没有需要调整订单佣金的订单！");
                    runTaskResult.Result = "当前没有需要调整订单佣金的订单！";
                    runTaskResult.Success = true;
                    return runTaskResult;
                }
                #region 有需要调整佣金的订单
                foreach (string item in globalConfigList)
                {
                    int tempIntervalMinute = Convert.ToInt32(item.Split(',')[0]);
                    decimal adjustAmount=Convert.ToDecimal(item.Split(',')[1]);
                    int dealCount = ETS.Util.ParseHelper.ToInt(item.Split(',')[2], 1);//执行次数
                    var dealOrderList = orderList.Where(t => t.IntervalMinute == tempIntervalMinute && t.DealCount < dealCount).ToList();
                    if (dealOrderList.Count <= 0)
                    {
                        continue;
                    }
                    LogCommon("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整处理中……");
                    var dealResult = iAutoAdjustProvider.AutoAdjustOrderCommission(dealOrderList, adjustAmount);
                    if (dealResult.DealFlag)
                    {
                        #region 全部处理成功
                        LogCommon("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整全部成功,共计【" + dealOrderList.Count + "】单，订单Id集合为：(" + dealResult.SuccessId + ")");
                        #endregion
                    }
                    else
                    {
                        #region 部分处理成功
                        LogCommonWithWaring("超时分钟数为【" + tempIntervalMinute + "】的订单佣金调整部分成功,处理成功：【" + dealResult.DealSuccQty + "】单，订单Id集合为：(" + dealResult.SuccessId + ")，处理失败：【" + (dealOrderList.Count - dealResult.DealSuccQty) + "】单，订单Id集合为：(" + dealResult.FailId + ")");
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常处理
                runTaskResult.Result = ex.ToString();
                runTaskResult.Success = false;
                LogCommonWithWaring(ex.ToString());
                #endregion
            }
            ShowRunningLog("=============本次自动调整订单佣金全部完成=============");
            runTaskResult.Success = true;
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
        /// <summary>
        /// 日志公用方法
        /// </summary>
        /// <param name="strLogContent"></param>
        public void LogCommon(string strLogContent)
        {
            ShowRunningLog(strLogContent);
            WriteLog(strLogContent);
        }
        /// <summary>
        /// 邮件预警和日志
        /// </summary>
        /// <param name="strLogContent"></param>
        public void LogCommonWithWaring(string strLogContent)
        {
            ShowRunningLog(strLogContent);
            WriteLog(strLogContent);
            SendEmailTo(strLogContent, CustomConfig["EmailAddress"]);
        }

    }
}
