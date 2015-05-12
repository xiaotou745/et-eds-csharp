using Ets.Dao.Finance;
using Ets.Dao.User;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Security;
using Ets.Service.IProvider.Finance;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Transaction;
using ETS.Transaction.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Dao.User;
namespace Ets.Service.Provider.Finance
{
    public class BusinessFinanceProvider : IBusinessFinanceProvider
    {
        #region 声明对象

        private readonly BusinessDao _businessDao = new BusinessDao();
        /// <summary>
        /// 商户余额流水表
        /// </summary>
        private readonly BusinessBalanceRecordDao _businessBalanceRecordDao = new BusinessBalanceRecordDao();
        /// <summary>
        /// 商户提现表
        /// </summary>
        private readonly BusinessWithdrawFormDao _businessWithdrawFormDao = new BusinessWithdrawFormDao();
        /// <summary>
        /// 商户提现日志
        /// </summary>
        private readonly BusinessWithdrawLogDao _businessWithdrawLogDao = new BusinessWithdrawLogDao();
        /// <summary>
        /// 商户金融账号表
        /// </summary>
        private readonly BusinessFinanceAccountDao _businessFinanceAccountDao = new BusinessFinanceAccountDao();

        /// <summary>
        /// 财务dao
        /// </summary>
        private BusinessFinanceDao businessFinanceDao = new BusinessFinanceDao();
        #endregion

        /// <summary>
        /// 根据参数获取商家提现申请单列表
        /// danny-20150509
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessWithdrawFormModel> GetBusinessWithdrawList(BusinessWithdrawSearchCriteria criteria)
        {
            return businessFinanceDao.GetBusinessWithdrawList<BusinessWithdrawFormModel>(criteria);
        }

        #region  商户提现功能  add by caoheyang 20150511

        /// <summary>
        /// 商户提现功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel WithdrawB(WithdrawBPM withdrawBpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                Business business = new Business();
                var businessFinanceAccount = new BusinessFinanceAccount();//商户金融账号信息
                Tuple<bool, FinanceWithdrawB> checkbool = CheckWithdrawB(withdrawBpm, ref business, ref businessFinanceAccount);
                if (checkbool.Item1 != true)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return SimpleResultModel.Conclude(checkbool.Item2);
                }
                else
                {
                    _businessDao.UpdateForWithdrawC(withdrawBpm); //更新商户表的余额，可提现余额
                    string withwardNo = "1";
                    #region 商户提现
                    long withwardId = _businessWithdrawFormDao.Insert(new BusinessWithdrawForm()
                    {
                        WithwardNo = withwardNo,//单号 规则待定
                        BusinessId = withdrawBpm.BusinessId,//商户Id
                        BalancePrice = business.BalancePrice,//提现前商户余额
                        AllowWithdrawPrice = business.AllowWithdrawPrice,//提现前商户可提现金额
                        Status = (int)BusinessWithdrawFormStatus.WaitAllow,//待审核
                        Amount = withdrawBpm.WithdrawPrice,//提现金额
                        Balance = business.BalancePrice - withdrawBpm.WithdrawPrice, //提现后余额
                        TrueName = businessFinanceAccount.TrueName,//商户收款户名
                        AccountNo = businessFinanceAccount.AccountNo, //卡号(DES加密)
                        AccountType = businessFinanceAccount.AccountType, //账号类型：
                        OpenBank = businessFinanceAccount.OpenBank,//开户行
                        OpenSubBank = businessFinanceAccount.OpenSubBank //开户支行
                    });
                    #endregion

                    #region 商户余额流水操作 更新骑士表的余额，可提现余额
                    _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                    {
                        BusinessId = withdrawBpm.BusinessId,//商户Id
                        Amount = -withdrawBpm.WithdrawPrice,//流水金额
                        Status = (int)BusinessBalanceRecordStatus.Tradeing, //流水状态(1、交易成功 2、交易中）
                        Balance = business.BalancePrice - withdrawBpm.WithdrawPrice, //交易后余额
                        RecordType = (int)BusinessBalanceRecordRecordType.Withdraw,
                        Operator = business.Name,
                        WithwardId = withwardId,
                        RelationNo = withwardNo,
                        Remark = "商户提现"
                    });
                    #endregion

                    #region 商户提现记录

