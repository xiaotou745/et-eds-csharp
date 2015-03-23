using Ets.Dao.Clienter;
using Ets.Dao.WtihdrawRecords;
using Ets.Service.IProvider.WtihdrawRecords;
using ETS.Transaction;
using ETS.Transaction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.WtihdrawRecords
{
    public class WtihdrawRecordsProvider : IWtihdrawRecordsProvider
    {
        /// <summary>
        /// 提现
        /// 窦海超
        /// 2015年3月23日 12:54:43
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddWtihdrawRecords(Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel model)
        {

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                WtihdrawRecordsDao withDao = new WtihdrawRecordsDao();
                ClienterDao clienterDao = new ClienterDao();
                bool checkBalance = clienterDao.UpdateClienterAccountBalance(model);
                if (!checkBalance)//如果余额不正确，返回错误
                {
                    return false;
                }
                bool checkAddwith = withDao.AddWtihdrawRecords(model);//新增提现记录

                bool checkAddrecords = withDao.AddRecords(model);//新增提现流水记录
                if (!checkAddwith || !checkAddrecords)
                {
                    return false;
                }
                tran.Complete();
            }
            return true;
        }
    }
}
