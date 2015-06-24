using System.Collections;
using Ets.Dao.Finance;
using Ets.Dao.User;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
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
        public ResultModel<object> WithdrawB(WithdrawBPM withdrawBpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                BusinessModel business = new BusinessModel();
                var businessFinanceAccount = new BusinessFinanceAccount();//商户金融账号信息
                FinanceWithdrawB checkbool = CheckWithdrawB(withdrawBpm, ref business, ref businessFinanceAccount);
                if (checkbool!=FinanceWithdrawB.Success)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                else
                {
                    _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                    {
                        Id=withdrawBpm.BusinessId,
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
                return  FinanceWithdrawB.NoPara;
            }
            business = _businessDao.GetById(withdrawBpm.BusinessId);//获取商户信息
            if (business == null || business.Status == null
                || business.Status != ConstValues.BUSINESS_AUDITPASS)  //商户状态为非 审核通过不允许 提现
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
                return  FinanceWithdrawB.FinanceAccountError;
            }
            return FinanceWithdrawB.Success;
        }

        #endregion

        #region  商户金融账号绑定/修改

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardBindB(CardBindBPM cardBindBpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                FinanceCardBindB checkbool = CheckCardBindB(cardBindBpm);  //验证数据合法性
                if (checkbool != FinanceCardBindB.Success)
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                int result = _businessFinanceAccountDao.Insert(new BusinessFinanceAccount()
                {
                    BusinessId = cardBindBpm.BusinessId,//商户ID
                    TrueName = cardBindBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindBpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true,// 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindBpm.AccountType == 0
                        ? (int)BusinessFinanceAccountType.WangYin : cardBindBpm.AccountType,  //账号类型 
                    BelongType = cardBindBpm.BelongType,//账号类别  0 个人账户 1 公司账户  
                    OpenBank = cardBindBpm.OpenBank, //开户行
                    OpenSubBank = cardBindBpm.OpenSubBank, //开户支行
                    CreateBy = cardBindBpm.CreateBy,//创建人  当前登录人
                    UpdateBy = cardBindBpm.CreateBy//新增时最后修改人与新增人一致  当前登录人
                });
                tran.Complete();
                return ResultModel<object>.Conclude(SystemEnum.Success);
            }
        }

        /// <summary>
        ///  商户绑定银行卡功能有效性验证 add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        private  FinanceCardBindB CheckCardBindB(CardBindBPM cardBindBpm)
        {
            if (cardBindBpm == null)
            {
                return  FinanceCardBindB.NoPara;
            }
            if (cardBindBpm.BelongType == (int)BusinessFinanceAccountBelongType.Conpany
               && string.IsNullOrWhiteSpace(cardBindBpm.OpenSubBank)) //公司帐户开户支行不能为空
            {
                return FinanceCardBindB.BelongTypeError;
            }
            int count = _businessFinanceAccountDao.GetCountByBusinessId(cardBindBpm.BusinessId);
            if (count > 0) //该商户已绑定过金融账号
            {
                return  FinanceCardBindB.Exists;
            }
            return FinanceCardBindB.Success;
        }


        /// <summary>
        /// 商户修改绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardModifyBpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardModifyB(CardModifyBPM cardModifyBpm)
        {
            FinanceCardModifyB boolRes = CheckCardModifyB(cardModifyBpm);
            if (boolRes != FinanceCardModifyB.Success)
            {
                return ResultModel<object>.Conclude(boolRes);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                _businessFinanceAccountDao.Update(new BusinessFinanceAccount()
                {
                    Id = cardModifyBpm.Id,
                    BusinessId = cardModifyBpm.BusinessId,//商户ID
                    TrueName = cardModifyBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardModifyBpm.AccountNo), //卡号(DES加密) 
                    BelongType = cardModifyBpm.BelongType,//账号类别  0 个人账户 1 公司账户  
                    OpenBank = cardModifyBpm.OpenBank, //开户行
                    OpenSubBank = cardModifyBpm.OpenSubBank, //开户支行
                    UpdateBy = cardModifyBpm.UpdateBy//修改人  当前登录人
                });
                tran.Complete();
                return ResultModel<object>.Conclude(SystemEnum.Success);
            }
        }


        /// <summary>
        /// 商户修改绑定银行卡功能有效性验证  add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardModifyBpm"></param>
        /// <returns></returns>
        private FinanceCardModifyB CheckCardModifyB(CardModifyBPM cardModifyBpm)
        {
            if (cardModifyBpm == null)
            {
                return FinanceCardModifyB.NoPara;
            }
            if (cardModifyBpm.BelongType == (int)BusinessFinanceAccountBelongType.Conpany
               && string.IsNullOrWhiteSpace(cardModifyBpm.OpenSubBank)) //公司帐户开户支行不能为空
            {
                return FinanceCardModifyB.BelongTypeError;
            }
            return FinanceCardModifyB.Success;
        }
        #endregion

        /// <summary>
        ///  商户交易流水API add by caoheyang 20150512
        /// </summary>
        /// <returns></returns>
        public ResultModel<object> GetRecords(int businessId)
        {
            IList<FinanceRecordsDM> records = _businessBalanceRecordDao.GetByBusinessId(businessId);
            return ResultModel<object>.Conclude(SystemEnum.Success,
              TranslateRecords(records));
        }

        /// <summary>
        /// 商户交易流水API 信息处理转换 add by caoheyang 20150512
        /// </summary>
        /// <param name="records">原始流水记录</param>
        /// <returns></returns>
        private IList<FinanceRecordsDMList> TranslateRecords(IList<FinanceRecordsDM> records)
        {
            IList<FinanceRecordsDMList> datas = new List<FinanceRecordsDMList>();
            datas.Add(new FinanceRecordsDMList() { MonthIfo = "本月", Datas = new List<FinanceRecordsDM>() });
            int index = 0; 
            foreach (var temp in records)
            {
                temp.StatusStr = ((BusinessBalanceRecordStatus)Enum.Parse(typeof(BusinessBalanceRecordStatus),
                    temp.Status.ToString(), false)).GetDisplayText(); //流水状态文本
                temp.RecordTypeStr =
                    ((BusinessBalanceRecordRecordType)Enum.Parse(typeof(BusinessBalanceRecordRecordType),
                        temp.RecordType.ToString(), false)).GetDisplayText(); //交易类型文本
                if (datas[index].MonthIfo == temp.MonthInfo)
                {
                    datas[index].Datas.Add(temp);
                }
                else
                {
                    datas.Add(new FinanceRecordsDMList() { MonthIfo = temp.MonthInfo, Datas = new List<FinanceRecordsDM>() });
                    index = index + 1;
                    datas[index].Datas.Add(temp);
                }
            }
            return datas;
        }

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
                strBuilder.AppendLine(string.Format("<td>{0}</td>",item.BusinessPhoneNo));
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
    }
}
