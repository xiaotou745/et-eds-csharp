using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    /// 备用金操作记录  add by caoheyang 20150812
    /// </summary>
    public interface IImprestBalanceRecordProvider
    {
        /// <summary>
        /// 通过手机号获取骑士的提现信息
        /// 2015年8月12日17:56:40
        /// 茹化肖
        /// </summary>
        /// <param name="phonenum"></param>
        /// <returns></returns>
        ImprestClienterModel ClienterPhoneCheck(string phonenum);
    }
}
