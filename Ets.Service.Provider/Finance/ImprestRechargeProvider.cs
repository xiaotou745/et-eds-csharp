using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Finance;

namespace Ets.Service.Provider.Finance
{
    /// <summary>
    /// 备用金 
    /// add by caoheyang 20150812
    /// </summary>
    public class ImprestRechargeProvider : IImprestRechargeProvider
    {
        private ImprestRechargeDao imprestRechargeDao = new ImprestRechargeDao();

        /// <summary>
        /// 获取备用金信息(锁库)
        /// 2015年8月12日17:51:49
        /// 茹化肖
        /// </summary>
        public ImprestRecharge GetRemainingAmountLock()
        {
            return imprestRechargeDao.GetRemainingAmountLock();
        }
    }
}
