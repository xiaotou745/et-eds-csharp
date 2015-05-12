using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Security;
using Ets.Service.IProvider.Finance;
using ETS.Transaction;
using ETS.Transaction.Common;

namespace Ets.Service.Provider.Finance
{
    public class ClienterFinanceProvider : IClienterFinanceProvider
    {
        #region 声明对象
        private readonly ClienterDao _clienterDao = new ClienterDao();
        /// <summary>
        /// 骑士余额流水表
        /// </summary>
        private readonly ClienterBalanceRecordDao _clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        /// <summary>
        /// 骑士提现表
        /// </summary>
        private readonly ClienterWithdrawFormDao _clienterWithdrawFormDao = new ClienterWithdrawFormDao();
        /// <summary>
        /// 骑士提现日志
        /// </summary>
        private readonly ClienterWithdrawLogDao _clienterWithdrawLogDao = new ClienterWithdrawLogDao();
        /// <summary>
        /// 骑士金融账号表
        /// </summary>
        private readonly ClienterFinanceAccountDao _clienterFinanceAccountDao = new ClienterFinanceAccountDao();
        #endregion

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
                clienter clienter = new clienter();
                var clienterFinanceAccount = new ClienterFinanceAccount();//骑士金融账号信息
                Tuple<bool, FinanceWithdrawC> checkbool = CheckWithdrawC(withdrawCpm, ref clienter, ref clienterFinanceAccount);
                if (checkbool.Item1 != true)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return SimpleResultModel.Conclude(checkbool.Item2);
                }
                else
                {
                    _clienterDao.UpdateForWithdrawC(withdrawCpm); //更新骑士表的余额，可提现余额
                    string withwardNo = "1";
                    #region 骑士提现
                    long withwardId = _clienterWithdrawFormDao.Insert(new ClienterWithdrawForm()
                              {
                                  WithwardNo = withwardNo,//单号 规则待定
                                  ClienterId = withdrawCpm.ClienterId,//骑士Id(Clienter表）
                                  BalancePrice = clienter.AccountBalance,//提现前骑士余额
                                  AllowWithdrawPrice = clienter.AllowWithdrawPrice,//提现前骑士可提现金额
                                  Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                                  Amount = withdrawCpm.WithdrawPrice,//提现金额
                                  Balance = clienter.AccountBalance - withdrawCpm.WithdrawPrice, //提现后余额
                                  TrueName = clienterFinanceAccount.TrueName,//骑士收款户名
                                  AccountNo = clienterFinanceAccount.AccountNo, //卡号(DES加密)
                                  AccountType = clienterFinanceAccount.AccountType, //账号类型：
                                  OpenBank = clienterFinanceAccount.OpenBank,//开户行
                                  OpenSubBank = clienterFinanceAccount.OpenSubBank //开户支行
                              });
                    #endregion

                    #region 骑士余额流水操作 更新骑士表的余额，可提现余额
                    _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                            {
                                ClienterId = withdrawCpm.ClienterId,//骑士Id(Clienter表）
                                Amount = -withdrawCpm.WithdrawPrice,//流水金额
                                Status = (int)ClienterBalanceRecordStatus.Tradeing, //流水状态(1、交易成功 2、交易中）
                                Balance = clienter.AccountBalance - withdrawCpm.WithdrawPrice, //交易后余额
                                RecordType = (int)ClienterBalanceRecordRecordType.Withdraw,
                                Operator = clienter.TrueName,
                                WithwardId = withwardId,
                                RelationNo = withwardNo,
                                Remark = "骑士提现"
                            });
                    #endregion

                    #region 骑士提现记录

