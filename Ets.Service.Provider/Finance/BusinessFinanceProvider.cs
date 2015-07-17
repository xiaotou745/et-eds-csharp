using System.Collections;
using ETS;
using Ets.Dao.Finance;
using Ets.Dao.User;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.Common.YeePay;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Pay.YeePay;
using ETS.Security;
using Ets.Service.IProvider.Finance;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Text;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using Ets.Dao.Business;
using Ets.Dao.Clienter;
using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;

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

        private ClienterDao clienterDao = new ClienterDao();
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
        public ResultModel<object> WithdrawB(WithdrawBPM withdrawBpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                BusinessModel business = new BusinessModel();
                var businessFinanceAccount = new BusinessFinanceAccount();//商户金融账号信息
                FinanceWithdrawB checkbool = CheckWithdrawB(withdrawBpm, ref business, ref businessFinanceAccount);
                if (checkbool != FinanceWithdrawB.Success)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                else
                {
                    _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                    {
                        Id = withdrawBpm.BusinessId,
                        Money = -withdrawBpm.WithdrawPrice
                    }); //更新商户表的余额，可提现余额
                    string withwardNo = Helper.generateOrderCode(withdrawBpm.BusinessId);
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
                        BelongType = businessFinanceAccount.BelongType,//账号类别  0 个人账户 1 公司账户  
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
                        RecordType = (int)BusinessBalanceRecordRecordType.WithdrawApply,
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
                return ResultModel<object>.Conclude(FinanceWithdrawB.Success); ;
            }
        }
        /// <summary>
        /// 商户提现功能 后台
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="withdrawBBackPM"></param>
        /// <returns></returns>
        public ResultModel<object> WithdrawB(WithdrawBBackPM withdrawBBackPM)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                BusinessModel business = new BusinessModel();
                var businessFinanceAccount = new BusinessFinanceAccount();//商户金融账号信息
                FinanceWithdrawB checkbool = CheckWithdrawB(withdrawBBackPM, ref business, ref businessFinanceAccount);
                if (checkbool != FinanceWithdrawB.Success)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                else
                {
                    _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                    {
                        Id = withdrawBBackPM.BusinessId,
                        Money = -withdrawBBackPM.WithdrawPrice
                    }); //更新商户表的余额，可提现余额
                    string withwardNo = Helper.generateOrderCode(withdrawBBackPM.BusinessId);
                    GlobalConfigModel globalConfig = GlobalConfigDao.GlobalConfigGet(0);
                    #region 商户提现
                    long withwardId = _businessWithdrawFormDao.Insert(new BusinessWithdrawForm()
                    {
                        WithwardNo = withwardNo,//单号 规则待定
                        BusinessId = withdrawBBackPM.BusinessId,//商户Id
                        BalancePrice = business.BalancePrice,//提现前商户余额
                        AllowWithdrawPrice = business.AllowWithdrawPrice,//提现前商户可提现金额
                        Status = (int)BusinessWithdrawFormStatus.WaitAllow,//待审核
                        Amount = withdrawBBackPM.WithdrawPrice,//提现金额
                        Balance = business.BalancePrice - withdrawBBackPM.WithdrawPrice, //提现后余额
                        TrueName = businessFinanceAccount.TrueName,//商户收款户名
                        AccountNo = businessFinanceAccount.AccountNo, //卡号(DES加密)
                        AccountType = businessFinanceAccount.AccountType, //账号类型：
                        BelongType = businessFinanceAccount.BelongType,//账号类别  0 个人账户 1 公司账户  
                        OpenBank = businessFinanceAccount.OpenBank,//开户行
                        OpenSubBank = businessFinanceAccount.OpenSubBank, //开户支行

                        IDCard = withdrawBBackPM.IDCard,//申请提款身份证号
                        OpenCity = withdrawBBackPM.OpenCity,//城市
                        OpenCityCode = withdrawBBackPM.OpenCityCode,//城市代码
                        OpenProvince = withdrawBBackPM.OpenProvince,//省份
                        OpenProvinceCode = withdrawBBackPM.OpenProvinceCode,//省份代码
                                  HandCharge = Convert.ToInt32(globalConfig.WithdrawCommission),//手续费
                        HandChargeOutlay = withdrawBBackPM.WithdrawPrice > Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney) ? HandChargeOutlay.EDaiSong : HandChargeOutlay.Private,//手续费支出方
                                  HandChargeThreshold = Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney)//手续费阈值
                              
                    });
                    #endregion

                    #region 商户余额流水操作 更新骑士表的余额，可提现余额
                    _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                    {
                        BusinessId = withdrawBBackPM.BusinessId,//商户Id
                        Amount = -withdrawBBackPM.WithdrawPrice,//流水金额
                        Status = (int)BusinessBalanceRecordStatus.Tradeing, //流水状态(1、交易成功 2、交易中）
                        RecordType = (int)BusinessBalanceRecordRecordType.WithdrawApply,
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
                        Remark = withdrawBBackPM.Remarks,
                        Operator = business.Name,
                    }); //更新商户表的余额，可提现余额 
                    #endregion
                    tran.Complete();
                }
                return ResultModel<object>.Conclude(FinanceWithdrawB.Success); ;
            }
        }
        /// <summary>
        /// 商户提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150511
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <param name="business">商户</param>
        /// <param name="businessFinanceAccount">骑士金融账号信息</param>
        /// <returns></returns>
        private FinanceWithdrawB CheckWithdrawB(WithdrawBPM withdrawBpm, ref BusinessModel business,
            ref  BusinessFinanceAccount businessFinanceAccount)
        {
            if (withdrawBpm == null)
            {
                return FinanceWithdrawB.NoPara;
            }
            business = _businessDao.GetById(withdrawBpm.BusinessId);//获取商户信息
            if (business == null || business.Status == null
                || business.Status != BusinessStatus.Status1.GetHashCode())  //商户状态为非 审核通过不允许 提现
            {
                return FinanceWithdrawB.BusinessError;
            }
            else if (business.AllowWithdrawPrice < withdrawBpm.WithdrawPrice)//可提现金额小于提现金额，提现失败
            {
                return FinanceWithdrawB.MoneyError;
            }
            else if (business.BalancePrice < business.AllowWithdrawPrice) //账户余额小于 可提现金额，提现失败 账号异常
            {
                return FinanceWithdrawB.FinanceAccountError;
            }
            businessFinanceAccount = _businessFinanceAccountDao.GetById(withdrawBpm.FinanceAccountId);//获取商户金融账号信息
            if (businessFinanceAccount == null || businessFinanceAccount.BusinessId != withdrawBpm.BusinessId)
            {
                return FinanceWithdrawB.FinanceAccountError;
            }
            return FinanceWithdrawB.Success;
        }

        #endregion


        /// <summary>
        /// 根据申请单Id获取商家提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public BusinessWithdrawFormModel GetBusinessWithdrawListById(string withwardId)
        {
            return businessFinanceDao.GetBusinessWithdrawListById(withwardId);
        }
        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            bool isBussinessIDValid = clienterDao.IsBussinessOrClienterValidByID(0, model.WithwardId);
            if (isBussinessIDValid)
            {
                return businessFinanceDao.BusinessWithdrawAudit(model);
            }
            throw new Exception("提款单对应的商户已经被取消资格，请联系客服");
        }
        /// <summary>
        /// 商户提现申请单确认打款调用易宝接口
        /// danny-20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo BusinessWithdrawPaying(BusinessWithdrawLog model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            var busiFinanceAccount = businessFinanceDao.GetBusinessFinanceAccount(model.WithwardId.ToString());
            if (busiFinanceAccount == null)
            {
                dealResultInfo.DealMsg = "获取提现单信息失败！";
                return dealResultInfo;
            }
            if (busiFinanceAccount.WithdrawTime < ParseHelper.ToDatetime(Config.WithdrawTime))
            {
                dealResultInfo.DealFlag = BusinessWithdrawPayOk(model);
                dealResultInfo.DealMsg = dealResultInfo.DealFlag ? "打款成功！" : "打款失败！";
                return dealResultInfo;
            }
            decimal amount = busiFinanceAccount.HandChargeOutlay == 0
                ? busiFinanceAccount.Amount
                : busiFinanceAccount.Amount + busiFinanceAccount.HandCharge;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //注册易宝子账户逻辑
                if (string.IsNullOrEmpty(busiFinanceAccount.YeepayKey) || busiFinanceAccount.YeepayStatus == 1)
                {
                    var brp = new YeeRegisterParameter
                    {
                        BindMobile = busiFinanceAccount.PhoneNo,
                        SignedName = busiFinanceAccount.TrueName,
                        CustomerType =
                            busiFinanceAccount.BelongType == 0
                                ? CustomertypeEnum.PERSON
                                : CustomertypeEnum.ENTERPRISE,
                        LinkMan = busiFinanceAccount.TrueName,
                        IdCard = busiFinanceAccount.BusiIDCard,
                        BusinessLicence = busiFinanceAccount.IDCard,
                        LegalPerson = busiFinanceAccount.TrueName,
                        BankAccountNumber = busiFinanceAccount.AccountNo,
                        BankName = busiFinanceAccount.OpenBank,
                        AccountName = busiFinanceAccount.TrueName,
                        BankProvince = busiFinanceAccount.OpenProvince,
                        BankCity = busiFinanceAccount.OpenCity
                    };
                    var dr = DealRegBusiSubAccount(brp);
                    if (!dr.DealFlag)
                    {
                        dealResultInfo.DealMsg = dr.DealMsg;
                        return dealResultInfo;
                    }
                    busiFinanceAccount.YeepayKey = dr.SuccessId; //子账户id
                }
                //转账逻辑
                var regTransfer = new Transfer().TransferAccounts("", amount.ToString(),
                    busiFinanceAccount.YeepayKey); //转账   子账户转给总账户
                if (regTransfer.code != "1")
                {
                    dealResultInfo.DealMsg = "商户易宝自动转账失败：" + regTransfer.code;
                    return dealResultInfo;
                }
                var regCash = new Transfer().CashTransfer(APP.B, ParseHelper.ToInt(model.WithwardId),
                    busiFinanceAccount.YeepayKey, amount.ToString()); //提现
                if (regCash.code != "1")
                {
                    dealResultInfo.DealMsg = "商户易宝自动提现失败：" + regCash.code;
                    return dealResultInfo;
                }
                if(!businessFinanceDao.BusinessWithdrawPayOk(model))
                {
                    dealResultInfo.DealMsg = "更改提现单状态为打款中失败！";
                    return dealResultInfo;
                }
                dealResultInfo.DealFlag = true;
                dealResultInfo.DealMsg = "商户提现单确认打款处理成功，等待银行打款！";
                tran.Complete();
                return dealResultInfo;
            }
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
                if (businessFinanceDao.BusinessWithdrawReturn(model)   //提现返现
                    && businessFinanceDao.BusinessWithdrawPayFailed(model) //更新打款失败
                    && businessFinanceDao.ModifyBusinessBalanceRecordStatus(model.WithwardId.ToString())  //修改流水 
                    && businessFinanceDao.ModifyBusinessAmountInfo(model.WithwardId.ToString()))  //提现单
                {
                    reg = true;
                    tran.Complete();
                }
            }
            return reg;
        }


        /// <summary>
        /// 易宝打款失败回调处理逻辑 
        /// add by caoheyang  20150716
        /// </summary>
        /// <param name="model"></param>
        ///  <param name="callback"></param>
        /// <returns></returns>
        public bool BusinessWithdrawPayFailed(BusinessWithdrawLogModel model, CashTransferCallback callback)
        {
            bool reg = false;
            var withdraw = _businessWithdrawFormDao.GetById(model.WithwardId);  //提现单
            if (withdraw == null)
            {
                return false;
            }
            Transfer transfer = new Transfer();
            TransferReturnModel tempmodel = transfer.TransferAccounts("",
                (ParseHelper.ToDecimal(callback.amount) - withdraw.HandCharge).ToString(),callback.ledgerno);
            if (tempmodel.code == "1") //易宝子账户到主账户打款 成功
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    if (businessFinanceDao.BusinessWithdrawReturn(model) //提现返现
                        && businessFinanceDao.BusinessWithdrawPayFailed(model) //更新打款失败
                        && businessFinanceDao.ModifyBusinessBalanceRecordStatus(model.WithwardId.ToString()) //修改流水 
                        && businessFinanceDao.ModifyBusinessAmountInfo(model.WithwardId.ToString())) //提现单
                    {
                        if (withdraw.HandChargeOutlay == 0) //个人支出手续费  增加手续费扣款记录流水 
                        {
                            _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                            {
                                Id = withdraw.BusinessId,
                                Money = -withdraw.HandCharge
                            }); //更新商户表的余额，可提现余额
                            _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                            {
                                BusinessId = withdraw.BusinessId, //商户Id
                                Amount = -withdraw.HandCharge, //手续费金额
                                Status = (int) BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                                RecordType = (int) BusinessBalanceRecordRecordType.ProcedureFee,
                                Operator = "易宝系统回调",
                                WithwardId = withdraw.Id,
                                RelationNo = withdraw.WithwardNo,
                                Remark = "易宝提现失败扣除手续费",
                            });
                        }
                        reg = true;
                        tran.Complete();
                    }
                }
            }
            else
            {
                LogHelper.LogWriterString("易宝子账户到主账户打款导致失败，提现单号为:" + model.WithwardId);
            }
            return reg;
        }

        /// <summary>
        /// 获取商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusinessBalanceRecord> GetBusinessBalanceRecordList(BusinessBalanceRecordSerchCriteria criteria)
        {
            return businessFinanceDao.GetBusinessBalanceRecordList(criteria);
        }
        /// <summary>
        /// 获取要导出的商户提现申请单
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusinessWithdrawFormModel> GetBusinessWithdrawForExport(BusinessWithdrawSearchCriteria criteria)
        {
            return businessFinanceDao.GetBusinessWithdrawForExport(criteria);
        }
        /// <summary>
        /// 获取要导出的商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusinessBalanceRecordModel> GetBusinessBalanceRecordListForExport(BusinessBalanceRecordSerchCriteria criteria)
        {
            return businessFinanceDao.GetBusinessBalanceRecordListForExport(criteria);
        }
        /// <summary>
        /// 生成excel文件
        /// 导出字段：商户名称、电话、开户行、账户名、卡号、提款金额
        /// danny-20150512
        /// </summary>
        /// <returns></returns>
        public string CreateBusinessWithdrawFormExcel(List<BusinessWithdrawFormModel> list)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>商户名称</td>");
            strBuilder.AppendLine("<td>电话</td>");
            strBuilder.AppendLine("<td>开户行</td>");
            strBuilder.AppendLine("<td>账户名</td>");
            strBuilder.AppendLine("<td>卡号</td>");
            strBuilder.AppendLine("<td>提款金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in list)
            {
                strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", item.BusinessName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.BusinessPhoneNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OpenBank));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.TrueName));
                strBuilder.AppendLine(string.Format("<td>'{0}'</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", item.Amount));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }
        /// <summary>
        /// 生成excel文件
        /// 导出字段：任务单号/交易流水号、所属银行、卡号、收支金额、余额、完成时间、操作人
        /// danny-20150512
        /// </summary>
        /// <returns></returns>
        public string CreateBusinessBalanceRecordExcel(List<BusinessBalanceRecordModel> list)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>任务单号/交易流水号</td>");
            strBuilder.AppendLine("<td>所属银行</td>");
            strBuilder.AppendLine("<td>卡号</td>");
            strBuilder.AppendLine("<td>收支金额</td>");
            strBuilder.AppendLine("<td>余额</td>");
            strBuilder.AppendLine("<td>完成时间</td>");
            strBuilder.AppendLine("<td>操作人</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in list)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}'</td>", item.RelationNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OpenBank));
                strBuilder.AppendLine(string.Format("<td>'{0}'</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Balance));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OperateTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", item.Operator));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }

        public decimal GetBusiBalance(string orderNo)
        {
            return businessFinanceDao.GetBusiBalance(orderNo);
        }

        /// <summary>
        /// 商户充值增加商家余额可提现和插入商家余额流水
        /// danny-20150526
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessRecharge(BusinessOptionLog model)
        {
            return businessFinanceDao.BusinessRecharge(model);
        }
        /// <summary>
        /// 获取商户提款收支记录列表分页版
        /// danny-20150604
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<BusinessBalanceRecord> GetBusinessBalanceRecordListOfPaging(BusinessBalanceRecordSerchCriteria criteria)
        {
            return businessFinanceDao.GetBusinessBalanceRecordListOfPaging<BusinessBalanceRecord>(criteria);
        }
        /// <summary>
        /// 根据单号查询充值详情
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150624</UpdateTime>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public BusinessRechargeDetail GetBusinessRechargeDetailByNo(string orderNo)
        {
            return businessFinanceDao.GetBusinessRechargeDetailByNo(orderNo);
        }
        /// <summary>
        /// 调用商户易宝子账号注册接口并对返回值进行处理
        /// danny-20150716
        /// </summary>
        /// <param name="model"></param>
        public DealResultInfo DealRegBusiSubAccount(YeeRegisterParameter model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            var registResult = new Register().RegSubaccount(model);//注册帐号
            if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            {
                if (!_businessFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.AccountId), registResult.ledgerno, 0))
                {
                    dealResultInfo.DealMsg = "调用易宝用户注册成功，回写数据库失败！";
                }
                else
                {
                    dealResultInfo.DealFlag = true;
                    dealResultInfo.SuccessId = registResult.ledgerno;
                    dealResultInfo.DealMsg = "商户绑定易宝支付成功！";
                }
            }
            else
            {
                _businessFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.AccountId), "", 1);
                if (registResult == null)
                {
                    dealResultInfo.DealMsg = "商户绑定易宝支付失败,返回结果为null!";
                }
                else
                {
                    LogHelper.LogWriterString("商户绑定易宝支付失败",
                        string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                    dealResultInfo.DealMsg = string.Format("商户绑定易宝支付失败,易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}", registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg);
                }
            }
            return dealResultInfo;
        }
        #region 用户自定义方法
        /// <summary>
        /// 商户提现功能检查数据合法性
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="withdrawBpm">参数实体</param>
        /// <param name="business">商户</param>
        /// <param name="businessFinanceAccount">商家金融账号信息</param>
        /// <returns></returns>
        private FinanceWithdrawB CheckWithdrawB(WithdrawBBackPM withdrawBpm, ref BusinessModel business,
            ref  BusinessFinanceAccount businessFinanceAccount)
        {
            if (withdrawBpm == null)
            {
                return FinanceWithdrawB.NoPara;
            }
            business = _businessDao.GetById(withdrawBpm.BusinessId);//获取商户信息
            if (business == null || business.Status == null
                || business.Status != (byte)BusinessStatus.Status1.GetHashCode())  //商户状态为非 审核通过不允许 提现
            {
                return FinanceWithdrawB.BusinessError;
            }
            else if (business.AllowWithdrawPrice < withdrawBpm.WithdrawPrice)//可提现金额小于提现金额，提现失败
            {
                return FinanceWithdrawB.MoneyError;
            }
            else if (business.BalancePrice < business.AllowWithdrawPrice) //账户余额小于 可提现金额，提现失败 账号异常
            {
                return FinanceWithdrawB.FinanceAccountError;
            }
            businessFinanceAccount = _businessFinanceAccountDao.GetById(withdrawBpm.FinanceAccountId);//获取商户金融账号信息
            if (businessFinanceAccount == null || businessFinanceAccount.BusinessId != withdrawBpm.BusinessId)
            {
                return FinanceWithdrawB.FinanceAccountError;
            }
            return FinanceWithdrawB.Success;
        }
        #endregion
    }
}