                    _businessWithdrawLogDao.Insert(new BusinessWithdrawLog()
                    {
                        WithwardId = withwardId,
                        Status = (int)BusinessWithdrawFormStatus.WaitAllow,//待审核
                        Remark = "商户发起提现操作",
                        Operator = business.Name,
                    }); //更新商户表的余额，可提现余额 
                    #endregion
                    tran.Complete();
                }
                return SimpleResultModel.Conclude(FinanceWithdrawB.Success); ;
            }
        }

        /// <summary>
        /// 商户提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150511
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <param name="business">商户</param>
        /// <param name="businessFinanceAccount">骑士金融账号信息</param>
        /// <returns></returns>
        private Tuple<bool, FinanceWithdrawB> CheckWithdrawB(WithdrawBPM withdrawBpm, ref Business business,
            ref  BusinessFinanceAccount businessFinanceAccount)
        {
            if (withdrawBpm.WithdrawPrice <= 0)   //提现金额小于等于0 提现有误
            {
                return new Tuple<bool, FinanceWithdrawB>(false, FinanceWithdrawB.WithdrawMoneyError);
            }
            business = _businessDao.GetById(withdrawBpm.BusinessId);//获取商户信息
            if (business == null || business.Status == null
                || business.Status != ConstValues.BUSINESS_AUDITPASS)  //商户状态为非 审核通过不允许 提现
            {
                return new Tuple<bool, FinanceWithdrawB>(false, FinanceWithdrawB.BusinessError);
            }
            else if (business.AllowWithdrawPrice < withdrawBpm.WithdrawPrice)//可提现金额小于提现金额，提现失败
            {
                return new Tuple<bool, FinanceWithdrawB>(false, FinanceWithdrawB.MoneyError);
            }
            var businessFinanceAccounts = _businessFinanceAccountDao.GetByBusinessId(withdrawBpm.BusinessId);//获取商户金融账号信息
            if (businessFinanceAccounts.Count <= 0)
            {
                return new Tuple<bool, FinanceWithdrawB>(false, FinanceWithdrawB.FinanceAccountError);
            }
            else
            {
                businessFinanceAccount = businessFinanceAccounts[0];
            }

            return new Tuple<bool, FinanceWithdrawB>(true, FinanceWithdrawB.Success);
        }

        #endregion

        #region  商户金融账号绑定/修改

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel CardBindB(CardBindBPM cardBindBpm)
        {
            if (cardBindBpm.AccountNo != cardBindBpm.AccountNo2) //两次录入的金融账号不一致
            {
                return SimpleResultModel.Conclude(FinanceCardBindB.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int count = _businessFinanceAccountDao.GetCountByBusinessId(cardBindBpm.BusinessId);
                if (count > 0)
                {
                    return SimpleResultModel.Conclude(FinanceCardBindB.Exists);//该商户已绑定过金融账号
                }
                int result = _businessFinanceAccountDao.Insert(new BusinessFinanceAccount()
                {
                    BusinessId = cardBindBpm.BusinessId,//商户ID
                    TrueName = cardBindBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindBpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true,// 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindBpm.AccountType == 0
                        ? (int)BusinessFinanceAccountType.WangYin : cardBindBpm.AccountType,  //账号类型 
                    OpenBank = cardBindBpm.OpenBank, //开户行
                    OpenSubBank = cardBindBpm.OpenSubBank, //开户支行
                    CreateBy = cardBindBpm.CreateBy,//创建人  当前登录人
                    UpdateBy = cardBindBpm.CreateBy//新增时最后修改人与新增人一致  当前登录人
                });
                tran.Complete();
                return SimpleResultModel.Conclude(SystemEnum.Success);
            }
        }


        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel CardModifyB(CardModifyBPM cardModifyBpm)
        {
            if (cardModifyBpm.AccountNo != cardModifyBpm.AccountNo2) //两次录入的金融账号不一致
            {
                return SimpleResultModel.Conclude(FinanceCardCardModifyB.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                _businessFinanceAccountDao.Update(new BusinessFinanceAccount()
                {
                    Id = cardModifyBpm.Id,
                    BusinessId = cardModifyBpm.BusinessId,//商户ID
                    TrueName = cardModifyBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardModifyBpm.AccountNo), //卡号(DES加密) 
                    OpenBank = cardModifyBpm.OpenBank, //开户行
                    OpenSubBank = cardModifyBpm.OpenSubBank, //开户支行
                    UpdateBy = cardModifyBpm.UpdateBy//修改人  当前登录人
                });
                tran.Complete();
                return SimpleResultModel.Conclude(SystemEnum.Success);
            }
        }
        #endregion

        /// <summary>
        ///  商户交易流水API add by caoheyang 20150512
        /// </summary>
        /// <returns></returns>
        public ResultModel<IList<FinanceRecordsDM>> GetRecords(int businessId)
        {
            IList<FinanceRecordsDM> records = _businessBalanceRecordDao.GetByBusinessId(businessId);
            return ResultModel<IList<FinanceRecordsDM>>.Conclude(SystemEnum.Success,
              TranslateRecords(records));
        }

        /// <summary>
        /// 商户交易流水API 信息处理转换 add by caoheyang 20150512
        /// </summary>
        /// <param name="records">原始流水记录</param>
        /// <returns></returns>
        private IList<FinanceRecordsDM> TranslateRecords(IList<FinanceRecordsDM> records)
        {
            foreach (var temp in records)
            {
                temp.StatusStr = ((BusinessBalanceRecordStatus)Enum.Parse(typeof(BusinessBalanceRecordStatus),
                    temp.Status.ToString(), false)).GetDisplayText(); //流水状态文本
                temp.RecordTypeStr =
                    ((BusinessBalanceRecordRecordType)Enum.Parse(typeof(BusinessBalanceRecordRecordType),
                        temp.RecordType.ToString(), false)).GetDisplayText(); //交易类型文本
                if (temp.YearInfo == DateTime.Now.Year + "年" + DateTime.Now.Month + "月")
                {
                    temp.YearInfo = "本月";
                }
                temp.OperateTimeStr = (
                    temp.OperateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")
                        ? "今天" //今日流水显示 "今日"
                        : temp.OperateTime.ToString("MM-dd"))
                        + " " +
                        temp.OperateTime.ToString("HH:mm"); //分
            }
            return records;
        }

        /// <summary>
        /// 根据申请单Id获取商家提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BusinessWithdrawFormModel GetBusinessWithdrawListById(string withwardId)
        {
            return businessFinanceDao.GetBusinessWithdrawListById(withwardId);
        }
        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<BusinessWithdrawLog> GetBusinessWithdrawOptionLog(string withwardId)
        {
            return businessFinanceDao.GetBusinessWithdrawOptionLog(withwardId);
        }
        /// <summary>
        /// 审核商户提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawAudit(BusinessWithdrawLog model)
        {
            return businessFinanceDao.BusinessWithdrawAudit(model);
        }
        /// <summary>
        /// 商户提现申请单确认打款
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawPayOk(BusinessWithdrawLog model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessFinanceDao.BusinessWithdrawPayOk(model))
                {
                    if (businessFinanceDao.ModifyBusinessBalanceRecordStatus(model.WithwardId.ToString()))
                    {
                        if (businessFinanceDao.ModifyBusinessTotalAmount(model.WithwardId.ToString()))
                        {
                            reg = true;
                            tran.Complete();
                        }
                    }
                }
            }
            return reg;
        }

        public BusinessDM GetDetails(int id)
        {
            return (new BusinessDao()).GetDetails(id);
        }
        /// <summary>
        /// 商户提现申请单审核拒绝
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawAuditRefuse(BusinessWithdrawLogModel model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessFinanceDao.BusinessWithdrawReturn(model))
                {
                    if (businessFinanceDao.BusinessWithdrawAuditRefuse(model))
                    {
                        if (businessFinanceDao.ModifyBusinessBalanceRecordStatus(model.WithwardId.ToString()))
                        {
                            if (businessFinanceDao.ModifyBusinessAmountInfo(model.WithwardId.ToString()))
                            {
                                reg = true;
                                tran.Complete();
                            }
                        }
                    }
                }
            }
            return reg;
        }
        /// <summary>
        /// 商户提现申请单打款失败
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawPayFailed(BusinessWithdrawLogModel model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessFinanceDao.BusinessWithdrawReturn(model))
                {
                    if (businessFinanceDao.BusinessWithdrawPayFailed(model))
                    {
                        if (businessFinanceDao.ModifyBusinessBalanceRecordStatus(model.WithwardId.ToString()))
                        {
                            if (businessFinanceDao.ModifyBusinessAmountInfo(model.WithwardId.ToString()))
                            {
                                reg = true;
                                tran.Complete();
                            }
                        }
                    }
                }
            }
            return reg;
        }

    }
}