                    _clienterWithdrawLogDao.Insert(new ClienterWithdrawLog()
                    {
                        WithwardId = withwardId,
                        Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                        Remark = "骑士发起提现操作",
                        Operator = clienter.TrueName
                    }); //更新骑士表的余额，可提现余额 
                    #endregion
                    tran.Complete();
                }
                return SimpleResultModel.Conclude(FinanceWithdrawC.Success); ;
            }
        }

        /// <summary>
        /// 骑士提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <param name="clienter">骑士</param>
        /// <param name="clienterFinanceAccount">骑士金融账号信息</param>
        /// <returns></returns>
        private Tuple<bool, FinanceWithdrawC> CheckWithdrawC(WithdrawCPM withdrawCpm, ref clienter clienter,
            ref  ClienterFinanceAccount clienterFinanceAccount)
        {
            if (withdrawCpm.WithdrawPrice <= 0)   //提现金额小于等于0 提现有误
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.WithdrawMoneyError);
            }
            clienter = _clienterDao.GetById(withdrawCpm.ClienterId);//获取超人信息
            if (clienter == null || clienter.Status == null
                || clienter.Status != ConstValues.CLIENTER_AUDITPASS)  //骑士状态为非 审核通过不允许 提现
             
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.ClienterError);
            }
            else if (clienter.AllowWithdrawPrice < withdrawCpm.WithdrawPrice) //可提现金额小于提现金额，提现失败
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.MoneyError);
            }
            var clienterFinanceAccounts = _clienterFinanceAccountDao.GetByClienterId(withdrawCpm.ClienterId);//获取超人金融账号信息
            if (clienterFinanceAccounts.Count <= 0)
            {
                return new Tuple<bool, FinanceWithdrawC>(false, FinanceWithdrawC.FinanceAccountError);
            }
            else
            {
                clienterFinanceAccount = clienterFinanceAccounts[0];
            }

            return new Tuple<bool, FinanceWithdrawC>(true, FinanceWithdrawC.Success);
        }

        #endregion


        #region 骑士金融账号绑定/修改
        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511 
        /// TODO 统一加密算法 目前只支付网银
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel CardBindC(CardBindCPM cardBindCpm)
        {
            if (cardBindCpm.AccountNo != cardBindCpm.AccountNo2) //两次录入的金融账号不一致
            {
                return SimpleResultModel.Conclude(FinanceCardBindC.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int count = _clienterFinanceAccountDao.GetCountByClienterId(cardBindCpm.ClienterId);
                if (count > 0)
                {
                    return SimpleResultModel.Conclude(FinanceCardBindC.Exists);//该骑士已绑定过金融账号
                }
                int result = _clienterFinanceAccountDao.Insert(new ClienterFinanceAccount()
                {
                    ClienterId = cardBindCpm.ClienterId,//骑士ID
                    TrueName = cardBindCpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindCpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true,// 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindCpm.AccountType == 0
                        ? (int)ClienterFinanceAccountType.WangYin : cardBindCpm.AccountType,  //账号类型 
                    OpenBank = cardBindCpm.OpenBank, //开户行
                    OpenSubBank = cardBindCpm.OpenSubBank, //开户支行
                    CreateBy = cardBindCpm.CreateBy,//创建人  当前登录人
                    UpdateBy = cardBindCpm.CreateBy//新增时最后修改人与新增人一致  当前登录人
                });
                tran.Complete();
                return SimpleResultModel.Conclude(SystemEnum.Success);
            }
        }


        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511  TODO 统一加密算法
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel CardModifyC(CardModifyCPM cardModifyCpm)
        {
            if (cardModifyCpm.AccountNo != cardModifyCpm.AccountNo2) //两次录入的金融账号不一致
            {
                return SimpleResultModel.Conclude(FinanceCardCardModifyC.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                _clienterFinanceAccountDao.Update(new ClienterFinanceAccount()
                {
                    Id = cardModifyCpm.Id,
                    ClienterId = cardModifyCpm.ClienterId,//骑士ID
                    TrueName = cardModifyCpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardModifyCpm.AccountNo), //卡号(DES加密) 
                    OpenBank = cardModifyCpm.OpenBank, //开户行
                    OpenSubBank = cardModifyCpm.OpenSubBank, //开户支行
                    UpdateBy = cardModifyCpm.UpdateBy//修改人  当前登录人
                });
                tran.Complete();
                return SimpleResultModel.Conclude(SystemEnum.Success);
            }
        }
        #endregion

        /// <summary>
        /// 骑士交易流水API add by caoheyang 20150511
        /// </summary> 
        /// <param name="clienterId">骑士id</param>
        /// <returns></returns>
        public ResultModel<IList<FinanceRecordsDM>> GetRecords(int clienterId)
         {
             IList<FinanceRecordsDM> records = _clienterBalanceRecordDao.GetByClienterId(clienterId);
             return ResultModel<IList<FinanceRecordsDM>>.Conclude(SystemEnum.Success,
               TranslateRecords(records));
         }

        /// <summary>
        /// 骑士交易流水API 信息处理转换 add by caoheyang 20150512
        /// </summary>
        /// <param name="records">原始流水记录</param>
        /// <returns></returns>
        private IList<FinanceRecordsDM> TranslateRecords(IList<FinanceRecordsDM> records)
        {
            foreach (var temp in records)
            {
                temp.StatusStr = ((ClienterBalanceRecordStatus) Enum.Parse(typeof (ClienterBalanceRecordStatus),
                    temp.Status.ToString(), false)).GetDisplayText(); //流水状态文本
                temp.RecordTypeStr =
                    ((ClienterBalanceRecordRecordType) Enum.Parse(typeof (ClienterBalanceRecordRecordType),
                        temp.RecordType.ToString(), false)).GetDisplayText(); //交易类型文本
            }
            return records;
        }
    }
}
