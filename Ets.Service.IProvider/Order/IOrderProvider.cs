using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Order
{
    public interface IOrderProvider
    {
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<ClientOrderResultModel> GetOrders(ClientOrderSearchCriteria criteria);
        /// <summary>
        /// 未登录时候获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<ClientOrderNoLoginResultModel> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria);
        /// <summary>
        /// 商户发布订单信息转换为数据库对应实体
        /// </summary>
        /// <param name="busiOrderInfoModel"></param>
        /// <returns></returns>
        order TranslateOrder(BusiOrderInfoModel busiOrderInfoModel);
    }
}
