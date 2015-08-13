using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;

namespace Ets.Service.IProvider.Finance
{

    /// <summary>
    /// 备用金 
    /// add by caoheyang 20150812
    /// </summary>
   public interface IImprestRechargeProvider
    {
        /// <summary>
        /// 获取备用金信息(锁库)
        /// 2015年8月12日17:51:49
        /// 茹化肖
        /// </summary>
        ImprestRecharge GetRemainingAmountLock();
    }
}
