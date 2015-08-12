using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;

namespace Ets.Service.Provider.Finance
{ 
    /// <summary>
    /// 备用金操作记录  add by caoheyang 20150812
    /// </summary>
    public class ImprestBalanceRecordProvider : IImprestBalanceRecordProvider
    {

 		private readonly ImprestBalanceRecordDao _imprestBalanceRecordDao = new ImprestBalanceRecordDao();

        /// <summary>
        /// 验证手机号是否存在
        /// 2015年8月12日17:53:24
        /// 茹化肖
        /// </summary>
        /// <param name="phonenum"></param>
        /// <returns></returns>
        public ImprestClienterModel ClienterPhoneCheck(string phonenum)
        {
            ImprestClienterModel mode=new ImprestClienterModel();
            //获取骑士信息
            var clienter = new ClienterDao().GetUserInfoByUserPhoneNo(phonenum);
            if (clienter == null)
            {
                 mode.Status = 0;
                 return mode;
            }
            //获取骑士提现中金额
            var amount = new ClienterFinanceDao().GetClienterWithdrawingAmount(clienter.Id);
            //获取备用金可用余额
            var imprestPrice = new ImprestRechargeDao().GetRemainingAmountNoLock();
            mode.Id = clienter.Id;
            mode.TrueName = clienter.TrueName;
            mode.ImprestPrice = imprestPrice;
            mode.PhoneNo = clienter.PhoneNo;
            mode.Status = 1;
            mode.WithdrawingPrice = amount;
            mode.AccountBalance = clienter.AccountBalance;
            mode.AllowWithdrawPrice = clienter.AllowWithdrawPrice;
            return mode;
        }

        /// <summary>
        /// 查询备用金流水列表  add by 彭宜  20150812
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ETS.Data.PageData.PageInfo<ImprestBalanceRecordModel> GetImprestBalanceRecordList(ImprestBalanceRecordSearchCriteria criteria)
        {
            return _imprestBalanceRecordDao.GetImprestBalanceRecordList(criteria);
        }
    }
}
