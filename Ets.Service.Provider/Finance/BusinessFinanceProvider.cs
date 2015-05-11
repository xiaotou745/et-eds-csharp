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
        private readonly ClienterBalanceRecordDao _clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        /// <summary>
        /// 商户提现表
        /// </summary>
        private readonly ClienterWithdrawFormDao _clienterWithdrawFormDao = new ClienterWithdrawFormDao();
        /// <summary>
        /// 商户提现日志
        /// </summary>
        private readonly ClienterWithdrawLogDao _clienterWithdrawLogDao = new ClienterWithdrawLogDao();
        /// <summary>
        /// 商户金融账号表
        /// </summary>
        private readonly ClienterFinanceAccountDao _clienterFinanceAccountDao = new ClienterFinanceAccountDao();

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

        /// <summary>
        /// 商户提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawBpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel WithdrawB(WithdrawBPM withdrawBpm)
        {
        return null;
        }

        /// <summary>
        /// 商户绑定银行卡功能 add by caoheyang 20150511
        /// </summary>
        /// <param name="cardBindBpm">参数实体</param>
        /// <returns></returns>
        public SimpleResultModel CardBindB(CardBindBPM cardBindBpm)
        {
            if (cardBindBpm.AccountNo != cardBindBpm.AccountNo2) //两次录入的金融账号不一致
            {
                return SimpleResultModel.Conclude(FinanceCardBindC.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int count = _clienterFinanceAccountDao.GetCountByClienterId(cardBindBpm.BusinessId);
                if (count > 0)
                {
                    return SimpleResultModel.Conclude(FinanceCardBindC.Exists);//该骑士已绑定过金融账号
                }
                int result = _clienterFinanceAccountDao.Insert(new ClienterFinanceAccount()
                {
                    ClienterId = cardBindBpm.BusinessId,//商户ID
                    TrueName = cardBindBpm.TrueName, //户名
                    AccountNo = DES.Encrypt(cardBindBpm.AccountNo), //卡号(DES加密)  
                    IsEnable = true,// 是否有效(true：有效 0：无效）  新增时true 
                    AccountType = cardBindBpm.AccountType == 0
                        ? (int)ClienterFinanceAccountType.WangYin : cardBindBpm.AccountType,  //账号类型 
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
                return SimpleResultModel.Conclude(FinanceCardCardModifyC.InputValid);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                _clienterFinanceAccountDao.Update(new ClienterFinanceAccount()
                {
                    Id = cardModifyBpm.Id,
                    ClienterId = cardModifyBpm.ClienterId,//骑士ID
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
    }
}
