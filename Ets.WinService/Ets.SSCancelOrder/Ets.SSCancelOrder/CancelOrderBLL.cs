using Common.Logging;
using Ets.Dao.Business;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using Ets.Dao.Order;
using Ets.Dao.User;
using ETS.Enums;
using Ets.Model.DomainModel.Order;
using Ets.Service.Provider.Clienter;
using ETS;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;
using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
namespace Ets.SSCancelOrder
{
    public class CancelOrderBLL : Quartz.IJob
    {

        //使用Common.Logging.dll日志接口实现日志记录        
        private ILog logger = LogManager.GetCurrentClassLogger();
        readonly IOrderProvider iOrderProvider = new OrderProvider();
        private static bool threadSafe = true;//线程安全
        #region IJob 成员

        public void Execute(Quartz.IJobExecutionContext context)
        {
            if (!threadSafe)
            {
                return;
            }
            threadSafe = false;
            try
            {
                LogHelper.LogWriter("执行啦:" + DateTime.Now);
                double hour = 24;
                GlobalConfigModel gcModel= GlobalConfigDao.GlobalConfigGet(0);
                if (gcModel != null && gcModel.SSCancelOrder != null)
                {
                    hour = Convert.ToInt32(gcModel.SSCancelOrder);
                }

                IList<NonJoinWithdrawModel> list = iOrderProvider.GetSSCancelOrder(hour);//获取没给可提现金额加钱的订单
                foreach (var item in list)
                {                    
                        try
                        {                             

                            SSOrderCancelPM pm = new SSOrderCancelPM();
                            pm.OrderId = item.id;
                            pm.OptUserName = "取消订单返还配送费";
                            pm.OptLog = "取消订单返还配送费";
                            pm.OrderId = item.id;
                            pm.Remark = "取消订单返还配送费";
                            pm.Platform = SuperPlatform.ServicePlatform.GetHashCode();
                            iOrderProvider.SSCancelOrder(pm);

                         
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogWriter(ex);
                        }               
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }
            finally
            {
                threadSafe = true;
            }

        }

        #endregion
    }
}
