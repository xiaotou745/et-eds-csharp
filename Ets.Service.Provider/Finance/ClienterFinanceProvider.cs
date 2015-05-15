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
using ETS.Util;
using ETS.Data.PageData;

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
        ClienterFinanceDao clienterFinanceDao = new ClienterFinanceDao();
        #endregion

        #region 骑士提现功能  add by caoheyang 20150509

        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> WithdrawC(WithdrawCPM withdrawCpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                clienter clienter = new clienter();
                var clienterFinanceAccount = new ClienterFinanceAccount();//骑士金融账号信息
                FinanceWithdrawC checkbool = CheckWithdrawC(withdrawCpm, ref clienter, ref clienterFinanceAccount);
                if (checkbool != FinanceWithdrawC.Success)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                else
                {
                    _clienterDao.UpdateForWithdrawC(withdrawCpm); //更新骑士表的余额，可提现余额
                    string withwardNo = Helper.generateOrderCode(withdrawCpm.ClienterId);
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
                return ResultModel<object>.Conclude(FinanceWithdrawC.Success); ;
            }
        }

        /// <summary>
        /// 骑士提现功能检查数据合法性，判断是否满足提现要求 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawCpm">参数实体</param>
        /// <param name="clienter">骑士</param>
        /// <param name="clienterFinanceAccount">骑士金融账号信息</param>
        /// <returns></returns>
        private FinanceWithdrawC CheckWithdrawC(WithdrawCPM withdrawCpm, ref clienter clienter,
            ref  ClienterFinanceAccount clienterFinanceAccount)
        {
            if (withdrawCpm == null)
            {
                return  FinanceWithdrawC.NoPara;
            }
            if (withdrawCpm.WithdrawPrice <= 0)   //提现金额小于等于0 提现有误
            {
                return FinanceWithdrawC.WithdrawMoneyError;
            }
            clienter = _clienterDao.GetById(withdrawCpm.ClienterId);//获取超人信息
            if (clienter == null || clienter.Status == null
                || clienter.Status != ConstValues.CLIENTER_AUDITPASS)  //骑士状态为非 审核通过不允许 提现
            {
                return FinanceWithdrawC.ClienterError;
            }
            else if (clienter.AllowWithdrawPrice < withdrawCpm.WithdrawPrice) //可提现金额小于提现金额，提现失败
            {
                return  FinanceWithdrawC.MoneyError;
            }
            clienterFinanceAccount = _clienterFinanceAccountDao.GetById(withdrawCpm.FinanceAccountId);//获取超人金融账号信息
            if (clienterFinanceAccount == null || clienterFinanceAccount.ClienterId != withdrawCpm.ClienterId)
            {
                return FinanceWithdrawC.FinanceAccountError;
            }
            return FinanceWithdrawC.Success;
        }

        #endregion


        #region 骑士金融账号绑定/修改

        /// <summary>
        /// 骑士绑定银行卡功能 add by caoheyang 20150511 
        /// TODO  目前只支付网银
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardBindC(CardBindCPM cardBindCpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                 FinanceCardBindC checkbool = CheckCardBindC(cardBindCpm);  //验证数据合法性
                 if (checkbool != FinanceCardBindC.Success) 
                {
                    return ResultModel<object>.Conclude(checkbool); 
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
                return ResultModel<object>.Conclude(FinanceCardBindC.Success);
            }
        }

        /// <summary>
        ///  骑士绑定银行卡功能有效性验证 add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        private  FinanceCardBindC CheckCardBindC(CardBindCPM cardBindCpm)
        {
            if (cardBindCpm == null)
            {
                return FinanceCardBindC.NoPara;
            }
            int count = _clienterFinanceAccountDao.GetCountByClienterId(cardBindCpm.ClienterId);
            if (count > 0) //该骑士已绑定过金融账号
            {
                return FinanceCardBindC.Exists;
            }
            return FinanceCardBindC.Success;
        }

        /// <summary>
        /// 骑士修改绑定银行卡功能 add by caoheyang 20150511  TODO 统一加密算法
        /// </summary>
        /// <param name="cardModifyCpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardModifyC(CardModifyCPM cardModifyCpm)
        {
            FinanceCardCardModifyC checkbool = CheckCardModifyC(cardModifyCpm);  //验证数据合法性
            if (checkbool != FinanceCardCardModifyC.Success)
            {
                return ResultModel<object>.Conclude(checkbool);
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
                return ResultModel<object>.Conclude(SystemEnum.Success);
            }
        }

        /// <summary>
        /// 骑士修改绑定银行卡功能有效性验证  add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardModifyCpm"></param>
        /// <returns></returns>
        private  FinanceCardCardModifyC CheckCardModifyC(CardModifyCPM cardModifyCpm)
        {
            if (cardModifyCpm == null)
            {
                return FinanceCardCardModifyC.NoPara;
            }
            return FinanceCardCardModifyC.Success;
        }

        #endregion

        /// <summary>
        /// 骑士交易流水API add by caoheyang 20150511
        /// </summary> 
        /// <param name="clienterId">骑士id</param>
        /// <returns></returns>
        public ResultModel<object> GetRecords(int clienterId)
         {
             IList<FinanceRecordsDM> records = _clienterBalanceRecordDao.GetByClienterId(clienterId);
             return ResultModel<object>.Conclude(SystemEnum.Success,
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
        /// 根据参数获取骑士提现申请单列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ClienterWithdrawFormModel> GetClienterWithdrawList(ClienterWithdrawSearchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterWithdrawList<ClienterWithdrawFormModel>(criteria);
        }
        /// <summary>
        /// 根据申请单Id获取骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ClienterWithdrawFormModel GetClienterWithdrawListById(string withwardId)
        {
            return clienterFinanceDao.GetClienterWithdrawListById(withwardId);
        }
        /// <summary>
        /// 获取骑士提款单操作日志
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterWithdrawLog> GetClienterWithdrawOptionLog(string withwardId)
        {
            return clienterFinanceDao.GetClienterWithdrawOptionLog(withwardId);
        }
        /// <summary>
        /// 审核骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawAudit(ClienterWithdrawLog model)
        {
            return clienterFinanceDao.ClienterWithdrawAudit(model);
        }
        /// <summary>
        /// 骑士提现申请单确认打款
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawPayOk(ClienterWithdrawLog model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.ClienterWithdrawPayOk(model))
                {
                    if (clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString()))
                    {
                        if (clienterFinanceDao.ModifyClienterTotalAmount(model.WithwardId.ToString()))
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
        /// 骑士提现申请单审核拒绝
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawAuditRefuse(ClienterWithdrawLogModel model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.ClienterWithdrawReturn(model))
                {
                    if (clienterFinanceDao.ClienterWithdrawAuditRefuse(model))
                    {
                        if (clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString()))
                        {
                            if (clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
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
        /// 骑士提现申请单打款失败
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawPayFailed(ClienterWithdrawLogModel model)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.ClienterWithdrawReturn(model))
                {
                    if (clienterFinanceDao.ClienterWithdrawPayFailed(model))
                    {
                        if (clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString()))
                        {
                            if (clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
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
        /// 获取骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterBalanceRecord> GetClienterBalanceRecordList(ClienterBalanceRecordSerchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterBalanceRecordList(criteria);
        }
        /// <summary>
        /// 获取要导出的骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ClienterWithdrawFormModel> GetClienterWithdrawForExport(ClienterWithdrawSearchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterWithdrawForExport(criteria);
        }
        /// <summary>
        /// 获取要导出的骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterBalanceRecordModel> GetClienterBalanceRecordListForExport(ClienterBalanceRecordSerchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterBalanceRecordListForExport(criteria);
        }
        /// <summary>
        /// 生成excel文件
        /// 导出字段：骑士姓名、电话、开户行、账户名、卡号、提款金额
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        public string CreateClienterWithdrawFormExcel(List<ClienterWithdrawFormModel> list)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>骑士姓名</td>");
            strBuilder.AppendLine("<td>电话</td>");
            strBuilder.AppendLine("<td>开户行</td>");
            strBuilder.AppendLine("<td>账户名</td>");
            strBuilder.AppendLine("<td>卡号</td>");
            strBuilder.AppendLine("<td>提款金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in list)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}'</td>", item.ClienterName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.ClienterPhoneNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OpenBank));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.TrueName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }
        /// <summary>
        /// 生成excel文件
        /// 导出字段：任务单号/交易流水号、所属银行、卡号、收支金额、余额、完成时间、操作人
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        public string CreateClienterBalanceRecordExcel(List<ClienterBalanceRecordModel> list)
        {
            StringBuilder strBuilder = new StringBuilder();
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
                strBuilder.AppendLine(string.Format("<td>{0}</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Balance));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OperateTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Operator));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }
    }
}
