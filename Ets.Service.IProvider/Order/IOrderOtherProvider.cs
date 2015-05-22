using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Bussiness;
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
        /// 更新一条记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="grabLongitude"></param>
        /// <param name="grabLatitude"></param>
        void Update(string orderNo, float grabLongitude, float grabLatitude);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="completeLongitude"></param>
        /// <param name="completeLatitude"></param>
        void UpdateComplete(string orderNo, float completeLongitude, float completeLatitude);

        /// <summary>
        /// 更新已提现
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateIsJoinWithdraw(int orderId);
    }
}
