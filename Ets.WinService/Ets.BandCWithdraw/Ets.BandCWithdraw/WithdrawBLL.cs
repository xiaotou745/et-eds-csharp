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
namespace Ets.BandCWithdraw
{
    public class WithdrawBLL : Quartz.IJob
    {

        //使用Common.Logging.dll日志接口实现日志记录        
        private ILog logger = LogManager.GetCurrentClassLogger();
        readonly ClienterAllowWithdrawRecordDao clienterAllowWithdrawRecordDao = new ClienterAllowWithdrawRecordDao();
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
                double hour = ParseHelper.ToDouble(Config.ConfigKey("DataTime"), -1);
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
                            LogHelper.LogWriter(string.Concat("执行订单ID：", item.id, "，ClienterId：", item.clienterId));
                            clienterDao.UpdateAllowWithdrawPrice(item.clienterPrice, item.clienterId);
                            //businessDao.UpdateAllowWithdrawPrice(item.businessPrice, item.businessId);
                            orderDao.UpdateJoinWithdraw(item.id);


                            ClienterAllowWithdrawRecord cawrm = new ClienterAllowWithdrawRecord()
                            {
                                ClienterId = item.clienterId,
                                Amount = item.clienterPrice,
                                Status = ClienterAllowWithdrawRecordStatus.Success.GetHashCode(),
                                RecordType = ClienterAllowWithdrawRecordType.OrderCommission.GetHashCode(),
                                Operator = item.clienterId.ToString(),
                                WithwardId = item.id,
                                RelationNo = "",
                                Remark = "72小时加可提现"
                            };
                            clienterAllowWithdrawRecordDao.Insert(cawrm);

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
            finally
            {
                threadSafe = true;
            }

        }

        #endregion
    }
}
