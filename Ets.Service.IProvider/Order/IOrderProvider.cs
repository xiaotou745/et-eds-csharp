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
        /// 根据参数获取订单
        /// danny-20150319
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<OrderListModel> GetOrders(OrderSearchCriteria criteria);
        /// <summary>
        /// 更新订单佣金
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool UpdateOrderInfo(order order);
        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        OrderListModel GetOrderByNo(string orderNo);
        /// <summary>
        /// 订单指派超人
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool RushOrder(OrderListModel order);
        /// <summary>
        /// 根据订单号获取订单信息
        /// wc
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        int GetOrderByOrderNo(string orderNo);
        /// <summary>
        /// 根据订单号 修改订单状态
        /// wc
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        int UpdateOrderStatus(string orderNo, int orderStatus);


        #region openapi 接口使用 add by caoheyang  20150325

        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="orderNo">订单号码</param>
        /// <param name="groupId">集团id</param>
        /// <returns>订单状态</returns>
        int GetStatus(string OriginalOrderNo, int groupId);

        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单号</returns>
        ResultModel<object> Create(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel);

        /// <summary>
        /// 查看订单详情接口  add by caoheyang 20150325
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        ResultModel<object> OrderDetail(OrderDetailPM_OpenApi paramodel);

        /// <summary>
        ///  supermanapi通过openapi同步第三方订单状态  add by caoheyang 20150327 
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        void AsyncOrderStatus(string orderNo);

        #endregion
        /// <summary>
        /// 订单统计
        /// danny-20150326
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        OrderCountManageList GetOrderCount(HomeCountCriteria criteria);
        /// <summary>
        /// 首页最近数据统计
        /// danny-20150327
        /// </summary>
        /// <param name="DayCount"></param>
        /// <returns></returns>
        PageInfo<HomeCountTitleModel> GetCurrentDateCountAndMoney(OrderSearchCriteria criteria);

        /// <summary>
        /// 接收订单，供第三方使用
        /// 窦海超
        /// 2015年3月30日 11:44:28
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel<Ets.Model.DomainModel.Order.NewPostPublishOrderResultModel> NewPostPublishOrder_B(Ets.Model.ParameterModel.Order.NewPostPublishOrderModel model);
        /// <summary>
        /// 根据订单号获取订单信息
        /// wc
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        OrderListModel GetOrderInfoByOrderNo(string orderNo);
        /// <summary>
        /// 取消订单
        /// danny-20150414
        /// </summary>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        bool CancelOrderByOrderNo(OrderOptionModel orderOptionModel);
        /// <summary>
        /// 获取订单操作日志
        /// danny-20150414
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        IList<OrderSubsidiesLog> GetOrderOptionLog(string OrderId);

    }
}
