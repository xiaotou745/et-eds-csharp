using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Model.Common;
using ETS.Enums;
using ETS.Security;
using ETS.Transaction;
using ETS.Transaction.Common;

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
                return ResultModel<object>.Conclude(SystemState.Success);
            }
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
                return ResultModel<object>.Conclude(SystemState.Success);
            }
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
