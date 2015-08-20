using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Business;
using Ets.Dao.Finance;
using Ets.Model.Common.YeePay;
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
            #region 参数验证
            //绑定个人账户
            if (cardBindBpm.BelongType == 0 && (string.IsNullOrEmpty(cardBindBpm.IDCard) || cardBindBpm.IDCard.Length < 18))
            {
                return ResultModel<object>.Conclude(FinanceCardBindB.IDCardError);
            }
            //绑定公司账户
            if (cardBindBpm.BelongType == 1 && (string.IsNullOrEmpty(cardBindBpm.IDCard) || cardBindBpm.IDCard.Length < 15))
            {
                return ResultModel<object>.Conclude(FinanceCardBindB.BusinessLicenceError);
            }
            FinanceCardBindB checkbool = CheckCardBindB(cardBindBpm); //验证数据合法性
            if (checkbool != FinanceCardBindB.Success)
            {
                return ResultModel<object>.Conclude(checkbool);
            }

            //这里其实是多查了一次数据库，主要是为了CreateBy.
            //由于上线当天版本已经稳定，所以没让APP去改版.
            //下次再次修改本接口时需要让APP把店铺名称传过来参数为CreateBy
            Ets.Model.DataModel.Business.BusListResultModel businessModel = _businessDao.GetBusiness(cardBindBpm.BusinessId);

            #endregion
            var id = 0;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                id = _businessFinanceAccountDao.Insert(new BusinessFinanceAccount()
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
                    CreateBy = businessModel.PhoneNo, //创建人  当前登录人
                    UpdateBy = businessModel.PhoneNo, //cardBindBpm.CreateBy 新增时最后修改人与新增人一致  当前登录人
                    OpenCity = cardBindBpm.OpenCity, //开户行
                    OpenProvince = cardBindBpm.OpenProvince, //开户市
                    IDCard = cardBindBpm.IDCard ?? "", //身份证
                    OpenProvinceCode = cardBindBpm.OpenProvinceCode,//省编码
                    OpenCityCode = cardBindBpm.OpenCityCode,//市编码
                });
                tran.Complete();
            }
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
                            YeepayStatus = 1,
                            OpenProvince = cardModifyBpm.OpenProvince, //省名称
                            OpenProvinceCode = cardModifyBpm.OpenProvinceCode,
                            OpenCity = cardModifyBpm.OpenCity, //市区名称
                            OpenCityCode = cardModifyBpm.OpenCityCode,
                            IDCard = cardModifyBpm.IDCard //公司账户时存营业执照，个人账户存身份证号码
                        });
                        tran.Complete();
                    }
                }
                else
                {
                    return ResultModel<object>.Conclude(FinanceCardModifyB.NoModify);
                }
            }
            else
            {
                return ResultModel<object>.Conclude(FinanceCardModifyB.BelongTypeError);
            }

            #region 2.异步调用注册ebao
            //var bYeeRegisterParameter = new YeeRegisterParameter()
            //{
            //    AccountName = cardModifyBpm.TrueName,
            //    BankAccountNumber = cardModifyBpm.AccountNo,
            //    BankCity = cardModifyBpm.OpenCity,
            //    BankName = cardModifyBpm.OpenBank,
            //    BankProvince = cardModifyBpm.OpenProvince,
            //    BindMobile = bfAccount.PhoneNo, //绑定手机
            //    BusinessLicence = cardModifyBpm.BelongType == 0 ? "" : cardModifyBpm.IDCard,
            //    IdCard = cardModifyBpm.IDCard,
            //    CustomerType = (cardModifyBpm.BelongType == 0
            //        ? CustomertypeEnum.PERSON
            //        : CustomertypeEnum.ENTERPRISE),
            //    LegalPerson = cardModifyBpm.TrueName,
            //    LinkMan = cardModifyBpm.TrueName,
            //    SignedName = cardModifyBpm.TrueName
            //};
            //var registResult = new Register().RegSubaccount(bYeeRegisterParameter); //注册帐号
            //if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1")   //绑定成功，更新易宝key
            //{
            //    _businessFinanceAccountDao.UpdateYeepayInfoById(cardModifyBpm.Id, registResult.ledgerno, 0);
            //}
            //else
            //{
            //    _businessFinanceAccountDao.UpdateYeepayInfoById(cardModifyBpm.Id, "", 1);  //绑定失败，更新易宝key
            //    if (registResult == null)
            //    {
            //        LogHelper.LogWriterString("商户绑定易宝支付失败", string.Format("返回结果为null"));
            //    }
            //    else
            //    {
            //        LogHelper.LogWriterString("商户绑定易宝支付失败",
            //            string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
            //                registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
            //    }
            //}
            #endregion

            return ResultModel<object>.Conclude(SystemState.Success);
        }

        /// <summary>
        /// 获取该商户的金融账户信息
        /// wc
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusinessFinanceAccount GetFinanceAccountByBusiId(int busiId)
        {
            return _businessFinanceAccountDao.GetFinanceAccountByBusiId(busiId);
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

            if (cardModifyBpm.BelongType == (int)BusinessFinanceAccountBelongType.Conpany)
            {
                if (string.IsNullOrWhiteSpace(cardModifyBpm.IDCard) || cardModifyBpm.IDCard.Trim().Length < 15)
                {
                    return FinanceCardModifyB.BusinessLicenceError;
                }
            }
            if (cardModifyBpm.BelongType == (int)BusinessFinanceAccountBelongType.Self)
            {
                if (string.IsNullOrWhiteSpace(cardModifyBpm.IDCard) || cardModifyBpm.IDCard.Trim().Length < 18)
                {
                    return FinanceCardModifyB.IDCardError;
                }
            }

            return FinanceCardModifyB.Success;
        }
        #endregion
    }
}
