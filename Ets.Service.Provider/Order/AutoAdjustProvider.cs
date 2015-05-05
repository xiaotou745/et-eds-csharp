using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Order;
using ETS.Transaction;
using ETS.Transaction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
    public class AutoAdjustProvider : IAutoAdjustProvider
    {
        OrderDao orderDao = new OrderDao();

        /// <summary>
        /// 执行补贴任务
        /// 窦海超
        /// 2015年4月3日 15:19:46
        /// </summary>
        public void AdjustOrderService()
        {
            //ETS.Util.Log.WriteTextToFile("执行补贴任务", ETS.Config.ConfigKey("LogPath"));
            string globalConfigModel = Ets.Dao.GlobalConfig.GlobalConfigDao.GlobalConfigGet(0).TimeSubsidies;
            if (string.IsNullOrEmpty(globalConfigModel))
            {
                return;
            }
            #region 超时分钟数拼接，查库用
            //ETS.Util.Log.WriteTextToFile("超时分钟数拼接", ETS.Config.ConfigKey("LogPath"));
            var globalConfigList = globalConfigModel.Split(';');
            string IntervalMinuteList = string.Empty;//超时分钟数串
            foreach (string globalConfigItem in globalConfigList)
            {
                string minute = globalConfigItem.Split(',')[0];
                IntervalMinuteList += minute + ",";
            }
            if (IntervalMinuteList.Length > 0)
            {
                IntervalMinuteList = IntervalMinuteList.TrimEnd(',');
            }
            #endregion
            var orderList = GetOverTimeOrder(IntervalMinuteList);
            if (orderList == null || orderList.Count <= 0)
            {
                return;
            }
            var arrIntervalMinuteList = IntervalMinuteList.Split(',');
            //ETS.Util.Log.WriteTextToFile("有需要调整佣金的订单", ETS.Config.ConfigKey("LogPath"));
            #region 有需要调整佣金的订单
            foreach (string item in globalConfigList)
            {
                int tempIntervalMinute = Convert.ToInt32(item.Split(',')[0]);
                decimal adjustAmount = Convert.ToDecimal(item.Split(',')[1]);
                int dealCount = ETS.Util.ParseHelper.ToInt(item.Split(',')[2], 1);//执行次数
                var dealOrderList = orderList.Where(t => t.IntervalMinute == tempIntervalMinute && t.DealCount < dealCount).ToList();
                if (dealOrderList.Count <= 0)
                {
                    continue;
                }
                //ETS.Util.Log.WriteTextToFile("AutoAdjustOrderCommission", ETS.Config.ConfigKey("LogPath"),true);
                AutoAdjustOrderCommission(dealOrderList, adjustAmount, ETS.Util.ParseHelper.ToInt(arrIntervalMinuteList[dealCount - 1]));
            }
            #endregion
        }

        /// <summary>
        /// 调整订单佣金
        /// 窦海超
        /// 2015年4月3日 09:41:19
        /// </summary>
        public void AutoAdjustOrderCommission(IList<OrderAutoAdjustModel> list, decimal AdjustAmount, int executeMinute)
        {
            try
            {
                foreach (OrderAutoAdjustModel item in list)
                {
                    //ETS.Util.Log.WriteTextToFile("调整订单佣金AutoAdjustOrderCommission", ETS.Config.ConfigKey("LogPath"),true);
                    using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                    {
                        orderDao.UpdateOrderCommissionById(AdjustAmount, item.Id);//更新账户
                        orderDao.InsertOrderSubsidiesLog(AdjustAmount, item.Id, executeMinute);//写入日志 
                        tran.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                //ETS.Util.LogHelper.LogWriter(ex);
                ETS.Util.LogHelper.LogWriter( ex,"当前时间:" + DateTime.Now.ToString() + "调整订单佣金引发异常");
            }
        }

        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        public IList<OrderAutoAdjustModel> GetOverTimeOrder(string IntervalMinute)
        {
            return orderDao.GetOverTimeOrder(IntervalMinute);
        }

    }
}
