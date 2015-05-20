﻿using Common.Logging;
using Ets.Dao.Clienter;
using Ets.Dao.Order;
using Ets.Dao.User;
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

namespace Ets.BandCWithdraw
{
    public class WithdrawBLL : Quartz.IJob
    {

        //使用Common.Logging.dll日志接口实现日志记录        
        private ILog logger = LogManager.GetCurrentClassLogger();
        #region IJob 成员

        public void Execute(Quartz.IJobExecutionContext context)
        {
            try
            {
                LogHelper.LogWriter("执行啦:" + DateTime.Now);
                int hour = ParseHelper.ToInt(Config.ConfigKey("DataTime"), -1);
                if (hour == -1)
                {
                    return;
                }
                ClienterDao clienterDao = new ClienterDao();
                BusinessDao businessDao = new BusinessDao();
                OrderDao orderDao = new OrderDao();
                IList<NonJoinWithdrawModel> list = orderDao.GetNonJoinWithdraw(hour);//获取没给可提现金额加钱的订单

                foreach (var item in list)
                {
                    using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                    {
                        try
                        {
                            clienterDao.UpdateAllowWithdrawPrice(item.clienterPrice, item.clienterId);
                            businessDao.UpdateAllowWithdrawPrice(item.businessPrice, item.businessId);
                            orderDao.UpdateJoinWithdraw(item.id);
                            tran.Complete();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogWriter(ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex);
            }

        }

        #endregion
    }
}