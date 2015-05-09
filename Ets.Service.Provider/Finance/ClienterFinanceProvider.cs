using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using ETS.Transaction;
using ETS.Transaction.Common;

namespace Ets.Service.Provider.Finance
{
    public class ClienterFinanceProvider : IClienterFinanceProvider
    {
        private  readonly ClienterDao _clienterDao = new ClienterDao();
        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel WithdrawC(WithdrawCPM withdrawCpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                Tuple<bool, FinanceWithdrawC> checkbool = CheckWithdrawC(withdrawCpm);
                if (checkbool.Item1 != true)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return SimpleResultModel.Conclude(checkbool.Item2);
                }
                else
                {
                    
                }
                return SimpleResultModel.Conclude(FinanceWithdrawC.Success); ;
            }
        }

        /// <summary>
        /// 骑士提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        private Tuple<bool,FinanceWithdrawC> CheckWithdrawC(WithdrawCPM withdrawCpm)
        {
            if (withdrawCpm.WithdrawPrice<=0)   //提现金额小于等于0 提现有误
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.WithdrawMoneyError);
            }
            clienter clienter = _clienterDao.GetById(withdrawCpm.ClienterId);//获取超人信息
            if (clienter == null || clienter.Status == null 
                || clienter.Status != ConstValues.CLIENTER_AUDITPASS  //骑士状态为非 审核通过不允许 提现
                || clienter.AllowWithdrawPrice<withdrawCpm.WithdrawPrice //可提现金额小于提现金额，提现失败
                )
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.ClienterError);
            }
            return new Tuple<bool, FinanceWithdrawC>(true, FinanceWithdrawC.Success);
        }
    }
}
