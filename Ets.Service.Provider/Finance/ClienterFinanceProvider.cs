﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ETS.Const;
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
using ETS.Pay.AliPay;
using ETS.Pay.YeePay;
using ETS.Security;
using Ets.Service.IProvider.Finance;
using Ets.Service.IProvider.Pay;
using Ets.Service.Provider.Pay;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using ETS.Data.PageData;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Dao.Message;
using Ets.Model.DataModel.Message;
using ETS.Const;
using Config = ETS.Config;

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

        readonly ClienterAllowWithdrawRecordDao clienterAllowWithdrawRecordDao = new ClienterAllowWithdrawRecordDao();
        /// <summary>
        /// 骑士提现日志
        /// </summary>
        private readonly ClienterWithdrawLogDao _clienterWithdrawLogDao = new ClienterWithdrawLogDao();
        /// <summary>
        /// 骑士金融账号表
        /// </summary>
        private readonly ClienterFinanceAccountDao _clienterFinanceAccountDao = new ClienterFinanceAccountDao();
        ClienterFinanceDao clienterFinanceDao = new ClienterFinanceDao();

        private IClienterProvider iClienterProvider = new ClienterProvider();
        ClienterMessageDao clienterMessageDao = new ClienterMessageDao();
        #endregion

        #region 骑士提现功能  add by caoheyang 20150509

        /// <summary>
        /// 骑士提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> WithdrawC(WithdrawCriteria model)
        {
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                var clienter = new clienter();
                var clienterFinanceAccount = new ClienterFinanceAccount();//骑士金融账号信息
                var checkbool = CheckWithdrawC(model, ref clienter, ref clienterFinanceAccount);
                if (checkbool != FinanceWithdrawC.Success)  //验证失败 此次提款操作无效 直接返回相关错误信息
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                var withwardNo = Helper.generateOrderCode(model.ClienterId);
                var globalConfig = GlobalConfigDao.GlobalConfigGet(0);
                //金融机构实扣手续费
                var handCharge = clienterFinanceAccount.AccountType == ClienterFinanceAccountType.WangYin.GetHashCode() ? Convert.ToDecimal(globalConfig.YeepayWithdrawCommission) : (clienterFinanceAccount.AccountType == ClienterFinanceAccountType.ZhiFuBao.GetHashCode() ? Convert.ToDecimal(globalConfig.AlipayWithdrawCommission) : 0);
                //实付金额配算除了易宝 给配加一个真实手续费,其他都是0  暂时
                var peiMoney = clienterFinanceAccount.AccountType == ClienterFinanceAccountType.WangYin.GetHashCode()?Convert.ToDecimal(globalConfig.YeepayWithdrawCommission):0;
                var withwardId = _clienterWithdrawFormDao.Insert(new ClienterWithdrawForm()
                {
                    WithwardNo = withwardNo,//单号 规则待定
                    ClienterId = model.ClienterId,//骑士Id(Clienter表）
                    BalancePrice = clienter.AccountBalance,//提现前骑士余额
                    AllowWithdrawPrice = clienter.AllowWithdrawPrice,//提现前骑士可提现金额
                    Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                    Amount = model.WithdrawPrice,//提现金额
                    Balance = clienter.AccountBalance - model.WithdrawPrice, //提现后余额
                    TrueName = clienterFinanceAccount.TrueName,//骑士收款户名
                    AccountNo = clienterFinanceAccount.AccountNo, //卡号(DES加密)
                    AccountType = clienterFinanceAccount.AccountType, //账号类型：
                    BelongType = clienterFinanceAccount.BelongType,//账号类别  0 个人账户 1 公司账户  
                    OpenBank = clienterFinanceAccount.OpenBank,//开户行
                    OpenSubBank = clienterFinanceAccount.OpenSubBank, //开户支行
                    IDCard = clienterFinanceAccount.IDCard,//申请提款身份证号
                    OpenCity = clienterFinanceAccount.OpenCity,//城市
                    OpenCityCode = clienterFinanceAccount.OpenCityCode,//城市代码
                    OpenProvince = clienterFinanceAccount.OpenProvince,//省份
                    OpenProvinceCode = clienterFinanceAccount.OpenProvinceCode,//省份代码
                    //HandCharge = Convert.ToInt32(globalConfig.WithdrawCommission),//手续费
                    HandCharge = handCharge,//手续费
                    PaidAmount = model.WithdrawPrice - Convert.ToDecimal(globalConfig.WithdrawCommission) + peiMoney,//实付金额=提现金额-手续费(3元)+配算金额(易宝1元其他0元)
                    //HandChargeOutlay = model.WithdrawPrice > Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney) ? HandChargeOutlay.EDaiSong : HandChargeOutlay.Private,//手续费支出方
                    HandChargeOutlay = HandChargeOutlay.Private,//手续费支出方（新版需求改为手续费统一由骑士支付）
                    PhoneNo = clienter.PhoneNo, //手机号 //PhoneNo = clienterFinanceAccount.CreateBy, //手机号
                    HandChargeThreshold = 0//手续费阈值（新版需求改为不设阀值）
                    //HandChargeThreshold = Convert.ToInt32(globalConfig.ClienterWithdrawCommissionAccordingMoney)//手续费阈值
                });

                _clienterWithdrawLogDao.Insert(new ClienterWithdrawLog()
                {
                    WithwardId = withwardId,
                    Status = (int)ClienterWithdrawFormStatus.WaitAllow,//待审核
                    Remark = "骑士发起提现操作",
                    Operator = clienter.TrueName
                });


                //更新骑士余额、可提现余额（实际到账金额）  
                iClienterProvider.UpdateCBalanceAndWithdraw(new ClienterMoneyPM()
                {
                    ClienterId = model.ClienterId,//骑士Id(Clienter表）
                    Amount = -(model.WithdrawPrice - Convert.ToDecimal(globalConfig.WithdrawCommission)),//流水金额(实际到账金额)
                    Status = ClienterAllowWithdrawRecordStatus.Tradeing.GetHashCode(), //流水状态(1、交易成功 2、交易中）
                    RecordType = ClienterAllowWithdrawRecordType.WithdrawApply.GetHashCode(),
                    Operator = clienter.TrueName,
                    WithwardId = withwardId,
                    RelationNo = withwardNo,
                    Remark = "提现扣除余额"
                });
                //更新骑士余额、可提现余额(手续费)  
                iClienterProvider.UpdateCBalanceAndWithdraw(new ClienterMoneyPM()
                {
                    ClienterId = model.ClienterId,//骑士Id(Clienter表）
                    Amount = -Convert.ToDecimal(globalConfig.WithdrawCommission),//流水金额（手续费）
                    Status = ClienterAllowWithdrawRecordStatus.Tradeing.GetHashCode(), //流水状态(1、交易成功 2、交易中）
                    RecordType = ClienterAllowWithdrawRecordType.ProcedureFee.GetHashCode(),
                    Operator = clienter.TrueName,
                    WithwardId = withwardId,
                    RelationNo = withwardNo,
                    Remark = "提现手续费"
                });

                tran.Complete();
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
        private FinanceWithdrawC CheckWithdrawC(WithdrawCriteria withdrawCpm, ref clienter clienter,
            ref  ClienterFinanceAccount clienterFinanceAccount)
        {
            if (withdrawCpm == null)
            {
                return FinanceWithdrawC.NoPara;
            }
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
            //如果是支付宝,不走下面的判断,因为支付宝没有身份证和银行信息 add by pengyi 20150914
            if (clienterFinanceAccount.AccountType!= 2 && (string.IsNullOrEmpty(clienterFinanceAccount.IDCard) || !Regex.IsMatch(clienterFinanceAccount.IDCard, Config.IDCARD_REG, RegexOptions.IgnoreCase) ||
                string.IsNullOrEmpty(clienterFinanceAccount.OpenCity) || string.IsNullOrEmpty(clienterFinanceAccount.OpenProvince) ||
                string.IsNullOrEmpty(clienterFinanceAccount.OpenBank) || string.IsNullOrEmpty(clienterFinanceAccount.OpenSubBank) ||
                string.IsNullOrEmpty(clienterFinanceAccount.OpenSubBank) || !Regex.IsMatch(clienterFinanceAccount.OpenSubBank, Config.OPEN_SUB_BANK_REG))
                )
            {
                return FinanceWithdrawC.BankInfoError;
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
            var accountType = cardBindCpm.AccountType == 0
                ? (int)ClienterFinanceAccountType.WangYin
                : cardBindCpm.AccountType;
            cardBindCpm.AccountType = accountType;
            FinanceCardBindC checkbool = CheckCardBindC(cardBindCpm);  //验证数据合法性
            if (checkbool != FinanceCardBindC.Success)
            {
                return ResultModel<object>.Conclude(checkbool);
            }
            //验证绑定的银行卡用户信息 和 该骑士用户姓名、身份证 是否一致
            clienter c = _clienterDao.GetById(cardBindCpm.ClienterId);
            if (c == null)
            {
                return ResultModel<object>.Conclude(FinanceCardBindC.NoClienter);
            }
            else
            {
                if (cardBindCpm.TrueName.Trim() != c.TrueName.Trim())
                {
                    return ResultModel<object>.Conclude(FinanceCardBindC.TrueNameNoMatch);
                }
                if (cardBindCpm.IDCard.Trim() != c.IDCard.Trim())
                {
                    return ResultModel<object>.Conclude(FinanceCardBindC.IDCardNoMatch);
                }
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
                    AccountType = accountType,  //账号类型 
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
            return ResultModel<object>.Conclude(FinanceCardBindC.Success, result);
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
            int count = _clienterFinanceAccountDao.GetCountByClienterId(cardBindCpm.ClienterId, cardBindCpm.AccountType);
            if (count > 0) //该骑士已绑定过指定账户类型的金融账号
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
            //验证绑定的银行卡用户信息 和 该骑士用户姓名、身份证 是否一致
            clienter c = _clienterDao.GetById(cardModifyCpm.ClienterId);
            if (c == null)
            {
                return ResultModel<object>.Conclude(FinanceCardBindC.NoClienter);
            }
            else
            {
                if (cardModifyCpm.TrueName.Trim() != c.TrueName.Trim())
                {
                    return ResultModel<object>.Conclude(FinanceCardBindC.TrueNameNoMatch);
                }
                if (cardModifyCpm.IDCard.Trim() != c.IDCard.Trim())
                {
                    return ResultModel<object>.Conclude(FinanceCardBindC.IDCardNoMatch);
                }
            }
            int withdrawCount = _clienterWithdrawFormDao.GetByClienterId(cardModifyCpm.ClienterId);
            if (withdrawCount > 0) //该骑士是否存在未完成的提现单
            {
                return ResultModel<object>.Conclude(FinanceCardModifyC.ForbitModify);
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
                            YeepayStatus = 1,//0正常1失败
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
            //var cYeeRegisterParameter = new YeeRegisterParameter()
            //{
            //    AccountName = cardModifyCpm.TrueName,
            //    BankAccountNumber = cardModifyCpm.AccountNo,
            //    BankCity = cardModifyCpm.OpenCity,
            //    BankName = cardModifyCpm.OpenBank,
            //    BankProvince = cardModifyCpm.OpenProvince,
            //    BindMobile = cfAccount.PhoneNo, //绑定手机
            //    BusinessLicence = "",
            //    IdCard = cardModifyCpm.IDCard,
            //    CustomerType = (cardModifyCpm.BelongType == 0
            //        ? CustomertypeEnum.PERSON
            //        : CustomertypeEnum.ENTERPRISE),
            //    LegalPerson = cardModifyCpm.TrueName,
            //    LinkMan = cardModifyCpm.TrueName,
            //    SignedName = cardModifyCpm.TrueName
            //};
            //var registResult = new Register().RegSubaccount(cYeeRegisterParameter); //注册帐号
            //if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            //{
            //    _clienterFinanceAccountDao.UpdateYeepayInfoById(cardModifyCpm.Id, registResult.ledgerno, 0);
            //}
            //else
            //{
            //    _clienterFinanceAccountDao.UpdateYeepayInfoById(cardModifyCpm.Id, "", 1);  //绑定失败，更新易宝key
            //    if (registResult == null)
            //    {
            //        LogHelper.LogWriterString("骑士绑定易宝支付失败", string.Format("返回结果为null"));
            //    }
            //    else
            //    {
            //        LogHelper.LogWriterString("骑士绑定易宝支付失败",
            //            string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
            //                registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
            //    }
            //}
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
        /// 确认打款时间锁
        /// 2015年8月1日 21:44:12
        /// 窦海超 
        /// </summary>
        private static object mylock = new object();

        /// <summary>
        /// 骑士提现申请单确认打款调用易宝接口
        /// danny-20150717
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DealResultInfo ClienterWithdrawPaying(ClienterWithdrawLog model)
        {
            #region 时间锁
            lock (mylock)
            {
                string key = string.Format(RedissCacheKey.Ets_Withdraw_Lock_C, model.WithwardId);
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                if (redis.Get<int>(key) == 1)
                {
                    return new DealResultInfo
                    {
                        DealMsg = "确认打款正在执行中，请勿重新提交，请一分钟后重试",
                        DealFlag = false
                    };
                }
                redis.Set(key, 1, new TimeSpan(0, 1, 0));
            }
            #endregion

            #region 对象声明及初始化
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
            model.IsCallBack = 0;
            //历史单据走之前逻辑
            if ((cliFinanceAccount.WithdrawTime < ParseHelper.ToDatetime(Config.WithdrawTime)) || (cliFinanceAccount.WithdrawStatus == ClienterWithdrawFormStatus.Paying.GetHashCode()))
            {
                model.Status = ClienterWithdrawFormStatus.Success.GetHashCode();
                model.OldStatus = cliFinanceAccount.WithdrawStatus == ClienterWithdrawFormStatus.Paying.GetHashCode() ? ClienterWithdrawFormStatus.Paying.GetHashCode() : ClienterWithdrawFormStatus.Allow.GetHashCode();
                dealResultInfo.DealFlag = ClienterWithdrawPayOk(model);
                dealResultInfo.DealMsg = dealResultInfo.DealFlag ? "打款成功！" : "打款失败！";
                return dealResultInfo;
            }
            #endregion
            //支付宝打款
            if (cliFinanceAccount.AccountType == 2) //该提现单是支付宝
            {
                ////调用支付宝打款
                //var alipaymodel = new PayProvider().AlipayTransfer(new AlipayTransferParameter()
                //{
                //    Partner = "2088911703660069",//2088911703660069
                //    InputCharset = "GBK",
                //    NotifyUrl = "http://pay153.yitaoyun.net:8011",
                //    Email = "info@edaisong.com",
                //    AccountName = "宋桥",
                //    PayDate = "20150914",
                //    BatchNo = "2010080100000211",
                //    BatchFee = "20",
                //    BatchNum = "1",
                //    DetailData = "10000001^dou631@163.com^白玉^1^测试转账"
                //});
            }
            #region 回写数据库返回结果对象
            if (!clienterFinanceDao.ClienterWithdrawPayOk(model))
            {
                dealResultInfo.DealMsg = "更改提现单状态为打款中失败！";
                return dealResultInfo;
            }

            //生成消息
            AddCConfirmPlayMoneyMessage(cliFinanceAccount);

            dealResultInfo.DealFlag = true;
            dealResultInfo.DealMsg = "骑士提现单确认打款处理成功，等待银行打款！";
            return dealResultInfo;
            #endregion
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
                if (clienterFinanceDao.ClienterWithdrawReturn(model) && clienterFinanceDao.ClienterClienterAllowWithdrawRecordReturn(model))
                {
                    if (clienterFinanceDao.ClienterWithdrawAuditRefuse(model))
                    {
                        if (clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString()))
                        {
                            if (clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
                            {
                                //生成消息
                                var cliFinanceAccount = clienterFinanceDao.GetClienterFinanceAccount(model.WithwardId.ToString());
                                cliFinanceAccount.AuditFailedReason = model.AuditFailedReason;
                                AddCAuditRejectionMessage(cliFinanceAccount);

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
                if (clienterFinanceDao.ClienterWithdrawReturn(model) && clienterFinanceDao.ClienterClienterAllowWithdrawRecordReturn(model)
                    && clienterFinanceDao.ClienterWithdrawPayFailed(model)
                    && clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString())
                    && clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
                {

                    ClienterFinanceAccountModel clienterFinanceAccountModel = clienterFinanceDao.GetClienterFinanceAccount(model.WithwardId.ToString());
                    int month = clienterFinanceAccountModel.WithdrawTime.Month;
                    int day = clienterFinanceAccountModel.WithdrawTime.Day;

                    long id = clienterMessageDao.Insert(new ClienterMessage
                    {
                        ClienterId = Convert.ToInt32(clienterFinanceAccountModel.ClienterId),
                        Content = string.Format(MessageConst.PlayMoneyFailure, month, day, model.PayFailedReason),
                        IsRead = 0
                    });

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
        /// <returns></returns>
        public bool ClienterWithdrawPayFailedForCallBack(ClienterWithdrawLogModel model)
        {
            bool reg = false;
            var withdraw = _clienterWithdrawFormDao.GetById(model.WithwardId);
            model.OldStatus = ClienterWithdrawFormStatus.Paying.GetHashCode();
            if (withdraw == null)
            {
                return reg;
            }

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.ClienterWithdrawReturn(model) && clienterFinanceDao.ClienterClienterAllowWithdrawRecordReturn(model)
                    && clienterFinanceDao.ClienterWithdrawPayFailed(model)
                    && clienterFinanceDao.ModifyClienterBalanceRecordStatus(model.WithwardId.ToString())
                    && clienterFinanceDao.ModifyClienterAmountInfo(model.WithwardId.ToString()))
                {
                    _clienterDao.UpdateCBalanceAndWithdraw(new UpdateForWithdrawPM
                    {
                        Id = withdraw.ClienterId,
                        Money = -withdraw.HandCharge
                    }); //更新骑士表的余额，可提现余额
                    if (withdraw.HandChargeOutlay == 0) //打款失败返回余额增加流水记录
                    {
                        _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                        {
                            ClienterId = withdraw.ClienterId, //骑士Id(Clienter表）
                            Amount = withdraw.Amount, //流水金额
                            Status = (int)ClienterBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                            RecordType = (int)ClienterBalanceRecordRecordType.PayFailure,
                            Operator = "易宝系统回调",
                            WithwardId = withdraw.Id,
                            RelationNo = withdraw.WithwardNo,
                            Remark = "打款失败"
                        });
                    }
                    reg = true;
                    tran.Complete();
                }
            }
            //}
            //else
            //{
            //    LogHelper.LogWriterString("易宝子账户到主账户打款导致失败，提现单号为:" + model.WithwardId);
            //}
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
            decimal amount = _clienterDao.GetUserStatus(model.ClienterId).amount;
            decimal allowWithdrawPrice = _clienterDao.GetUserStatus(model.ClienterId).AllowWithdrawPrice;

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //更新骑士余额、可提现余额 
                iClienterProvider.UpdateCBalanceAndWithdraw(new ClienterMoneyPM()
                                                            {
                                                                ClienterId = model.ClienterId,
                                                                Amount = model.RechargeAmount,
                                                                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                                                                RecordType = ClienterBalanceRecordRecordType.BalanceAdjustment.GetHashCode(),
                                                                Operator = model.OptName,
                                                                WithwardId = 0,
                                                                RelationNo = "",
                                                                Remark = model.Remark
                                                            });

                tran.Complete();
            }

            return true;
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
            var registResult = new PayProvider().RegisterYee(model);//注册帐号
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

        #region 用户自定义方法

        /// <summary>
        /// 新增确认打款消息
        /// 胡灵波
        /// 2015年8月28日 11:53:31
        /// </summary>
        /// <param name="clienterFinanceAccountModel"></param>
        void AddCConfirmPlayMoneyMessage(ClienterFinanceAccountModel clienterFinanceAccountModel)
        {
            int month = clienterFinanceAccountModel.WithdrawTime.Month;
            int day = clienterFinanceAccountModel.WithdrawTime.Day;

            long id = clienterMessageDao.Insert(new ClienterMessage
            {
                ClienterId = Convert.ToInt32(clienterFinanceAccountModel.ClienterId),
                Content = string.Format(MessageConst.ConfirmPlayMoney, month, day, clienterFinanceAccountModel.Amount.ToString("0.00")),
                IsRead = 0
            });
        }

        /// <summary>
        /// 新增审核拒绝消息
        /// 胡灵波
        /// 2015年8月28日 12:05:47
        /// </summary>
        /// <param name="clienterFinanceAccountModel"></param>
        void AddCAuditRejectionMessage(ClienterFinanceAccountModel clienterFinanceAccountModel)
        {
            int month = clienterFinanceAccountModel.WithdrawTime.Month;
            int day = clienterFinanceAccountModel.WithdrawTime.Day;

            long id = clienterMessageDao.Insert(new ClienterMessage
            {
                ClienterId = Convert.ToInt32(clienterFinanceAccountModel.ClienterId),
                Content = string.Format(MessageConst.AuditRejection, month, day, clienterFinanceAccountModel.AuditFailedReason),
                IsRead = 0
            });
        }
        #endregion
    }
}
