using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ETS;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using Ets.Dao.GlobalConfig;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.Common.YeePay;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Model.ParameterModel.Finance;
using ETS.Pay.YeePay;
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
                    _clienterDao.UpdateForWithdrawC(new UpdateForWithdrawPM
                    {
                        Id = withdrawCpm.ClienterId,
                        Money = -withdrawCpm.WithdrawPrice
                    }); //更新骑士表的余额，可提现余额
                    string withwardNo = Helper.generateOrderCode(withdrawCpm.ClienterId);
                    GlobalConfigModel globalConfig = GlobalConfigDao.GlobalConfigGet(0);
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
                                  BelongType = clienterFinanceAccount.BelongType,//账号类别  0 个人账户 1 公司账户  
                                  OpenBank = clienterFinanceAccount.OpenBank,//开户行
                                  OpenSubBank = clienterFinanceAccount.OpenSubBank, //开户支行
                                  IDCard = withdrawCpm.IDCard,//申请提款身份证号
                                  OpenCity = withdrawCpm.OpenCity,//城市
                                  OpenCityCode = withdrawCpm.OpenCityCode,//城市代码
                                  OpenProvince = withdrawCpm.OpenProvince,//省份
                                  OpenProvinceCode = withdrawCpm.OpenProvinceCode,//省份代码
                                  HandCharge = Convert.ToInt32(globalConfig.WithdrawCommission),//手续费
                                  HandChargeOutlay = withdrawCpm.WithdrawPrice > Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney) ? HandChargeOutlay.EDaiSong : HandChargeOutlay.Private,//手续费支出方
                                  HandChargeThreshold = Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney)//手续费阈值
                              });
                    #endregion

                    #region 骑士余额流水操作 更新骑士表的余额，可提现余额
                    _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                            {
                                ClienterId = withdrawCpm.ClienterId,//骑士Id(Clienter表）
                                Amount = -withdrawCpm.WithdrawPrice,//流水金额
                                Status = (int)ClienterBalanceRecordStatus.Tradeing, //流水状态(1、交易成功 2、交易中）
                                RecordType = (int)ClienterBalanceRecordRecordType.WithdrawApply,
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
                return FinanceWithdrawC.NoPara;
            }
            if (string.IsNullOrWhiteSpace(withdrawCpm.OpenProvince))
                return FinanceWithdrawC.NoOpenProvince;
            //if (withdrawCpm.OpenProvinceCode == 0)
            //    return FinanceWithdrawC.NoOpenProvinceCode;
            if (string.IsNullOrWhiteSpace(withdrawCpm.OpenCity))
                return FinanceWithdrawC.NoOpenCity;
            //if (withdrawCpm.OpenCityCode == 0)
            //    return FinanceWithdrawC.NoOpenCityCode;
            if (!Regex.IsMatch(withdrawCpm.IDCard, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                return FinanceWithdrawC.NoIDCard;
            if (withdrawCpm.WithdrawPrice % 100 != 0 || withdrawCpm.WithdrawPrice < 100
                || withdrawCpm.WithdrawPrice > 3000) //提现金额小于500 加2手续费
            {
                return FinanceWithdrawC.MoneyDoubleError;
            }
            clienter = _clienterDao.GetById(withdrawCpm.ClienterId);//获取超人信息
            if (clienter == null || clienter.Status == null
                || clienter.Status != ClienteStatus.Status1.GetHashCode())  //骑士状态为非 审核通过不允许 提现
            {
                return FinanceWithdrawC.ClienterError;
            }
            else if (clienter.AllowWithdrawPrice < withdrawCpm.WithdrawPrice) //可提现金额小于提现金额，提现失败
            {
                return FinanceWithdrawC.MoneyError;
            }
            else if (clienter.AccountBalance < clienter.AllowWithdrawPrice) //账户余额小于 可提现金额，提现失败 账号异常
            {
                return FinanceWithdrawC.FinanceAccountError;
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
        /// 骑士绑定银行卡功能 add by caoheyang 20150511    modify by 彭宜  20150717   绑定银行卡时会和易宝支付关联
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardBindC(CardBindCPM cardBindCpm)
        {
            #region 参数验证
            FinanceCardBindC checkbool = CheckCardBindC(cardBindCpm);  //验证数据合法性
            if (checkbool != FinanceCardBindC.Success)
            {
                return ResultModel<object>.Conclude(checkbool);
            }
            #endregion
            var result = 0;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                result = _clienterFinanceAccountDao.Insert(new ClienterFinanceAccount()
                {
                    ClienterId = cardBindCpm.ClienterId,//骑士ID
                    TrueName = cardBindCpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindCpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true,// 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindCpm.AccountType == 0
                        ? (int)ClienterFinanceAccountType.WangYin : cardBindCpm.AccountType,  //账号类型 
                    BelongType = cardBindCpm.BelongType,//账号类别  0 个人账户 1 公司账户  
                    OpenBank = cardBindCpm.OpenBank, //开户行
                    OpenSubBank = cardBindCpm.OpenSubBank, //开户支行
                    CreateBy = cardBindCpm.CreateBy,//创建人  当前登录人
                    UpdateBy = cardBindCpm.CreateBy,//新增时最后修改人与新增人一致  当前登录人
                    OpenCity = cardBindCpm.OpenCity,//开户行
                    OpenProvince = cardBindCpm.OpenProvince,//开户市
                    IDCard = cardBindCpm.IDCard,//身份证号
                    OpenCityCode = cardBindCpm.OpenCityCode,//市编码
                    OpenProvinceCode = cardBindCpm.OpenProvinceCode,//省编码
                });
                tran.Complete();
            }
            #region 异步请求易宝注册接口
            Task.Factory.StartNew(() =>
            {
                //请求易宝注册接口,如果成功,则更新账户易宝key和status
                var phoneNo = _clienterDao.GetPhoneNo(cardBindCpm.ClienterId);
                var parameter = new YeeRegisterParameter()
                {
                    AccountName = cardBindCpm.TrueName,
                    BankAccountNumber = cardBindCpm.AccountNo,
                    BankCity = cardBindCpm.OpenCity,
                    BankName = cardBindCpm.OpenBank,
                    BankProvince = cardBindCpm.OpenProvince,
                    BindMobile = phoneNo,
                    BusinessLicence = "",
                    IdCard = cardBindCpm.IDCard,
                    CustomerType = (cardBindCpm.BelongType == 0
                        ? CustomertypeEnum.PERSON
                        : CustomertypeEnum.ENTERPRISE),
                    LegalPerson = cardBindCpm.TrueName,
                    LinkMan = cardBindCpm.TrueName,
                    SignedName = cardBindCpm.TrueName,
                };
                var result1 = new Register().RegSubaccount(parameter);//注册帐号
                if (result1 != null && !string.IsNullOrEmpty(result1.code) && result1.code.Trim() == "1")
                {
                    _clienterFinanceAccountDao.UpdateYeepayInfoById(result, result1.ledgerno, 0);
                }
                else
                {
                    if (result1 == null)
                    {
                        //ETS.Util.LogHelper.LogWriterString("骑士绑定易宝支付失败", string.Format("返回结果为null"));
                        return;
                    }
                    else
                    {
                        ETS.Util.LogHelper.LogWriterString("骑士绑定易宝支付失败", string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            result1.code, result1.ledgerno, result1.hmac, result1.msg));
                    }
                }
            });
            #endregion
            return ResultModel<object>.Conclude(FinanceCardBindC.Success);
        }

        /// <summary>
        ///  骑士绑定银行卡功能有效性验证 add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardBindCpm">参数实体</param>
        /// <returns></returns>
        private FinanceCardBindC CheckCardBindC(CardBindCPM cardBindCpm)
        {
            if (cardBindCpm == null)
            {
                return FinanceCardBindC.NoPara;
            }
            if (cardBindCpm.BelongType == (int)ClienterFinanceAccountBelongType.Conpany
                && string.IsNullOrWhiteSpace(cardBindCpm.OpenSubBank)) //公司帐户开户支行不能为空
            {
                return FinanceCardBindC.BelongTypeError;
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
            FinanceCardModifyC checkbool = CheckCardModifyC(cardModifyCpm);  //验证数据合法性
            if (checkbool != FinanceCardModifyC.Success)
            {
                return ResultModel<object>.Conclude(checkbool);
            }
            int withdrawCount= _clienterWithdrawFormDao.GetByClienterId(cardModifyCpm.ClienterId);
            if (withdrawCount > 0) //该骑士是否存在未完成的提现单
            {
                return ResultModel<object>.Conclude(FinanceCardModifyC.NoModify);
            }
            ClienterFinanceAccount cfAccount = _clienterFinanceAccountDao.GetById(cardModifyCpm.Id);
            if (cfAccount != null)
            {
                if (cfAccount.OpenBank != cardModifyCpm.OpenBank || cfAccount.OpenSubBank != cardModifyCpm.OpenSubBank ||
                    cfAccount.OpenProvince != cardModifyCpm.OpenCity || cfAccount.IDCard != cardModifyCpm.IDCard ||
                    cfAccount.TrueName != cardModifyCpm.TrueName ||
                    cfAccount.AccountNo != DES.Encrypt(cardModifyCpm.AccountNo))
                {
                    //1.修改
                    using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                    {
                        _clienterFinanceAccountDao.Update(new ClienterFinanceAccount()
                        {
                            Id = cardModifyCpm.Id,
                            ClienterId = cardModifyCpm.ClienterId, //骑士ID
                            TrueName = cardModifyCpm.TrueName, //户名
                            AccountNo = DES.Encrypt(cardModifyCpm.AccountNo), //卡号(DES加密)
                            BelongType = cardModifyCpm.BelongType, //账号类别  0 个人账户 1 公司账户  
                            OpenBank = cardModifyCpm.OpenBank, //开户行
                            OpenSubBank = cardModifyCpm.OpenSubBank, //开户支行
                            UpdateBy = cardModifyCpm.UpdateBy, //修改人  当前登录人
                            OpenProvince = cardModifyCpm.OpenProvince, //省名称
                            OpenProvinceCode = cardModifyCpm.OpenProvinceCode,
                            OpenCity = cardModifyCpm.OpenCity, //市区名称
                            OpenCityCode = cardModifyCpm.OpenCityCode, 
                            IDCard = cardModifyCpm.IDCard //身份证号
                        });
                        tran.Complete();
                    }
                }
                else
                {
                    return ResultModel<object>.Conclude(FinanceCardModifyC.NoModify);
                }
            }
            else
            {
                return ResultModel<object>.Conclude(FinanceCardModifyC.BelongTypeError);
            }

            #region 2.异步调用注册ebao
            var cYeeRegisterParameter = new YeeRegisterParameter()
            { 
                AccountName = cardModifyCpm.TrueName,
                BankAccountNumber = cardModifyCpm.AccountNo,
                BankCity = cardModifyCpm.OpenCity,
                BankName = cardModifyCpm.OpenBank,
                BankProvince = cardModifyCpm.OpenProvince,
                BindMobile = cfAccount.PhoneNo, //绑定手机
                BusinessLicence = "",
                IdCard = cardModifyCpm.IDCard,
                CustomerType = (cardModifyCpm.BelongType == 0
                    ? CustomertypeEnum.PERSON
                    : CustomertypeEnum.ENTERPRISE),
                LegalPerson = cardModifyCpm.TrueName,
                LinkMan = cardModifyCpm.TrueName,
                SignedName = cardModifyCpm.TrueName
            };
            var registResult = new Register().RegSubaccount(cYeeRegisterParameter); //注册帐号
            if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            {
                _clienterFinanceAccountDao.UpdateYeepayInfoById(cardModifyCpm.Id, registResult.ledgerno, 0);
            }
            else
            {
                _clienterFinanceAccountDao.UpdateYeepayInfoById(cardModifyCpm.Id, "", 1);  //绑定失败，更新易宝key
                if (registResult == null)
                {
                    LogHelper.LogWriterString("骑士绑定易宝支付失败", string.Format("返回结果为null"));
                }
                else
                {
                    LogHelper.LogWriterString("骑士绑定易宝支付失败",
                        string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                }
            }
            #endregion


            return ResultModel<object>.Conclude(SystemState.Success);
        }

        /// <summary>
        /// 骑士修改绑定银行卡功能有效性验证  add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardModifyCpm"></param>
        /// <returns></returns>
        private FinanceCardModifyC CheckCardModifyC(CardModifyCPM cardModifyCpm)
        {
            if (cardModifyCpm == null)
            {
                return FinanceCardModifyC.NoPara;
            }
            if (cardModifyCpm.BelongType == (int)ClienterFinanceAccountBelongType.Conpany
                && string.IsNullOrWhiteSpace(cardModifyCpm.OpenSubBank)) //公司帐户开户支行不能为空
            {
                return FinanceCardModifyC.BelongTypeError;
            }

            return FinanceCardModifyC.Success;
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
            return ResultModel<object>.Conclude(SystemState.Success,
              TranslateRecords(records));
        }

        /// <summary>
        /// 骑士交易流水API 信息处理转换 add by caoheyang 20150512
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
                temp.StatusStr = ((ClienterBalanceRecordStatus)Enum.Parse(typeof(ClienterBalanceRecordStatus),
                    temp.Status.ToString(), false)).GetDisplayText(); //流水状态文本
                temp.RecordTypeStr =
                    ((ClienterBalanceRecordRecordType)Enum.Parse(typeof(ClienterBalanceRecordRecordType),
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
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ClienterWithdrawFormModel GetClienterWithdrawListById(string withwardId)
        {
            return clienterFinanceDao.GetClienterWithdrawListById(withwardId);
        }
        /// <summary>
        /// 获取骑士提款单操作日志
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            bool isClienterIDValid = _clienterDao.IsBussinessOrClienterValidByID(1, model.WithwardId);
            if (isClienterIDValid)
            {
                return clienterFinanceDao.ClienterWithdrawAudit(model);
            }
            throw new Exception("提款单对应的骑士已经被取消资格，请联系客服");
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
        /// 骑士提现申请单确认打款调用易宝接口
        /// danny-20150717
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo ClienterWithdrawPaying(ClienterWithdrawLog model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            var cliFinanceAccount = clienterFinanceDao.GetClienterFinanceAccount(model.WithwardId.ToString());
            if (cliFinanceAccount == null)
            {
                dealResultInfo.DealMsg = "获取提现单信息失败！";
                return dealResultInfo;
            }
            if (cliFinanceAccount.WithdrawTime < ParseHelper.ToDatetime(Config.WithdrawTime))
            {
               dealResultInfo.DealFlag= ClienterWithdrawPayOk(model);
               dealResultInfo.DealMsg = dealResultInfo.DealFlag ? "打款成功！" : "打款失败！";
                return dealResultInfo;
            }
            decimal amount = cliFinanceAccount.HandChargeOutlay == 0
                ? cliFinanceAccount.Amount
                : cliFinanceAccount.Amount + cliFinanceAccount.HandCharge;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //注册易宝子账户逻辑
                if (string.IsNullOrEmpty(cliFinanceAccount.YeepayKey) || cliFinanceAccount.YeepayStatus == 1)
                {
                    var brp = new YeeRegisterParameter
                    {
                        BindMobile = cliFinanceAccount.PhoneNo,
                        SignedName = cliFinanceAccount.TrueName,
                        CustomerType =
                            cliFinanceAccount.BelongType == 0
                                ? CustomertypeEnum.PERSON
                                : CustomertypeEnum.ENTERPRISE,
                        LinkMan = cliFinanceAccount.TrueName,
                        IdCard = cliFinanceAccount.IDCard,
                        BusinessLicence = cliFinanceAccount.IDCard,
                        LegalPerson = cliFinanceAccount.TrueName,
                        BankAccountNumber = cliFinanceAccount.AccountNo,
                        BankName = cliFinanceAccount.OpenBank,
                        AccountName = cliFinanceAccount.TrueName,
                        BankProvince = cliFinanceAccount.OpenProvince,
                        BankCity = cliFinanceAccount.OpenCity
                    };
                    var dr = DealRegCliSubAccount(brp);
                    if (!dr.DealFlag)
                    {
                        dealResultInfo.DealMsg = dr.DealMsg;
                        return dealResultInfo;
                    }
                    cliFinanceAccount.YeepayKey = dr.SuccessId; //子账户id
                }
                //转账逻辑
                var regTransfer = new Transfer().TransferAccounts("", amount.ToString(),
                    cliFinanceAccount.YeepayKey); //转账   子账户转给总账户
                if (regTransfer.code != "1")
                {
                    dealResultInfo.DealMsg = "骑士易宝自动转账失败：" + regTransfer.code;
                    return dealResultInfo;
                }
                var regCash = new Transfer().CashTransfer(APP.B, ParseHelper.ToInt(model.WithwardId),
                    cliFinanceAccount.YeepayKey, amount.ToString()); //提现
                if (regCash.code != "1")
                {
                    dealResultInfo.DealMsg = "骑士易宝自动提现失败：" + regCash.code;
                    return dealResultInfo;
                }
                if (!clienterFinanceDao.ClienterWithdrawPayOk(model))
                {
                    dealResultInfo.DealMsg = "更改提现单状态为打款中失败！";
                    return dealResultInfo;
                }
                dealResultInfo.DealFlag = true;
                dealResultInfo.DealMsg = "骑士提现单确认打款处理成功，等待银行打款！";
                tran.Complete();
                return dealResultInfo;
            }
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
                if (clienterFinanceDao.ClienterWithdrawReturn(model)
                    && clienterFinanceDao.ClienterWithdrawPayFailed(model)
                    && clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString())
                    && clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
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
        public bool ClienterWithdrawPayFailed(ClienterWithdrawLogModel model, CashTransferCallback callback)
        {
            bool reg = false;
            var withdraw = _clienterWithdrawFormDao.GetById(model.WithwardId);
            if (withdraw == null)
            {
                return reg;
            }
            Transfer transfer = new Transfer();
            TransferReturnModel tempmodel = transfer.TransferAccounts("",
                (ParseHelper.ToDecimal(callback.amount) - withdraw.HandCharge).ToString(), callback.ledgerno);
            if (tempmodel.code == "1") //易宝子账户到主账户打款 成功
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    if (clienterFinanceDao.ClienterWithdrawReturn(model)
                        && clienterFinanceDao.ClienterWithdrawPayFailed(model)
                        && clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString())
                        && clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
                    {
                        _clienterDao.UpdateForWithdrawC(new UpdateForWithdrawPM
                        {
                            Id = withdraw.ClienterId,
                            Money = -withdraw.HandCharge
                        }); //更新骑士表的余额，可提现余额
                        if (withdraw.HandChargeOutlay == 0) //个人支出手续费  增加手续费扣款记录流水 
                        {
                            _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                            {
                                ClienterId = withdraw.ClienterId, //骑士Id(Clienter表）
                                Amount = -withdraw.HandCharge, //流水金额
                                Status = (int)ClienterBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                                RecordType = (int)ClienterBalanceRecordRecordType.ProcedureFee,
                                Operator = "易宝系统回调",
                                WithwardId = withdraw.Id,
                                RelationNo = withdraw.WithwardNo,
                                Remark = "易宝提现失败扣除手续费"
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
        /// 获取骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ClienterBalanceRecord> GetClienterBalanceRecordList(ClienterBalanceRecordSerchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterBalanceRecordList(criteria);
        }

        /// <summary>
        /// 获取骑士提款收支记录列表分页版
        /// danny-20150604
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ClienterBalanceRecord> GetClienterBalanceRecordListOfPaging(ClienterBalanceRecordSerchCriteria criteria)
        {
            return clienterFinanceDao.GetClienterBalanceRecordListOfPaging<ClienterBalanceRecord>(criteria);
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
        /// <param name="criteria"></param>
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
            strBuilder.AppendLine("<td>卡号</td>");
            strBuilder.AppendLine("<td>骑士姓名</td>");
            strBuilder.AppendLine("<td>提款金额</td>");
            strBuilder.AppendLine("<td>申请时间</td>");
            strBuilder.AppendLine("<td>电话</td>");
            strBuilder.AppendLine("<td>开户行</td>");
            strBuilder.AppendLine("<td>账户名</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in list)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.ClienterName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount.ToString("F2")));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.WithdrawDateStart));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.ClienterPhoneNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OpenBank));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", item.TrueName));

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
                strBuilder.AppendLine(string.Format("<tr><td>'{0}</td>", item.RelationNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OpenBank));
                strBuilder.AppendLine(string.Format("<td>'{0}</td>", ParseHelper.ToDecrypt(item.AccountNo)));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Balance));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.OperateTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", item.Operator));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }

        /// <summary>
        /// 骑士余额调整
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterRecharge(ClienterOptionLog model)
        {
            return clienterFinanceDao.ClienterRecharge(model);
        }
        /// <summary>
        /// 调用商户易宝子账号注册接口并对返回值进行处理
        /// danny-20150716
        /// </summary>
        /// <param name="model"></param>
        public DealResultInfo DealRegCliSubAccount(YeeRegisterParameter model)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            var registResult = new Register().RegSubaccount(model);//注册帐号
            if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            {
                if (!_clienterFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.AccountId), registResult.ledgerno, 0))
                {
                    dealResultInfo.DealMsg = "调用易宝用户注册成功，回写数据库失败！";
                }
                else
                {
                    dealResultInfo.DealFlag = true;
                    dealResultInfo.SuccessId = registResult.ledgerno;
                    dealResultInfo.DealMsg = "骑士绑定易宝支付成功！";
                }
            }
            else
            {
                _clienterFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.AccountId), "", 1);
                if (registResult == null)
                {
                    dealResultInfo.DealMsg = "骑士绑定易宝支付失败,返回结果为null!";
                }
                else
                {
                    LogHelper.LogWriterString("骑士绑定易宝支付失败",
                        string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                    dealResultInfo.DealMsg = string.Format("商户绑定易宝支付失败,易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}", registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg);
                }
            }
            return dealResultInfo;
        }
    }
}
