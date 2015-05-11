using Ets.Dao.Finance;
using Ets.Dao.User;
using ETS.Enums;
using Ets.Model.Common;
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
            return null;
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
            return businessFinanceDao.BusinessWithdrawPayOk(model);
        }
    }
}
