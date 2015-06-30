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
using Ets.Model.DataModel.Business;
using ETS.Enums;
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
        order TranslateOrder(BussinessOrderInfoPM busiOrderInfoModel,out BusListResultModel business);

        /// <summary>
        /// 添加一条订单记录
        /// </summary>
        PubOrderStatus AddOrder(order order);
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
        OrderListModel GetOrderByNo(string orderNo, int orderId);
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
        /// 第三方订单列表根据订单号 修改订单状态   平杨  TODO 目前支适用于美团
        /// </summary>
        ///  <UpdateBy>确认接入时扣除商家结算费功能  caoheyang 20150526</UpdateBy>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">目标订单状态</param>
        /// <param name="remark"></param>
        /// <param name="status">原始订单状态</param>
        /// <returns></returns>
        int UpdateOrderStatus(string orderNo, int orderStatus, string remark, int? status);
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        int GetOrderStatus(int orderId, int businessId);

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
        bool AsyncOrderStatus(string orderNo);

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
        OrderListModel GetOrderInfoByOrderNo(string orderNo, int orderId = 0);
        ///// <summary>
        ///// 取消订单
        ///// danny-20150414
        ///// </summary>
        ///// <param name="orderOptionModel"></param>
        ///// <returns></returns>
        //bool CancelOrderByOrderNo(OrderOptionModel orderOptionModel);
        /// <summary>
        /// 取消订单 后台取消
        /// danny-20150521
        /// </summary>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        DealResultInfo CancelOrderByOrderNo(OrderOptionModel orderOptionModel);

        /// <summary>
        /// 获取订单操作日志
        /// danny-20150414
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        IList<OrderSubsidiesLog> GetOrderOptionLog(int OrderId);

        /// <summary>
        /// 第三方更新E代送订单状态   add by caoheyang 20150421  
        /// </summary>
        /// <param name="paramodel">参数</param>
        /// <returns></returns>
        ResultModel<object> UpdateOrderStatus_Other(ChangeStatusPM_OpenApi paramodel);

        /// <summary>
        /// 判断订单是否存在      
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        bool IsExist(int id);
        /// <summary>
        /// 获取主订单信息
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderPM">获取订单详情参数实体</param>
        /// <returns></returns>
        order GetById(OrderPM orderPM);
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="id">订单查询实体</param>
        /// <returns></returns>
        OrderDM GetDetails(OrderPM modelPM);

        /// <summary>
        /// 骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        ResultModel<object> GetJobC(GetJobCPM model);

        void UpdateTake(OrderPM modelPM);

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150520</UpdateTime>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetStatus(int id);

        /// <summary>
        /// 商户端 取消订单  商户端只能取消 待接单的订单  add by caoehyang  20150521 
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        ResultModel<bool> CancelOrderB(CancelOrderBPM paramodel);
        /// <summary>
        /// 配送数据分析
        /// </summary>
        /// <param name="model"></param>
        IList<DistributionAnalyzeResult> DistributionAnalyze(OrderDistributionAnalyze model, int pageIndex,int pageSize, out int totalRows);
        /// <summary>
        /// 订单中所有城市去重列表
        /// </summary>
        /// <returns></returns>
        IList<string> OrderReceviceCity();

        /// <summary>
        /// 根据orderID获取订单地图数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderMapDetail GetOrderMapDetail(long orderID);
        /// <summary>
        /// 一键发单修改地址和电话
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newAddress"></param>
        /// <param name="newPhone"></param>
        /// <returns></returns>
        bool UpdateOrderAddressAndPhone(string orderId, string newAddress, string newPhone);
    }
}
