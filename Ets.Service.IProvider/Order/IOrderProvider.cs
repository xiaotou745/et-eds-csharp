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
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        int GetStatus(string orderNo);
        /// </summary>        /// 商户发布订单信息转换为数据库对应实体
        /// </summary>
        /// <param name="busiOrderInfoModel"></param>
        /// <returns></returns>
        order TranslateOrder(BusiOrderInfoModel busiOrderInfoModel);

        /// <summary>
        /// 添加一条订单记录
        /// </summary>
        string AddOrder(order order);

        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns></returns>
        int Create(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel);
    }
}
