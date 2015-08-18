using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Order;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Enums;
namespace Ets.Service.IProvider.Order
{
    public interface IOrderOtherProvider
    {
        /// <summary>
        /// 更新已提现
        /// 胡灵波
        /// 2015年8月18日 18:00:29
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateIsJoinWithdraw(int orderId);

        /// <summary>
        /// 更新取消时间
        /// 胡灵波
        /// 2015年8月18日 17:57:11
        bool UpdateCancelTime(int orderId);
    }
}
