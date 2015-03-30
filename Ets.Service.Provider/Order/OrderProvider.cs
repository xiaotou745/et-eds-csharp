using CalculateCommon;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.MyPush;
using Ets.Service.Provider.Subsidy;
using Ets.Service.Provider.User;
using ETS.Enums;
using ETS.Data.PageData;
using ETS.Page;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Subsidy;
using Newtonsoft.Json.Linq;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using System.Configuration;
using System.Net.Http;
using Ets.Dao.User;

namespace Ets.Service.Provider.Order
{
    public class OrderProvider : IOrderProvider
    {
        private OrderDao OrderDao = new OrderDao();
        private IBusinessProvider iBusinessProvider = new BusinessProvider();
        private ISubsidyProvider iSubsidyProvider = new SubsidyProvider();

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ClientOrderResultModel> GetOrders(ClientOrderSearchCriteria criteria)
        {
            IList<ClientOrderResultModel> list = new List<ClientOrderResultModel>();
            var orderList = OrderDao.GetOrders(criteria);
            for (int i = 0; i < orderList.ContentList.Count; i++)
            {
                var resultModel = new ClientOrderResultModel();
                var from = orderList.ContentList[i];
                if (from.clienterId != null)
                    resultModel.userId = from.clienterId.Value;
                resultModel.OrderNo = from.OrderNo;
                resultModel.OrderCount = from.OrderCount;
                var orderComm = new OrderCommission() { Amount = from.Amount, CommissionRate = from.CommissionRate, DistribSubsidy = from.DistribSubsidy, OrderCount = from.OrderCount, WebsiteSubsidy = from.WebsiteSubsidy };
                var amount = OrderCommissionProvider.GetCurrenOrderPrice(orderComm);

                resultModel.income = from.OrderCommission;  //佣金 Edit bycaoheyang 20150327
                resultModel.Amount = amount; //C端 获取订单的金额 Edit bycaoheyang 20150305


                resultModel.businessName = from.BusinessName;
                resultModel.businessPhone = from.BusinessPhone;
                if (from.PickUpCity != null)
                {
                    resultModel.pickUpCity = from.PickUpCity.Replace("市", "");
                }

                if (from.PubDate.HasValue)
                {
                    resultModel.pubDate = from.PubDate.Value.ToShortTimeString();
                }
                resultModel.pickUpAddress = from.PickUpAddress;
                resultModel.receviceName = from.ReceviceName;
                resultModel.receviceCity = from.ReceviceCity;
                resultModel.receviceAddress = from.ReceviceAddress;
                resultModel.recevicePhone = from.RecevicePhoneNo;
                resultModel.IsPay = from.IsPay.Value;
                resultModel.Remark = from.Remark == null ? "" : from.Remark;
                resultModel.Status = from.Status.Value;

                if (from.BusiLatitude == null || from.BusiLatitude == 0 || from.BusiLongitude == null || from.BusiLongitude == 0)
                {
                    resultModel.distance = "--";
                    resultModel.distanceB2R = "--";
                }
                else
                {
                    if (degree.longitude == 0 || degree.latitude == 0)
                        resultModel.distance = "--";
                    else //计算超人当前到商户的距离
                    {
                        Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                        Degree degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value); ; //商户经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        resultModel.distance = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    if (from.ReceviceLongitude != null && from.ReceviceLatitude != null
                        && from.ReceviceLongitude != 0 && from.ReceviceLatitude != 0)  //计算商户到收货人的距离
                    {
                        Degree degree1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);  //商户经纬度
                        Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);  //收货人经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        resultModel.distanceB2R = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    else
                        resultModel.distanceB2R = "--";
                }
                list.Add(resultModel);
            }
            return list;
        }

        /// <summary>
        /// 未登录时获取最新任务     登录未登录根据城市有没有值判断。
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        #region
        public IList<ClientOrderNoLoginResultModel> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            IList<ClientOrderNoLoginResultModel> list = new List<ClientOrderNoLoginResultModel>();
            var orderList = OrderDao.GetOrders(criteria);
            for (int i = 0; i < orderList.ContentList.Count; i++)
            {
                var resultModel = new ClientOrderNoLoginResultModel();
                var from = orderList.ContentList[i];
                if (from.clienterId != null)
                    resultModel.userId = from.clienterId.Value;
                resultModel.OrderNo = from.OrderNo;
                resultModel.OrderCount = from.OrderCount;
                var orderComm = new OrderCommission() { Amount = from.Amount, CommissionRate = from.CommissionRate, DistribSubsidy = from.DistribSubsidy, OrderCount = from.OrderCount, WebsiteSubsidy = from.WebsiteSubsidy };
                var amount = OrderCommissionProvider.GetCurrenOrderPrice(orderComm);
                resultModel.income = from.OrderCommission;  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
                resultModel.Amount = amount; //C端 获取订单的金额 Edit bycaoheyang 20150305
                resultModel.businessName = from.BusinessName;
                resultModel.businessPhone = from.BusinessPhone;

                if (from.PickUpCity != null)
                {
                    resultModel.pickUpCity = from.PickUpCity.Replace("市", "");
                }

                if (from.PubDate.HasValue)
                {
                    resultModel.pubDate = from.PubDate.Value.ToShortTimeString();
                }
                resultModel.pickUpAddress = from.PickUpAddress;
                resultModel.receviceName = from.ReceviceName;
                resultModel.receviceCity = from.ReceviceCity;
                resultModel.receviceAddress = from.ReceviceAddress;
                resultModel.recevicePhone = from.RecevicePhoneNo;
                resultModel.IsPay = from.IsPay.Value;
                resultModel.Remark = from.Remark;
                resultModel.Status = from.Status.Value;


                if (from.BusiLatitude == null || from.BusiLatitude == 0 || from.BusiLongitude == null || from.BusiLongitude == 0)
                {
                    resultModel.distance = "--";
                    resultModel.distanceB2R = "--";
                }
                else
                {
                    if (degree.longitude == 0 || degree.latitude == 0)
                        resultModel.distance = "--";
                    else //计算超人当前到商户的距离
                    {
                        Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                        Degree degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value); ; //商户经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        resultModel.distance = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    if (from.ReceviceLongitude != null && from.ReceviceLatitude != null
                        && from.ReceviceLongitude != 0 && from.ReceviceLatitude != 0)  //计算商户到收货人的距离
                    {
                        Degree degree1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);  //商户经纬度
                        Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);  //收货人经纬度
                        double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                        resultModel.distanceB2R = res < 1000 ? (res.ToString("f2") + "米") : ((res / 1000).ToString("f2") + "公里");
                    }
                    else
                        resultModel.distanceB2R = "--";
                }


                list.Add(resultModel);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 转换B端发布的订单信息为 数据库中需要的 订单 数据
        /// </summary>
        /// <param name="busiOrderInfoModel"></param>
        /// <returns></returns>
        public order TranslateOrder(Model.ParameterModel.Bussiness.BusiOrderInfoModel busiOrderInfoModel)
        {
            order to = new order();

            to.OrderNo = Helper.generateOrderCode(busiOrderInfoModel.userId);  //根据userId生成订单号(15位)
            to.businessId = busiOrderInfoModel.userId; //当前发布者
            BusListResultModel business = iBusinessProvider.GetBusiness(busiOrderInfoModel.userId);
            //business business = BusiLogic.busiLogic().GetBusinessById(busiOrderInfoModel.userId);  //根据发布者id,获取发布者的相关信息实体
            if (business != null)
            {
                to.PickUpCity = business.City;  //商户所在城市
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                to.ReceviceCity = business.City; //城市
                to.DistribSubsidy = business.DistribSubsidy;//设置外送费,从商户中找。
            }
            if (ConfigSettings.Instance.IsGroupPush)
            {
                if (busiOrderInfoModel.OrderFrom != 0)
                    to.OrderFrom = busiOrderInfoModel.OrderFrom;
                else
                    to.OrderFrom = 0;
            }
            to.Remark = busiOrderInfoModel.Remark;
            if (string.IsNullOrWhiteSpace(busiOrderInfoModel.receviceName))
            {
                to.ReceviceName = "";
            }
            else
            {
                to.ReceviceName = busiOrderInfoModel.receviceName;
            }
            to.RecevicePhoneNo = busiOrderInfoModel.recevicePhone;
            to.ReceviceAddress = busiOrderInfoModel.receviceAddress;
            to.IsPay = busiOrderInfoModel.IsPay;
            to.Amount = busiOrderInfoModel.Amount;
            to.OrderCount = busiOrderInfoModel.OrderCount;  //订单数量
            to.ReceviceLongitude = busiOrderInfoModel.longitude;
            to.ReceviceLatitude = busiOrderInfoModel.laitude;
            SubsidyResultModel subsidy = iSubsidyProvider.GetCurrentSubsidy(groupId: business.GroupId == null ? 0 : Convert.ToInt32(business.GroupId));
            if (subsidy != null)
            {
                to.WebsiteSubsidy = subsidy.WebsiteSubsidy;  //网站补贴
                to.CommissionRate = subsidy.OrderCommission == null ? 0 : subsidy.OrderCommission; //佣金比例 
                OrderCommission orderComm = new OrderCommission() { Amount = busiOrderInfoModel.Amount, CommissionRate = subsidy.OrderCommission, DistribSubsidy = to.DistribSubsidy, OrderCount = busiOrderInfoModel.OrderCount, WebsiteSubsidy = subsidy.WebsiteSubsidy };
                //必须写to.DistribSubsidy ，防止bussiness为空情况
                to.OrderCommission = CommissionFactory.GetCommission().GetCurrenOrderCommission(orderComm);
                to.CommissionFormulaMode = ConfigSettings.Instance.OrderCommissionType;
            }
            to.Status = ConstValues.ORDER_NEW;
            return to;
        }

        /// <summary>
        /// 添加一条 订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string AddOrder(order order)
        {
            int result = OrderDao.AddOrder(order);
            if (result > 0)
            {
                Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！", string.Empty, order.PickUpCity); //激光推送
                return "1";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 根据参数获取订单
        /// danny-20150319
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<OrderListModel> GetOrders(OrderSearchCriteria criteria)
        {
            PageInfo<OrderListModel> pageinfo = OrderDao.GetOrders<OrderListModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 更新订单佣金
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool UpdateOrderInfo(order order)
        {
            return OrderDao.UpdateOrderInfo(order);
        }
        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo)
        {
            return OrderDao.GetOrderByNo(orderNo);
        }
        /// <summary>
        /// 订单指派超人
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RushOrder(OrderListModel order)
        {
            if (OrderDao.RushOrder(order))
            {
                Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", order.businessId.ToString(), string.Empty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据订单号查询 是否存在该订单
        /// wangchao 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public int GetOrderByOrderNo(string orderNo)
        {
            return OrderDao.GetOrderByOrderNo(orderNo);
        }
        /// <summary>
        /// 根据订单号 修改订单状态
        /// wc
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int UpdateOrderStatus(string orderNo, int orderStatus)
        {
            int result = OrderDao.CancelOrderStatus(orderNo, orderStatus);
            if (result > 0) //更该订单状态时，同步第三方订单状态
            {
                AsyncOrderStatus(orderNo);
            }
            return result;
        }



        #region openapi 接口使用 add by caoheyang  20150325

        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单号码</returns>
        public string Create(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                paramodel.CommissionFormulaMode = ConfigSettings.Instance.OrderCommissionType;
                ISubsidyProvider subsidyProvider = new SubsidyProvider();//补贴记录
                SubsidyResultModel subsidy = subsidyProvider.GetCurrentSubsidy(paramodel.store_info.group);
                //计算获得订单骑士佣金
                paramodel.ordercommission = CommissionFactory.GetCommission().GetCurrenOrderCommission(new OrderCommission()
                {
                    CommissionRate = subsidy.OrderCommission,/*佣金比例*/
                    Amount = paramodel.total_price, /*订单金额*/
                    DistribSubsidy = paramodel.delivery_fee,/*外送费*/
                    OrderCount = paramodel.package_count,/*订单数量*/
                    WebsiteSubsidy = subsidy.WebsiteSubsidy
                }/*网站补贴*/);
                paramodel.websitesubsidy = ParseHelper.ToDecimal(subsidy.WebsiteSubsidy);//网站补贴
                paramodel.commissionrate = ParseHelper.ToDecimal(subsidy.OrderCommission);//订单佣金比例
                string orderNo = OrderDao.CreateToSql(paramodel);
                if (!string.IsNullOrWhiteSpace(orderNo))
                    Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！"
                        , string.Empty, paramodel.address.city_code); //激光推送   
                tran.Complete();
                return orderNo;
            }
        }

        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="orderNo">订单号码</param>
        /// <param name="groupId">集团id</param>
        /// <returns>订单状态</returns>
        public int GetStatus(string OriginalOrderNo, int groupId)
        {
            OrderDao OrderDao = new OrderDao();
            return OrderDao.GetStatus(OriginalOrderNo, groupId);
        }


        /// <summary>
        /// 查看订单详情接口  add by caoheyang 20150325
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        public OrderDetailDM_OpenApi OrderDetail(OrderDetailPM_OpenApi paramodel)
        {
            return null;
        }

        /// <summary>
        ///  supermanapi通过openapi同步第三方订单状态  add by caoheyang 20150327 
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        public ResultModel<object> AsyncOrderStatus(string orderNo)
        {
            OrderListModel orderlistModel = OrderDao.GetOrderByNo(orderNo);
            ParaModel<AsyncStatusPM_OpenApi> paramodel = new ParaModel<AsyncStatusPM_OpenApi>() { group = orderlistModel.GroupId, fields = new AsyncStatusPM_OpenApi() };
            if (paramodel.GetSign() == null)//为当前集团参数实体生成sign签名信息
                return null;
            paramodel.fields.status = ParseHelper.ToInt(orderlistModel.Status, -1);
            paramodel.fields.ClienterTrueName = orderlistModel.ClienterTrueName;
            paramodel.fields.ClienterPhoneNo = orderlistModel.ClienterPhoneNo;
            paramodel.fields.BusinessName = orderlistModel.BusinessName;
            paramodel.fields.OriginalOrderNo = orderlistModel.OriginalOrderNo;
            string url = ConfigurationManager.AppSettings["AsyncStatus"];
            string json = new HttpClient().PostAsJsonAsync(url, paramodel).Result.Content.ReadAsStringAsync().Result;
            JObject jobject = JObject.Parse(json);
            return null;
        }

        #endregion

        /// <summary>
        /// 订单统计
        /// danny-20150326
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public OrderCountManageList GetOrderCount(HomeCountCriteria criteria)
        {
            PageInfo<OrderCountModel> pageinfo = OrderDao.GetOrderCount<OrderCountModel>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<OrderCountModel> list = pageinfo.Records.ToList();
            var orderCountManageList = new OrderCountManageList(list, pr);
            return orderCountManageList;
        }
        /// <summary>
        ///  首页最近数据统计
        /// danny-20150327
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public PageInfo<HomeCountTitleModel> GetCurrentDateCountAndMoney(OrderSearchCriteria criteria)
        {
            PageInfo<HomeCountTitleModel> pageinfo = OrderDao.GetCurrentDateCountAndMoney<HomeCountTitleModel>(criteria);
            return pageinfo;
        }


        /// <summary>
        /// 接收订单，供第三方使用
        /// 窦海超
        /// 2015年3月30日 11:44:28
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ETS.Expand.ActionStatus(typeof(OrderPublicshStatus))]
        public Ets.Model.Common.ResultModel<Model.DomainModel.Order.NewPostPublishOrderResultModel> NewPostPublishOrder_B(Model.ParameterModel.Order.NewPostPublishOrderModel model)
        {
            if (string.IsNullOrWhiteSpace(model.OriginalOrderNo))   //原始订单号非空验证
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OriginalOrderNoEmpty);
            if (model.OriginalBusinessId == 0)   //原平台商户Id非空验证
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OriginalBusinessIdEmpty);
            if (string.IsNullOrWhiteSpace(model.OrderFrom.ToString()))   //订单来源
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OrderFromEmpty);
            if (string.IsNullOrWhiteSpace(model.IsPay.ToString()))   //请确认是否已付款
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.IsPayEmpty);

            if (string.IsNullOrWhiteSpace(model.ReceiveName))    //收货人名称
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveNameEmpty);

            if (string.IsNullOrWhiteSpace(model.ReceivePhoneNo)) //手机号
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceivePhoneEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Province) || string.IsNullOrWhiteSpace(model.Receive_ProvinceCode))  //所在省
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveProvinceEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_City) || string.IsNullOrWhiteSpace(model.Receive_CityCode))  //所在市
                return ResultModel<NewPostPublishOrderResultModel>.Conclude
                    (OrderPublicshStatus.ReceiveCityEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Area) || string.IsNullOrWhiteSpace(model.Receive_AreaCode))  //所在区
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveAreaEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Address))   //收货地址
                return ResultModel<NewPostPublishOrderResultModel>.Conclude
                    (OrderPublicshStatus.ReceiveAddressEmpty);
            //验证原平台商户是否已经注册
            //var busi = BusiLogic.busiLogic().GetBusiByOriIdAndOrderFrom(model.OriginalBusinessId, model.OrderFrom);
            BusinessDao busDao = new BusinessDao();
            Business busi = busDao.GetBusiByOriIdAndOrderFrom(model.OriginalBusinessId, model.OrderFrom);
            if (busi == null)
            {
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNoExist);
            }
            else
            {
                if (busi.Status != ConstValues.BUSINESS_AUDITPASS)
                {
                    return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNotAudit);
                }
            }
            //验证该平台 商户 订单号 是否存在
            Ets.Dao.Order.OrderDao orderDao = new Dao.Order.OrderDao();
            var order = orderDao.GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            //var order = OrderLogic.orderLogic().GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            if (order != null)
            {
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OrderHadExist);
            }

            order dborder = OrderInstance(model);  //整合订单信息
            bool result = Convert.ToBoolean(AddOrder(dborder));    //添加订单记录，并且触发极光推送。          
            if (result)
            {
                NewPostPublishOrderResultModel resultModel = new NewPostPublishOrderResultModel { OriginalOrderNo = model.OriginalOrderNo, OrderNo = dborder.OrderNo };
                LogHelper.LogWriter("订单发布成功", new { model = model, resultModel = resultModel });
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Success, resultModel);
            }
            else
            {
                NewPostPublishOrderResultModel resultModel = new NewPostPublishOrderResultModel { Remark = "订单发布失败" };
                LogHelper.LogWriter("订单发布失败", new { model = model });
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Failed);
            }
        }

        /// <summary>
        /// 获取订单信息
        /// 窦海超
        /// 2015年3月30日 17:12:17
        /// </summary>
        /// <param name="from">参数对象</param>
        /// <returns></returns>
        private order OrderInstance(Model.ParameterModel.Order.NewPostPublishOrderModel from)
        {
            order to = new order();
            BusinessDao businessDao = new BusinessDao();
            Business abusiness = businessDao.GetBusiByOriIdAndOrderFrom(from.OriginalBusinessId, from.OrderFrom);

            if (abusiness != null)
            {
                from.BusinessId = abusiness.Id;
            }
            else
            {
                return null;
            }
            to.OrderNo = Helper.generateOrderCode(abusiness.Id);  //根据userId生成订单号(15位)
            to.businessId = abusiness.Id; //当前发布者
            BusListResultModel business = businessDao.GetBusiness(abusiness.Id);  //根据发布者id,获取发布者的相关信息实体
            if (business != null)
            {
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                to.ReceviceCity = business.City; //城市
            }
            to.SongCanDate = from.SongCanDate; //送餐时间
            to.Remark = from.Remark;

            to.ReceviceName = from.ReceiveName;
            to.RecevicePhoneNo = from.ReceivePhoneNo;

            to.ReceiveProvince = from.Receive_Province;
            to.ReceiveProvinceCode = from.Receive_ProvinceCode;

            to.ReceviceCity = from.Receive_City;
            to.ReceiveCityCode = from.Receive_CityCode;

            to.ReceiveArea = from.Receive_Area;
            to.ReceiveAreaCode = from.Receive_AreaCode;

            to.ReceviceLatitude = from.Receive_Latitude;
            to.ReceviceLongitude = from.Receive_Longitude;

            to.ReceviceAddress = from.Receive_Address;

            to.OrderFrom = from.OrderFrom;
            to.Quantity = from.Quantity;
            to.OriginalOrderNo = from.OriginalOrderNo;

            to.Weight = from.Weight;

            to.IsPay = from.IsPay;
            to.Amount = from.Amount;

            to.OrderType = from.OrderType; //订单类型 1送餐订单 2取餐盒订单 
            to.KM = from.KM; //送餐距离

            to.GuoJuQty = from.GuoJuQty; //锅具数量
            to.LuJuQty = from.LuJuQty;  //炉具数量

            to.DistribSubsidy = from.DistribSubsidy; //外送费
            to.OrderCount = from.OrderCount == 0 ? 1 : from.OrderCount; //订单数量
            //计算订单佣金
            //var subsidy = GetCurrentSubsidy(business.GroupId);

            var subsidy = new OrderDao().GetCurrentSubsidy(business.GroupId.Value);//设置结算比例
            if (subsidy != null)
            {
                to.WebsiteSubsidy = subsidy.WebsiteSubsidy == null ? 0 : subsidy.WebsiteSubsidy; //网站补贴 
                to.CommissionRate = subsidy.OrderCommission == null ? 0 : subsidy.OrderCommission; //佣金比例 
            }
            else
            {
                to.WebsiteSubsidy = 0m;
                to.CommissionRate = 0m;
            }
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            if (to.DistribSubsidy != null)//如果外送费有数据，按照外送费计算骑士佣金
                distribe = Convert.ToDecimal(to.DistribSubsidy);
            else if (to.WebsiteSubsidy != null)//如果外送费没数据，按照网站补贴计算骑士佣金
                distribe = Convert.ToDecimal(to.WebsiteSubsidy);

            to.OrderCommission = from.Amount * to.CommissionRate + distribe * to.OrderCount;//计算佣金

            to.Status = ConstValues.ORDER_NEW;

            return to;
        }

    }
}
