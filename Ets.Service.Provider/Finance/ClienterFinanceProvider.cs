using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using ETS.Transaction;
using ETS.Transaction.Common;

namespace Ets.Service.Provider.Finance
{
    public class ClienterFinanceProvider : IClienterFinanceProvider
    {
        private  readonly ClienterDao _clienterDao = new ClienterDao();
        //骑士余额流水表
        private readonly ClienterBalanceRecordDao _clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        //骑士提现表
        private readonly ClienterWithdrawFormDao _clienterWithdrawFormDao = new ClienterWithdrawFormDao();
        //骑士提现日志
        private readonly ClienterWithdrawLogDao _clienterWithdrawLogDao = new ClienterWithdrawLogDao();

        #region 骑士提现功能  add by caoheyang 20150509

        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel WithdrawC(WithdrawCPM withdrawCpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                clienter clienter=new clienter();
                Tuple<bool, FinanceWithdrawC> checkbool = CheckWithdrawC(withdrawCpm, ref clienter);
                if (checkbool.Item1 != true)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return SimpleResultModel.Conclude(checkbool.Item2);
                }
                else
                {
                    _clienterDao.UpdateForWithdrawC(withdrawCpm); //更新骑士表的余额，可提现余额
                    string withwardNo = "1";
                    //骑士提现
                    long withwardId= _clienterWithdrawFormDao.Insert(new ClienterWithdrawForm()
                    {
                        WithwardNo = withwardNo,//单号 规则待定
                        ClienterId = withdrawCpm.ClienterId,//骑士Id(Clienter表）
                        BalancePrice = clienter.AccountBalance,//提现前骑士余额
                        AllowWithdrawPrice = clienter.AllowWithdrawPrice,//提现前骑士可提现金额
                        Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                        Amount = withdrawCpm.WithdrawPrice,//提现金额
                        Balance = clienter.AccountBalance - withdrawCpm.WithdrawPrice, //提现后余额
                    });
                    //骑士余额流水操作 更新骑士表的余额，可提现余额
                    _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                    {
                        ClienterId = withdrawCpm.ClienterId,//骑士Id(Clienter表）
                        Amount = -withdrawCpm.WithdrawPrice,//流水金额
                        Status = (int)ClienterBalanceRecordStatus.Tradeing, //流水状态(1、交易成功 2、交易中）
                        Balance = clienter.AccountBalance - withdrawCpm.WithdrawPrice, //交易后余额
                        RecordType = (int)ClienterBalanceRecordRecordType.Withdraw,
                        Operator = clienter.TrueName,
                        RelationNo = withwardNo,
                        Remark = "骑士提现"
                    });  
                    //骑士提现记录
                    _clienterWithdrawLogDao.Insert(new ClienterWithdrawLog()
                    {
                        WithwardId = withwardId,
                        Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                        Remark="骑士发起提现操作",
                        Operator = clienter.TrueName
                    }); //更新骑士表的余额，可提现余额
                    tran.Complete();
                }
                return SimpleResultModel.Conclude(FinanceWithdrawC.Success); ;
            }
        }
        /// <summary>
        /// 骑士提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <param name="clienter">超人</param>
        /// <returns></returns>
        private Tuple<bool, FinanceWithdrawC> CheckWithdrawC(WithdrawCPM withdrawCpm,ref clienter clienter)
        {
            if (withdrawCpm.WithdrawPrice <= 0)   //提现金额小于等于0 提现有误
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.WithdrawMoneyError);
            }
            clienter = _clienterDao.GetById(withdrawCpm.ClienterId);//获取超人信息
            if (clienter == null || clienter.Status == null
                || clienter.Status != ConstValues.CLIENTER_AUDITPASS  //骑士状态为非 审核通过不允许 提现
                || clienter.AllowWithdrawPrice < withdrawCpm.WithdrawPrice //可提现金额小于提现金额，提现失败
                )
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.ClienterError);
            }
            return new Tuple<bool, FinanceWithdrawC>(true, FinanceWithdrawC.Success);
        }
 
        #endregion
    }
}
