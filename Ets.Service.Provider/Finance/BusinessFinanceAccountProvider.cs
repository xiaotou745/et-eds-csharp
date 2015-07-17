using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Business;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Pay.YeePay;
using Ets.Service.IProvider.Finance;
using Ets.Model.Common;
using ETS.Enums;
using ETS.Security;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;

namespace Ets.Service.Provider.Finance
{
    /// <summary>
    /// 商家金融账号表 
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150629</UpdateTime>
    /// </summary>
    public class BusinessFinanceAccountProvider : IBusinessFinanceAccountProvider
    {
        private readonly BusinessFinanceAccountDao _businessFinanceAccountDao = new BusinessFinanceAccountDao();
        private readonly BusinessFinanceDao _businessFinanceDao = new BusinessFinanceDao();
        private readonly BusinessDao _businessDao = new BusinessDao();
        /// <summary>
        /// 获取金额账号Id
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetBFinanceAccountId(int businessId)
        {
            return _businessFinanceAccountDao.GetBFinanceAccountId(businessId);
        }

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> CardBindB(CardBindBPM cardBindBpm)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                FinanceCardBindB checkbool = CheckCardBindB(cardBindBpm); //验证数据合法性
                if (checkbool != FinanceCardBindB.Success)
                {
                    return ResultModel<object>.Conclude(checkbool);
                }
                int result = _businessFinanceAccountDao.Insert(new BusinessFinanceAccount()
                {
                    BusinessId = cardBindBpm.BusinessId, //商户ID
                    TrueName = cardBindBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindBpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true, // 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindBpm.AccountType == 0
                        ? (int)BusinessFinanceAccountType.WangYin
                        : cardBindBpm.AccountType, //账号类型 
                    BelongType = cardBindBpm.BelongType, //账号类别  0 个人账户 1 公司账户  
                    OpenBank = cardBindBpm.OpenBank, //开户行
                    OpenSubBank = cardBindBpm.OpenSubBank, //开户支行
                    CreateBy = cardBindBpm.CreateBy, //创建人  当前登录人
                    UpdateBy = cardBindBpm.CreateBy, //新增时最后修改人与新增人一致  当前登录人
                    OpenCity = cardBindBpm.OpenCity, //开户行
                    OpenProvince = cardBindBpm.OpenProvince, //开户市
                    IDCard = cardBindBpm.IDCard, //营业执照
                });
                tran.Complete();
            }
            #region 异步请求易宝注册接口
            Task.Factory.StartNew(() =>
            {
                //请求易宝注册接口,如果成功,则更新账户易宝key和status
                var business = _businessDao.GetById(cardBindBpm.BusinessId);
                if (business == null)
                {
                    ETS.Util.LogHelper.LogWriter(new ArgumentException("business值为null"), "BusinessFinanceAccountProvider.CardBindB-绑定银行账户");
                    return;
                }
                string requestid = TimeHelper.GetTimeStamp(false);
                string bindmobile = business.PhoneNo;  //绑定手机
                string customertype = (cardBindBpm.BelongType == 0 ?
                    CustomertypeEnum.PERSON.ToString() : CustomertypeEnum.ENTERPRISE.ToString()); //注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业
                string signedname = cardBindBpm.TrueName; //签约名   商户签约名；个人，填写姓名；企业，填写企业名称。
                string linkman = cardBindBpm.TrueName; //联系人
                string idcard = cardBindBpm.BelongType == 0 ? cardBindBpm.IDCard : ""; //身份证  customertype为PERSON时，必填
                string businesslicence = cardBindBpm.BelongType == 0 ? "" : cardBindBpm.IDCard; //营业执照号 customertype为ENTERPRISE时，必填
                string legalperson = cardBindBpm.TrueName;
                string bankaccountnumber = cardBindBpm.AccountNo; //银行卡号 
                string bankname = cardBindBpm.OpenBank; //开户行
                string accountname = cardBindBpm.TrueName; //开户名
                string bankaccounttype = (cardBindBpm.BelongType == 0 ?
                    BankaccounttypeEnum.PrivateCash.ToString() : BankaccounttypeEnum.PublicCash.ToString());  //银行卡类别  PrivateCash：对私 PublicCash： 对公
                string bankprovince = cardBindBpm.OpenProvince;
                string bankcity = cardBindBpm.OpenCity;
                var result = new Register().RegSubaccount(requestid, bindmobile, customertype, signedname, linkman,
                idcard, businesslicence, legalperson, bankaccountnumber, bankname,
                accountname, bankaccounttype, bankprovince, bankcity);//注册帐号
                if (result != null && !string.IsNullOrEmpty(result.code) && result.code.Trim() == "1")
                {
                    _businessFinanceAccountDao.UpdateYeepayInfo(cardBindBpm.BusinessId, result.ledgerno, 0);
                }
                else
                {
                    if (result == null)
                    {
                        ETS.Util.LogHelper.LogWriterString("商户绑定易宝支付失败", string.Format("返回结果为null"));
                    }
                    else
                    {
                        ETS.Util.LogHelper.LogWriterString("商户绑定易宝支付失败", string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            result.code, result.ledgerno, result.hmac, result.msg));
                    }
                }
            });
            #endregion
            return ResultModel<object>.Conclude(SystemState.Success);
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
            //TODO 验证该 商户id 下 是存在未完成的 提现申请单 ，如果存在不允许修改 
            int withdrawCount = _businessFinanceDao.GetBusinessWithdrawByBusinessId(cardModifyBpm.BusinessId);
            if (withdrawCount > 0)
            {
                return ResultModel<object>.Conclude(FinanceCardModifyB.ForbitModify); 
            }
            BusinessFinanceAccount bfAccount = _businessFinanceAccountDao.GetById(cardModifyBpm.Id);
            if (bfAccount != null)
            {
                if (bfAccount.OpenBank != cardModifyBpm.OpenBank || bfAccount.OpenSubBank != cardModifyBpm.OpenSubBank ||
                    bfAccount.OpenProvince != cardModifyBpm.OpenCity || bfAccount.IDCard != cardModifyBpm.IDCard ||
                    bfAccount.TrueName != cardModifyBpm.TrueName ||
                    bfAccount.AccountNo != DES.Encrypt(cardModifyBpm.AccountNo))
                {
                    using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                    {
                        _businessFinanceAccountDao.Update(new BusinessFinanceAccount()
                        {
                            Id = cardModifyBpm.Id,
                            BusinessId = cardModifyBpm.BusinessId, //商户ID
                            TrueName = cardModifyBpm.TrueName, //户名
                            AccountNo = DES.Encrypt(cardModifyBpm.AccountNo), //卡号(DES加密) 
                            BelongType = cardModifyBpm.BelongType, //账号类别  0 个人账户 1 公司账户  
                            OpenBank = cardModifyBpm.OpenBank, //开户行
                            OpenSubBank = cardModifyBpm.OpenSubBank, //开户支行
                            UpdateBy = cardModifyBpm.UpdateBy, //修改人  当前登录人
                            OpenProvince = cardModifyBpm.OpenProvince, //省名称
                            OpenCity = cardModifyBpm.OpenCity, //市区名称
                            IDCard = cardModifyBpm.IDCard //公司账户时存营业执照，个人账户存身份证号码
                        });
                        tran.Complete();
                    }
                }
            }
            else
            {
                return ResultModel<object>.Conclude(FinanceCardModifyB.BelongTypeError);
            }

            #region 2.异步调用注册ebao

            string requestid = TimeHelper.GetTimeStamp(false);
            string bindmobile = bfAccount.PhoneNo; //绑定手机
            string customertype = (cardModifyBpm.BelongType == 0 ? CustomertypeEnum.PERSON.ToString() : CustomertypeEnum.ENTERPRISE.ToString()); //注册类型PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业
            string signedname = cardModifyBpm.TrueName; //签约名   商户签约名；个人，填写姓名；企业，填写企业名称。
            string linkman = cardModifyBpm.TrueName; //联系人
            string idcard = cardModifyBpm.IDCard; //身份证  customertype为PERSON时，必填 
            string businesslicence = cardModifyBpm.BelongType == 0 ? "" : cardModifyBpm.IDCard; //营业执照号 customertype为ENTERPRISE时，必填
            string legalperson = cardModifyBpm.TrueName;
            string bankaccountnumber = cardModifyBpm.AccountNo; //银行卡号 
            string bankname = cardModifyBpm.OpenBank; //开户行
            string accountname = cardModifyBpm.TrueName; //开户名
            string bankaccounttype = (cardModifyBpm.BelongType == 0 ? BankaccounttypeEnum.PrivateCash.ToString() : BankaccounttypeEnum.PublicCash.ToString()); //银行卡类别  PrivateCash：对私 PublicCash： 对公
            string bankprovince = cardModifyBpm.OpenProvince;
            string bankcity = cardModifyBpm.OpenCity;
            var registResult = new Register().RegSubaccount(requestid, bindmobile, customertype, signedname, linkman,
                idcard, businesslicence, legalperson, bankaccountnumber, bankname,
                accountname, bankaccounttype, bankprovince, bankcity); //注册帐号
            if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            {
                _businessFinanceAccountDao.UpdateYeepayInfoById(cardModifyBpm.Id, registResult.ledgerno, 0);
            }
            else
            {
                _businessFinanceAccountDao.UpdateYeepayInfoById(cardModifyBpm.Id, "", 1);  //绑定失败，更新易宝key
                if (registResult == null)
                {
                    LogHelper.LogWriterString("商户绑定易宝支付失败", string.Format("返回结果为null"));
                }
                else
                {
                    LogHelper.LogWriterString("商户绑定易宝支付失败",
                        string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                            registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                }
            }
            #endregion

            return ResultModel<object>.Conclude(SystemState.Success);
        }


        #region 用户自定义方法
        /// <summary>
        ///  商户绑定银行卡功能有效性验证 add by caoheyang 20150511 
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        private FinanceCardBindB CheckCardBindB(CardBindBPM cardBindBpm)
        {
            if (cardBindBpm == null)
            {
                return FinanceCardBindB.NoPara;
            }
            if (cardBindBpm.BelongType == (int)BusinessFinanceAccountBelongType.Conpany
               && string.IsNullOrWhiteSpace(cardBindBpm.OpenSubBank)) //公司帐户开户支行不能为空
            {
                return FinanceCardBindB.BelongTypeError;
            }
            int count = _businessFinanceAccountDao.GetCountByBusinessId(cardBindBpm.BusinessId);
            if (count > 0) //该商户已绑定过金融账号
            {
                return FinanceCardBindB.Exists;
            }
            return FinanceCardBindB.Success;
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
    }
}
