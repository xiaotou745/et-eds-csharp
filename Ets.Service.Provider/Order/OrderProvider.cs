#region
using CalculateCommon;
using Ets.Dao.Finance;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Finance;
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
using Ets.Service.Provider.OpenApi;
using System.Configuration;
using System.Net.Http;
using Ets.Dao.User;
using Ets.Dao.GlobalConfig;
using Ets.Service.Provider.Common;
using ETS.Const;
using Ets.Service.Provider.Clienter;
using Ets.Service.IProvider.OpenApi;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Service.IProvider.Statistics;
using Ets.Model.DataModel.Strategy;
using Ets.Service.Provider.Order;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
#endregion
namespace Ets.Service.Provider.Order
{
    public class OrderProvider : IOrderProvider
    {
        private OrderDao orderDao = new OrderDao();
        private BusinessProvider iBusinessProvider = new BusinessProvider();
        private ClienterProvider iClienterProvider = new ClienterProvider();

        private ISubsidyProvider iSubsidyProvider = new SubsidyProvider();
        private IBusinessGroupProvider iBusinessGroupProvider = new BusinessGroupProvider();
        private readonly BusinessDao _businessDao = new BusinessDao();
        private readonly BusinessBalanceRecordDao _businessBalanceRecordDao = new BusinessBalanceRecordDao();
        //和区域有关的  wc
        readonly Ets.Service.IProvider.Common.IAreaProvider iAreaProvider = new Ets.Service.Provider.Common.AreaProvider();

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ClientOrderResultModel> GetOrders(ClientOrderSearchCriteria criteria)
        {
            IList<ClientOrderResultModel> list = new List<ClientOrderResultModel>();
            var orderList = orderDao.GetOrders(criteria);
            if (orderList != null && orderList.ContentList != null)
            {
                for (int i = 0; i < orderList.ContentList.Count; i++)
                {
                    var resultModel = new ClientOrderResultModel();
                    var from = orderList.ContentList[i];
                    if (from.clienterId != null)
                        resultModel.userId = from.clienterId.Value;
                    resultModel.OrderNo = from.OrderNo;
                    resultModel.OrderId = from.Id; //订单Id
                    resultModel.OriginalOrderNo = from.OriginalOrderNo;
                    resultModel.OrderFrom = from.OrderFrom; //订单来源
                    resultModel.OrderCount = from.OrderCount;
                    var orderComm = new OrderCommission() { Amount = from.Amount, DistribSubsidy = from.DistribSubsidy, OrderCount = from.OrderCount };
                    var amount = DefaultOrPriceProvider.GetCurrenOrderPrice(orderComm);

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
                    resultModel.HadUploadCount = from.HadUploadCount;
                    resultModel.GroupId = from.GroupId;
                    if (from.GroupId == SystemConst.Group3) //全时 需要做验证码验证
                        resultModel.NeedPickupCode = 1;

                    if (from.BusiLatitude == null || from.BusiLatitude == 0 || from.BusiLongitude == null || from.BusiLongitude == 0)
                    {
                        resultModel.distance = "--";
                        resultModel.distanceB2R = "--";
                        resultModel.distance_OrderBy = 9999999.0;
                    }
                    else
                    {
                        if (degree.longitude == 0 || degree.latitude == 0)
                        { resultModel.distance = "--"; resultModel.distance_OrderBy = 9999999.0; }
                        else //计算超人当前到商户的距离
                        {
                            Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                            Degree degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value); ; //商户经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                            resultModel.distance_OrderBy = res;
                        }
                        if (from.ReceviceLongitude != null && from.ReceviceLatitude != null
                            && from.ReceviceLongitude != 0 && from.ReceviceLatitude != 0)  //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);  //商户经纬度
                            Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);  //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distanceB2R = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                        }
                        else
                            resultModel.distanceB2R = "--";
                    }
                    list.Add(resultModel);
                }
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
            var orderList = orderDao.GetOrders(criteria);
            if (orderList != null && orderList.ContentList != null)
            {
                for (int i = 0; i < orderList.ContentList.Count; i++)
                {
                    var resultModel = new ClientOrderNoLoginResultModel();
                    var from = orderList.ContentList[i];
                    if (from.clienterId != null)
                        resultModel.userId = from.clienterId.Value;
                    resultModel.OrderNo = from.OrderNo;
                    resultModel.OrderId = from.Id;  //订单Id
                    resultModel.OrderCount = from.OrderCount;
                    resultModel.OriginalOrderNo = from.OriginalOrderNo;
                    resultModel.OrderFrom = from.OrderFrom;
                    var orderComm = new OrderCommission() { Amount = from.Amount, DistribSubsidy = from.DistribSubsidy, OrderCount = from.OrderCount };
                    var amount = DefaultOrPriceProvider.GetCurrenOrderPrice(orderComm);
                    resultModel.income = from.OrderCommission;  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
                    resultModel.Amount = amount; //C端 获取订单的金额 Edit bycaoheyang 20150305
                    resultModel.businessName = from.BusinessName;
                    resultModel.businessPhone = from.BusinessPhone;
                    resultModel.HadUploadCount = from.HadUploadCount;
                    resultModel.GroupId = from.GroupId;
                    if (from.GroupId == SystemConst.Group3)
                        resultModel.NeedPickupCode = 1;

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

                        resultModel.distance_OrderBy = 9999999.0; //用来排序
                    }
                    else
                    {
                        if (degree.longitude == 0 || degree.latitude == 0)
                        {
                            resultModel.distance = "--";
                            resultModel.distance_OrderBy = 9999999.0;
                        }
                        else //计算超人当前到商户的距离
                        {
                            Degree degree1 = new Degree(degree.longitude, degree.latitude);   //超人当前的经纬度
                            Degree degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value); ; //商户经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                            resultModel.distance_OrderBy = res;
                        }
                        if (from.ReceviceLongitude != null && from.ReceviceLatitude != null
                            && from.ReceviceLongitude != 0 && from.ReceviceLatitude != 0)  //计算商户到收货人的距离
                        {
                            Degree degree1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);  //商户经纬度
                            Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);  //收货人经纬度
                            var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                            resultModel.distanceB2R = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                        }
                        else
                            resultModel.distanceB2R = "--";
                    }


                    list.Add(resultModel);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 转换B端发布的订单信息为 数据库中需要的 订单 数据
        /// </summary>
        /// <param name="busiOrderInfoModel"></param>
        /// <returns></returns>
        public order TranslateOrder(Model.ParameterModel.Bussiness.BussinessOrderInfoPM busiOrderInfoModel)
        {
            order to = new order();
            ///TODO 订单号生成规则，定了以后要改；
            to.OrderNo = Helper.generateOrderCode(busiOrderInfoModel.userId);  //根据userId生成订单号(15位)
            to.businessId = busiOrderInfoModel.userId; //当前发布者
            BusListResultModel business = iBusinessProvider.GetBusiness(busiOrderInfoModel.userId);
            if (business != null)
            {
                to.PickUpCity = business.City;  //商户所在城市
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                to.ReceviceCity = business.City; //城市
                to.DistribSubsidy = business.DistribSubsidy;//设置外送费,从商户中找。
                to.BusinessCommission = ParseHelper.ToDecimal(business.BusinessCommission);//商户结算比例
                to.BusinessName = business.Name;
                to.CommissionType = business.CommissionType;//结算类型：1：固定比例 2：固定金额
                to.CommissionFixValue = ParseHelper.ToDecimal(business.CommissionFixValue);//固定金额     
                to.BusinessGroupId = business.BusinessGroupId;
                to.MealsSettleMode = business.MealsSettleMode;
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
                to.ReceviceName = "";
            else
                to.ReceviceName = busiOrderInfoModel.receviceName;
            to.RecevicePhoneNo = busiOrderInfoModel.recevicePhone;
            to.ReceviceAddress = busiOrderInfoModel.receviceAddress;
            to.IsPay = busiOrderInfoModel.IsPay;
            to.Amount = busiOrderInfoModel.Amount;
            to.OrderCount = busiOrderInfoModel.OrderCount;  //订单数量
            to.ReceviceLongitude = busiOrderInfoModel.longitude;
            to.ReceviceLatitude = busiOrderInfoModel.laitude;
            to.PubLongitude = busiOrderInfoModel.PubLongitude;//商户发单经度
            to.PubLatitude = busiOrderInfoModel.PubLatitude;


            //必须写to.DistribSubsidy ，防止bussiness为空情况
            OrderCommission orderComm = new OrderCommission()
            {
                Amount = busiOrderInfoModel.Amount, /*订单金额*/
                DistribSubsidy = to.DistribSubsidy,/*外送费*/
                OrderCount = busiOrderInfoModel.OrderCount/*订单数量*/,
                BusinessCommission = to.BusinessCommission, /*商户结算比例*/
                CommissionType = to.CommissionType,/*结算类型：1：固定比例 2：固定金额*/
                CommissionFixValue = to.CommissionFixValue,/*固定金额*/
                BusinessGroupId = business.BusinessGroupId,
                StrategyId = business.StrategyId

            };


            OrderPriceProvider commProvider = CommissionFactory.GetCommission(business.StrategyId);
            to.CommissionFormulaMode = business.StrategyId;
            to.CommissionRate = commProvider.GetCommissionRate(orderComm); //佣金比例 
            to.OrderCommission = commProvider.GetCurrenOrderCommission(orderComm); //订单佣金
            to.WebsiteSubsidy = commProvider.GetOrderWebSubsidy(orderComm);//网站补贴
            to.SettleMoney = commProvider.GetSettleMoney(orderComm);//订单结算金额            
            to.CommissionFormulaMode = ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet(business.BusinessGroupId).CommissionFormulaMode);
            to.Adjustment = commProvider.GetAdjustment(orderComm);//订单额外补贴金额           
            to.Status = Convert.ToByte(OrderStatus.订单待抢单.GetHashCode());
            to.TimeSpan = busiOrderInfoModel.TimeSpan;
            to.listOrderChild = busiOrderInfoModel.listOrderChlid;

            if (!(bool)to.IsPay && to.MealsSettleMode == MealsSettleMode.Status1.GetHashCode())//未付款且线上支付
            {                      
                    to.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(to.Amount) +
                                   ParseHelper.ToDecimal(to.DistribSubsidy) * ParseHelper.ToInt(to.OrderCount), 2);
            }
           
            return to;
        }

        /// <summary>
        /// 添加一条 订单信息
        /// </summary>   
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150514</UpdateTime>
        /// <param name="order">订单实体</param>
        /// <returns></returns>
        public PubOrderStatus AddOrder(order order)
        {
            int result = 0;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                bool isExist = orderDao.IsExist(order);
                if (isExist)
                {
                    return PubOrderStatus.OrderHasExist;
                }
                
                result = orderDao.AddOrder(order);              
                if (result <= 0)//订单发布失败
                {
                    return PubOrderStatus.InvalidPubOrder;
                }

                //扣商户金额
                _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                {
                    Id =Convert.ToInt32(order.businessId),
                    Money = -order.SettleMoney
                });

                #region 商户余额流水操作 
                _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                {
                    BusinessId = Convert.ToInt32(order.businessId),
                    Amount = -order.SettleMoney,
                    Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                    RecordType = (int)BusinessBalanceRecordRecordType.PublishOrder,
                    Operator = order.BusinessName,
                    Remark = "扣除商家结算费",
                    WithwardId = result,
                    RelationNo=order.OrderNo
                });
                #endregion

                if (order.Adjustment > 0)
                {
                    bool b = orderDao.addOrderSubsidiesLog(order.Adjustment, result, "补贴加钱,订单金额:" + order.Amount + "-佣金补贴策略id:" + order.CommissionFormulaMode);
                    if (!b)//写入日志失败
                    {
                        return PubOrderStatus.InvalidPubOrder;
                    }                                 
                }

                tran.Complete(); 
            }         
            return PubOrderStatus.Success;
        }

        /// <summary>
        /// 根据参数获取订单
        /// danny-20150319
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<OrderListModel> GetOrders(OrderSearchCriteria criteria)
        {
            PageInfo<OrderListModel> pageinfo = orderDao.GetOrders<OrderListModel>(criteria);
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
            return orderDao.UpdateOrderInfo(order);
        }
        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo)
        {
            return orderDao.GetOrderByNo(orderNo);
        }
        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo, int orderId)
        {
            return orderDao.GetOrderByNo(orderNo, orderId);
        }
        /// <summary>
        /// 订单指派超人
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RushOrder(OrderListModel order)
        {
            if (orderDao.RushOrder(order))
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
            return orderDao.GetOrderByOrderNo(orderNo);
        }
        /// <summary>
        /// 根据订单号 修改订单状态
        /// wc
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int UpdateOrderStatus(string orderNo, int orderStatus, string remark, int? status)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int result = orderDao.CancelOrderStatus(orderNo, orderStatus, remark, status);
                if (result > 0 & AsyncOrderStatus(orderNo))
                {
                    tran.Complete();
                    return 1;
                }
            }
            return 0;
        }



        #region openapi 接口使用 add by caoheyang  20150325

        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单号码</returns>
        public ResultModel<object> Create(CreatePM_OpenApi paramodel)
        {
            //查询缓存，看看当前店铺是否存在,缓存存储E代送的商户id
            var redis = new ETS.NoSql.RedisCache.RedisCache();

            #region 第三方订单是否重复推送的验证  add by caoheyang 20150417

            string orderExistsNo = redis.Get<string>(string.Format(ETS.Const.RedissCacheKey.OtherOrderInfo, paramodel.store_info.group.ToString(),
            paramodel.order_id.ToString()));  //查询缓存，看当前订单是否存在,“true”代表存在，key的形式为集团ID_第三方平台订单号
            if (orderExistsNo != null)
                return ResultModel<object>.Conclude(OrderApiStatusType.OrderExists, new { order_no = orderExistsNo });
            if (paramodel.total_price <= 0)  //金额小于等于0，数据不合法，返回信息 待用数据特性优化
            {
                return ResultModel<object>.Conclude(OrderApiStatusType.ParaError);
            }
            #endregion

            #region 设置用户的省市区编码信息 add by caoheyang 20150407
            string orderCodeInfo = new AreaProvider().GetOpenCode(new Ets.Model.ParameterModel.Area.ParaAreaNameInfo()
            {
                ProvinceName = paramodel.address.province,
                CityName = paramodel.address.city,
                AreaName = paramodel.address.area
            });
            if (orderCodeInfo == ETS.Const.SystemConst.CityOpenInfo || string.IsNullOrWhiteSpace(orderCodeInfo))
                return ResultModel<object>.Conclude(OrderApiStatusType.ParaError, "用户省市区信息错误");
            else
            {
                string[] storeCodes = orderCodeInfo.Split('_');
                paramodel.address.province_code = storeCodes[0];
                paramodel.address.city_code = storeCodes[1];
                paramodel.address.area_code = storeCodes[2];
            }
            #endregion

            #region  维护店铺相关信息 add by caoheyang 20150416
            string bussinessIdstr = redis.Get<string>(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo, paramodel.store_info.group.ToString(),
              paramodel.store_info.store_id.ToString()));
            if (bussinessIdstr == null || ParseHelper.ToInt(bussinessIdstr) == 0) //缓存中无店铺id 
            {
                //当第三方未传递经纬的情况下，根据地址调用百度接口获取经纬度信息  add by caoheyang 20150416
                if (paramodel.store_info.longitude == 0 || paramodel.store_info.latitude == 0)  //店铺经纬度
                {
                    Tuple<decimal, decimal> localtion = BaiDuHelper.GeoCoder(paramodel.store_info.province
                        + paramodel.store_info.city + paramodel.store_info.area + paramodel.store_info.address);
                    paramodel.store_info.longitude = localtion.Item1;  //精度
                    paramodel.store_info.latitude = localtion.Item2; //纬度
                }
                //if (paramodel.address.longitude == 0 || paramodel.address.latitude == 0)  //用户经纬度
                //{
                //    Tuple<decimal, decimal> localtion = BaiDuHelper.GeoCoder(paramodel.store_info.province
                //        + paramodel.address.city + paramodel.address.area + paramodel.address.address);
                //    paramodel.address.longitude = localtion.Item1;  //精度
                //    paramodel.address.latitude = localtion.Item2; //纬度
                //}

                #region 设置门店的省市区编码信息 add by caoheyang 20150407
                string storecodeInfo = new AreaProvider().GetOpenCode(new Ets.Model.ParameterModel.Area.ParaAreaNameInfo()
                {
                    ProvinceName = paramodel.store_info.province,
                    CityName = paramodel.store_info.city,
                    AreaName = paramodel.store_info.area
                });
                if (storecodeInfo == ETS.Const.SystemConst.CityOpenInfo || string.IsNullOrWhiteSpace(storecodeInfo))
                    return ResultModel<object>.Conclude(OrderApiStatusType.ParaError, "门店省市区信息错误");
                else
                {
                    string[] storeCodes = storecodeInfo.Split('_');
                    paramodel.store_info.province_code = storeCodes[0];
                    paramodel.store_info.city_code = storeCodes[1];
                    paramodel.store_info.area_code = storeCodes[2];
                }
                #endregion
            }
            else
                paramodel.businessId = ParseHelper.ToInt(bussinessIdstr);
            #endregion

            #region 根据集团id为店铺设置外送费，结算比例等财务相关信息add by caoheyang 20150417

            //此处其实应该取数据库，但是由于发布订单时关于店铺的逻辑后期要改，暂时这么处理 
            IGroupProviderOpenApi groupProvider = OpenApiGroupFactory.Create(paramodel.store_info.group);
            paramodel = groupProvider.SetCommissonInfo(paramodel);

            #endregion

            #region 佣金相关  add by caoheyang 20150416
            paramodel.CommissionFormulaMode = 0;//默认第三方策略，是普通策略 //ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet.CommissionFormulaMode);
            paramodel.CommissionType = 1;//结算类型：1：固定比例 2：固定金额
            paramodel.CommissionFixValue = 0;//固定金额
            paramodel.BusinessGroupId = 1;//分组ID

            //计算获得订单骑士佣金
            OrderCommission orderComm = new OrderCommission()
            {
                Amount = paramodel.total_price, /*订单金额*/
                DistribSubsidy = paramodel.store_info.delivery_fee,/*外送费*/
                OrderCount = paramodel.package_count ?? 1,/*订单数量，默认为1*/
                BusinessCommission = paramodel.store_info.businesscommission,/*商户结算比例*/
                BusinessGroupId = paramodel.BusinessGroupId,
                StrategyId = 0

            }/*网站补贴*/;
            OrderPriceProvider commissonPro = CommissionFactory.GetCommission(0);//万达、全时采用默认分组下策略
            paramodel.ordercommission = commissonPro.GetCurrenOrderCommission(orderComm);  //骑士佣金
            paramodel.websitesubsidy = commissonPro.GetOrderWebSubsidy(orderComm);//网站补贴
            paramodel.commissionrate = commissonPro.GetCommissionRate(orderComm);//订单佣金比例
            paramodel.settlemoney = commissonPro.GetSettleMoney(orderComm);//订单结算金额
            paramodel.adjustment = commissonPro.GetAdjustment(orderComm);//订单额外补贴金额

            #endregion

            string orderNo = null; //订单号码
            try
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    if (bussinessIdstr == null)
                    {
                        paramodel.businessId = orderDao.CreateToSqlAddBusiness(paramodel);  //商户id
                    }
                    orderNo = Helper.generateOrderCode(paramodel.businessId);
                    paramodel.OrderNo = orderNo;  
                    //将当前订单插入到缓存中，设置过期时间30天
                    redis.Set(string.Format(ETS.Const.RedissCacheKey.OtherOrderInfo, paramodel.store_info.group.ToString(),
                        paramodel.order_id.ToString()), orderNo, DateTime.Now.AddDays(30));  //先加入缓存，相当于加锁
                    int orderId = orderDao.CreateToSql(paramodel);  //插入订单返回订单id
                    orderDao.CreateToSqlAddOrderOther(paramodel.businessId, orderId); //操作插入rderOther表
                    orderDao.CreateToSqlAddOrderDetail(paramodel, orderNo); //操作插入OrderDetail表
                    orderDao.CreateToSqlAddOrderChild(paramodel, orderId); //插入订单子表
                    InsertOrderOptRecord(new order()
                    {
                        businessId = paramodel.businessId,
                        SettleMoney=paramodel.settlemoney,
                        Adjustment=paramodel.adjustment,
                        Id=orderId,
                        BusinessName = paramodel.store_info.store_name,
                        Amount = paramodel.total_price,
                        OrderNo=orderNo,
                        CommissionFormulaMode = paramodel.CommissionFormulaMode
                    });
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                redis.Delete(string.Format(RedissCacheKey.OtherOrderInfo, //同步失败，清除缓存内的订单信息信息
                           paramodel.store_info.group.ToString(), paramodel.order_id.ToString()));
                if (bussinessIdstr == null)  //同步失败，之前店铺不存在时，清空店铺缓存，因为数据已经被回滚，但是缓存加上了
                    redis.Delete(string.Format(RedissCacheKey.OtherBusinessIdInfo, paramodel.store_info.group.ToString(), paramodel.store_info.store_id.ToString()));
                throw ex; //异常上抛
            }

            return string.IsNullOrWhiteSpace(orderNo) ? ResultModel<object>.Conclude(OrderApiStatusType.ParaError) :
             ResultModel<object>.Conclude(OrderApiStatusType.Success, new { order_no = orderNo });
        }

        /// <summary>
        /// 插入订单时，操作商户商户金额相关操作 
        /// </summary>
        /// <AddBy>caoheyang 20150522</AddBy>
        /// <param name="order"></param>
        private void InsertOrderOptRecord(order order)
        {
            //扣商户金额
            _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
            {
                Id = Convert.ToInt32(order.businessId),
                Money = -order.SettleMoney
            });
            #region 商户余额流水操作
            _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
            {
                BusinessId = Convert.ToInt32(order.businessId),
                Amount = -order.SettleMoney,
                Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                RecordType = (int)BusinessBalanceRecordRecordType.PublishOrder,
                Operator = "系统",
                WithwardId=order.Id,
                RelationNo=order.OrderNo,
                Remark = "商户发单，系统自动扣商家结算费"
            });
            #endregion

            if (order.Adjustment > 0)
            {
                bool b = orderDao.addOrderSubsidiesLog(order.Adjustment, order.Id, "补贴加钱,订单金额:" + order.Amount + "-佣金补贴策略id:" + order.CommissionFormulaMode);
            }
        }

        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="originalOrderNo">第三方平台订单号码</param>
        /// <param name="orderfrom">订单来源</param>
        /// <returns>订单状态</returns>
        public int GetStatus(string originalOrderNo, int orderfrom)
        {
            OrderDao OrderDao = new OrderDao();
            return OrderDao.GetStatus(originalOrderNo, orderfrom);
        }


        /// <summary>
        /// 查看订单详情接口  add by caoheyang 20150325
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        public ResultModel<object> OrderDetail(OrderDetailPM_OpenApi paramodel)
        {
            OrderDao OrderDao = new OrderDao();
            OrderListModel order = OrderDao.GetOpenOrder(paramodel.order_no, paramodel.orderfrom);
            return order == null ? ResultModel<object>.Conclude(OrderApiStatusType.ParaError) :
                ResultModel<object>.Conclude(OrderApiStatusType.Success, new
                {
                    orderinfo = new { order_status = order.Status, clientername = order.ClienterName, clienterphoneno = order.ClienterPhoneNo }
                });
        }




        /// <summary>
        ///  supermanapi通过openapi同步第三方订单状态  add by caoheyang 20150327 
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        public bool AsyncOrderStatus(string orderNo)
        {
            OrderListModel orderlistModel = orderDao.GetOrderByNo(orderNo);
            if (orderlistModel == null) return false;
            if (orderlistModel.OrderFrom > 0)   //一个商户对应多个集团时需要更改 
            {
                ParaModel<AsyncStatusPM_OpenApi> paramodel = new ParaModel<AsyncStatusPM_OpenApi>() { fields = new AsyncStatusPM_OpenApi() { orderfrom = orderlistModel.OrderFrom } };
                if (paramodel.GetSign() == null)//为当前集团参数实体生成sign签名信息
                    return false;
                paramodel.fields.status = ParseHelper.ToInt(orderlistModel.Status, -1);
                paramodel.fields.ClienterTrueName = orderlistModel.ClienterTrueName;
                paramodel.fields.ClienterPhoneNo = orderlistModel.ClienterPhoneNo;
                paramodel.fields.BusinessName = orderlistModel.BusinessName;
                paramodel.fields.OriginalOrderNo = orderlistModel.OriginalOrderNo;
                paramodel.fields.order_no = orderlistModel.OrderNo;
                paramodel.fields.orderfrom = orderlistModel.OrderFrom;
                paramodel.fields.OtherCancelReason = orderlistModel.OtherCancelReason;

                string url = ConfigurationManager.AppSettings["AsyncStatus"];
                string json = new HttpClient().PostAsJsonAsync(url, paramodel).Result.Content.ReadAsStringAsync().Result;
                LogHelper.LogWriter("调用第三方接口同步状态:", new { url = url, paramodel = paramodel, result = json });
                JObject jobject = JObject.Parse(json);
                return jobject.Value<int>("Status") == 0; //接口调用状态 区分大小写  
            }
            return true;
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
            PageInfo<OrderCountModel> pageinfo = orderDao.GetOrderCount<OrderCountModel>(criteria);
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
            PageInfo<HomeCountTitleModel> pageinfo = orderDao.GetCurrentDateCountAndMoney<HomeCountTitleModel>(criteria);
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
                //商户必须是审核通过的， 商户审核通过 意味着 已经设置结算比例， 因为在后台管理系统中 商户审核通过时会验证商户结算比例是否设置
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
                if (order.Status == ConstValues.ORDER_CANCEL)    // 在存在订单的情况下如果是去掉订单的状态，直接修改为订单待接单状态
                {
                    int upResult = orderDao.UpdateOrderStatus_Other(new ChangeStatusPM_OpenApi() { groupid = model.OrderFrom, order_no = order.OriginalOrderNo, orderfrom = model.OrderFrom, remark = "第三方再次推送", status = 0 });
                    if (upResult > 0)
                    {
                        return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Success);
                    }
                    else
                    {
                        return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Failed);
                    }
                }
                else
                {
                    return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OrderHadExist);
                }
            }
            #region 转换省市区
            //转换省
            var _province = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.Receive_Province, JiBie = 1 });
            if (_province != null)
            {
                model.Receive_ProvinceCode = _province.NationalCode.ToString();
            }
            //转换市
            var _city = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.Receive_City, JiBie = 2 });
            if (_city != null)
            {

                model.Receive_CityCode = _city.NationalCode.ToString();
            }
            //转换区
            var _area = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.Receive_Area, JiBie = 3 });
            if (_area != null)
            {
                model.Receive_AreaCode = _area.NationalCode.ToString();
            }
            #endregion
            order dborder = OrderInstance(model);  //整合订单信息
            PubOrderStatus addResult = AddOrder(dborder);    //添加订单记录，并且触发极光推送。          
            if (addResult == PubOrderStatus.Success)
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
        /// 订单详情接口
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单详情</returns>
        public ListOrderDetailModel GetOrderDetail(string order_no)
        {
            ListOrderDetailModel mo = new ListOrderDetailModel();
            var order = orderDao.GetOrderByNo(order_no);
            if (order != null)
            {
                if (order.OrderFrom == 0)
                    order.OrderFromName = "B端";
                else if (order.OrderFrom == 1)
                    order.OrderFromName = "易淘食";
                else if (order.OrderFrom == 2)
                    order.OrderFromName = "万达";
                else if (order.OrderFrom == 3)
                    order.OrderFromName = "全时";
                else if (order.OrderFrom == 4)
                    order.OrderFromName = "美团";
                var list = orderDao.GetOrderDetail(order_no);
                mo.order = order;
                mo.orderDetails = list;
            }
            return mo;
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
            to.Amount = from.Amount - from.DistribSubsidy;//订单金额=聚网客总金额-外送费

            to.OrderType = from.OrderType; //订单类型 1送餐订单 2取餐盒订单 
            to.KM = from.KM; //送餐距离

            to.GuoJuQty = from.GuoJuQty; //锅具数量
            to.LuJuQty = from.LuJuQty;  //炉具数量

            to.DistribSubsidy = from.DistribSubsidy; //外送费
            to.OrderCount = from.OrderCount == 0 ? 1 : from.OrderCount; //订单数量
            //计算订单佣金

            //必须写to.DistribSubsidy ，防止bussiness为空情况
            OrderCommission orderComm = new OrderCommission()
            {
                Amount = from.Amount - from.DistribSubsidy, /*订单金额*/
                DistribSubsidy = from.DistribSubsidy,/*外送费*/
                OrderCount = to.OrderCount/*订单数量*/,
                BusinessCommission = to.BusinessCommission, /*商户结算比例*/
                BusinessGroupId = business.BusinessGroupId,
                StrategyId = business.StrategyId
            };
            OrderPriceProvider commProvider = CommissionFactory.GetCommission(business.StrategyId);
            to.CommissionFormulaMode = business.StrategyId;
            to.CommissionRate = commProvider.GetCommissionRate(orderComm); //佣金比例 
            to.OrderCommission = commProvider.GetCurrenOrderCommission(orderComm); //订单佣金
            to.WebsiteSubsidy = commProvider.GetOrderWebSubsidy(orderComm);//网站补贴
            to.SettleMoney = commProvider.GetSettleMoney(orderComm);//订单结算金额


            to.CommissionFormulaMode = business.StrategyId;
            to.Adjustment = commProvider.GetAdjustment(orderComm);//订单额外补贴金额

            to.Status = ConstValues.ORDER_NEW;

            return to;
        }
        /// <summary>
        /// 根据订单号 获取订单信息
        /// wc
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderInfoByOrderNo(string orderNo, int orderId = 0)
        {
            return orderDao.GetOrderInfoByOrderNo(orderNo);
        }

        #region 旧方法已弃用
        ///// <summary>
        ///// 通过订单号取消订单
        ///// danny-20150414
        ///// </summary>
        ///// <returns></returns>
        //public bool CancelOrderByOrderNo(OrderOptionModel orderOptionModel)
        //{
        //    bool result = false;
        //    var orderModel = orderDao.GetOrderByNo(orderOptionModel.OrderNo);
        //    if (orderModel != null)
        //    {
        //        //如果是已取消
        //        if (orderModel.Status == 3)
        //        {
        //            return true;
        //        }
        //        #region 判断到底扣不扣钱

        //        ETS.NoSql.RedisCache.RedisCache redisCache = new ETS.NoSql.RedisCache.RedisCache();
        //        string orderKey = string.Format(RedissCacheKey.CheckOrderPay, orderOptionModel.OrderNo);
        //        string CheckOrderPay = redisCache.Get<string>(orderKey);

        //        #endregion

        //        //如果订单状态是待接单|已接单|已完成+未上传完小票。则直接取消订单
        //        using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
        //        {
        //            result = orderDao.CancelOrder(orderModel, orderOptionModel);
        //            if (result && orderModel.Status == 1 && orderModel.HadUploadCount == orderModel.NeedUploadCount && CheckOrderPay == "1")
        //            {
        //                //需要上传的小票大于等于总数量+订单已完成则要扣钱
        //                //(因为订单小票有可能不传。所以用的是订单数量和需要上传小票数量对比判断)
        //                result = orderDao.UpdateAccountBalanceByClienterId(orderModel, orderOptionModel);
        //            }
        //            if (result)
        //            {
        //                tran.Complete();
        //            }
        //        }
        //        if (result)
        //        {
        //            AsyncOrderStatus(orderModel.OrderNo);
        //        }
        //    }
        //    return result;
        //}
        #endregion

        /// <summary>
        /// 通过订单号取消订单（新）
        /// danny-20150419
        /// </summary>
        /// <returns></returns>
        public DealResultInfo CancelOrderByOrderNo(OrderOptionModel orderOptionModel)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag =false
            };
            var orderModel = orderDao.GetOrderByNo(orderOptionModel.OrderNo);
            if (orderModel == null)
            {
                dealResultInfo.DealMsg = "未查询到订单信息！";
                return dealResultInfo;
            }
            orderModel.OptUserName = orderOptionModel.OptUserName;
            orderModel.Remark = "管理后台取消订单："+orderOptionModel.OptLog;
            var orderTaskPayStatus = orderDao.GetOrderTaskPayStatus(orderModel.Id);
            #region 订单不可取消
            if (orderModel.Status == 3)//订单已为取消状态
            {
                dealResultInfo.DealMsg = "订单已为取消状态，不能再次取消操作！";
                return dealResultInfo;
            }
            if (orderModel.IsJoinWithdraw == 1 )//订单已分账
            {
                dealResultInfo.DealMsg = "订单已分账，不能取消订单！";
                return dealResultInfo;
            }
            if (orderModel.MealsSettleMode == 1 && orderTaskPayStatus > 0 && !orderModel.IsPay.Value)//餐费未线上支付模式并且餐费有支付
            {
                dealResultInfo.DealMsg = "餐费有支付，不能取消订单！";
                return dealResultInfo;
            }
            #endregion
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (orderDao.CancelOrder(orderModel, orderOptionModel))
                {
                    if (orderModel.Status == 1 && orderTaskPayStatus == 2 && orderModel.HadUploadCount == orderModel.NeedUploadCount)//已完成订单
                    {
                        if (!orderDao.OrderCancelReturnClienter(orderModel))
                        {
                            dealResultInfo.DealMsg = "扣除骑士佣金失败！";
                            return dealResultInfo;
                        }
                    }
                    if (!orderDao.OrderCancelReturnBusiness(orderModel))
                    {
                        dealResultInfo.DealMsg = "商家应收返回失败！";
                        return dealResultInfo;
                    }
                    AsyncOrderStatus(orderModel.OrderNo);
                    dealResultInfo.DealFlag = true;
                    dealResultInfo.DealMsg = "订单取消成功！";
                    tran.Complete();
                    return dealResultInfo;
                }
                dealResultInfo.DealMsg = "订单状态更新失败！";
                return dealResultInfo;
            }
        }
        /// <summary>
        /// 获取订单操作日志
        /// danny-20150414
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public IList<OrderSubsidiesLog> GetOrderOptionLog(int OrderId)
        {
            return orderDao.GetOrderOptionLog(OrderId);
        }


        /// <summary>
        /// 第三方更新E代送订单状态   add by caoheyang 20150421  
        /// 目前该方法仅美团回调取消订单时使用,固定只允许取消订单，后期根据实际需求进行扩展
        /// 可能会挪到具体的集团特有方法中。
        /// </summary>
        /// <param name="paramodel">参数</param>
        /// <returns></returns>
        public ResultModel<object> UpdateOrderStatus_Other(ChangeStatusPM_OpenApi paramodel)
        {
            paramodel.status = OrderConst.OrderStatus3;
            paramodel.remark = "第三方集团取消订单，同步E代送系统订单状态";
            int currenStatus = orderDao.GetStatus(paramodel.order_no, paramodel.orderfrom);  //目前订单状态
            if (currenStatus == -1) //订单不存在
                return ResultModel<object>.Conclude(OrderApiStatusType.OrderNotExist);
            else if (OrderConst.OrderStatus30 != currenStatus
                && OrderConst.OrderStatus0 != currenStatus)  //订单状态非30，,不允许取消订单
                return ResultModel<object>.Conclude(OrderApiStatusType.OrderIsJoin);
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int result = orderDao.UpdateOrderStatus_Other(paramodel);
                if (result > 0)
                {
                    tran.Complete();
                    return ResultModel<object>.Conclude(OrderApiStatusType.Success);
                }
                else
                    return ResultModel<object>.Conclude(OrderApiStatusType.SystemError);
            }
        }

        public IList<OrderRecordsLog> GetOrderRecords(string originalOrderNo, int group)
        {
            return orderDao.GetOrderRecords(originalOrderNo, group);
        }
        /// <summary>
        /// 获取订单拒绝原因
        /// 平扬-20150424
        /// </summary>
        /// <returns></returns>
        public string OtherOrderCancelReasons()
        {
            return ETS.Config.OrderCancelReasons;
        }

        public string CanOrder(string originalOrderNo, int group)
        {
            var order = orderDao.GetOrderByOrderNoAndOrderFrom(originalOrderNo, group, 0);
            //if (order.Status == OrderConst.ORDER_CANCEL)
            //{
            //    return "订单已经取消";
            //}
            if (order.Status == OrderConst.ORDER_NEW)
            {
                var k = orderDao.CancelOrderStatus(order.OrderNo, ConstValues.ORDER_CANCEL, "第三方取消订单", null);
                if (k > 0)
                {
                    return "1"; //取消成功
                }
                else
                {
                    return "取消失败";  //取消失败
                }
            }
            else
            {
                return "订单已被抢无法取消";
            }

        }

        /// <summary>
        /// 判断订单是否存在      
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            return orderDao.IsExist(id);
        }

        /// <summary>
        /// 获取主订单信息
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderPM">获取订单详情参数实体</param>
        /// <returns></returns>
        public order GetById(OrderPM orderPM)
        {
            return orderDao.GetById(orderPM);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="id">订单查询实体</param>
        /// <returns></returns>
        public OrderDM GetDetails(OrderPM modelPM)
        {
            OrderDM orderDM = new OrderDM();

            int id = modelPM.OrderId;
            order order = GetById(modelPM);
            orderDM.Id = order.Id;
            orderDM.OrderNo = order.OrderNo;
            orderDM.OriginalOrderNo = order.OriginalOrderNo;
            orderDM.OrderFrom = order.OrderFrom;
            orderDM.OrderCommission = order.OrderCommission;
            orderDM.PubDate = ParseHelper.ToDatetime(order.PubDate, DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            orderDM.businessName = order.BusinessName;
            orderDM.pickUpCity = order.PickUpCity;
            orderDM.PickUpAddress = order.PickUpAddress;
            orderDM.businessPhone = order.BusinessPhone;
            orderDM.BusinessAddress = order.BusinessAddress;
            orderDM.ReceviceName = order.ReceviceName;
            orderDM.receviceCity = order.ReceviceCity;
            orderDM.RecevicePhoneNo = order.RecevicePhoneNo;
            orderDM.ReceviceAddress = order.ReceviceAddress;
            orderDM.Amount = order.Amount;
            orderDM.IsPay = Convert.ToBoolean(order.IsPay);
            orderDM.Remark = order.Remark;
            orderDM.Status = order.Status;
            orderDM.OrderCount = order.OrderCount;
            orderDM.GroupId = order.GroupId;
            orderDM.PickupCode = order.PickupCode;
            orderDM.Payment = order.Payment;
            orderDM.Invoice = order.Invoice;
            orderDM.NeedUploadCount = order.NeedUploadCount;
            orderDM.HadUploadCount = order.HadUploadCount;
            orderDM.TotalDistribSubsidy = order.TotalDistribSubsidy;
            orderDM.ClienterName = order.ClienterName;
            orderDM.ClienterPhoneNo = order.ClienterPhoneNo;
            orderDM.GrabTime = order.GrabTime;
            orderDM.businessId = ParseHelper.ToInt(order.businessId, 0);
            orderDM.TotalAmount = order.TotalAmount;
            orderDM.MealsSettleMode = order.MealsSettleMode;
            orderDM.ClienterId = ParseHelper.ToInt(order.clienterId);
             
            #region 是否允许修改小票
            orderDM.IsModifyTicket = true;
            if (order.HadUploadCount >= order.OrderCount && order.Status == OrderStatus.订单完成.GetHashCode())
            {
                orderDM.IsModifyTicket = false;
            }
            #endregion

            orderDM.distance =order.distance==-1?"--":  order.distance > 1000 ? Math.Round(order.distance * 0.001, 2) + "Km" : Math.Round(order.distance , 0)+"m";


            OrderChildProvider orderChildPr = new OrderChildProvider();
            List<OrderChildInfo> listOrderChildInfo = orderChildPr.GetByOrderId(id);
            orderDM.listOrderChild = listOrderChildInfo;

            OrderDetailProvider orderDetailPr = new OrderDetailProvider();
            orderDM.listOrderDetail = orderDetailPr.GetByOrderNo(order.OrderNo);


            
            bool IsExistsUnFinish = true;//默认是存在有未支付订单
            if (ParseHelper.ToBool(order.IsPay, false) || order.MealsSettleMode == MealsSettleMode.Status0.GetHashCode())  //线下
            {
                IsExistsUnFinish = false;//如果主任务是顾客已支付，就视认为没有未支付的订单
            }
            else
            {               
               IsExistsUnFinish = listOrderChildInfo.Exists(t => t.PayStatus == PayStatusEnum.WaitPay.GetHashCode());//如果顾客没支付，查询子订单是否有未支付子订单
            }
            orderDM.IsExistsUnFinish = IsExistsUnFinish;

            return orderDM;
        }

        /// <summary>
        /// 计算经纬度
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150515</UpdateTime>
        /// <param name="id">订单动态实体、查询实体、订单实体</param>
        /// <returns></returns>
        void CalculationLgAndLa(OrderDM orderDM, OrderPM modelPM, order order)
        {
            if (order.Longitude == null || order.Longitude == 0 || order.Latitude == null || order.Latitude == 0)
            {
                orderDM.distance = "--";
                orderDM.distanceB2R = "--";
                orderDM.distance_OrderBy = 9999999.0;
            }
            else
            {
                if (modelPM.longitude == 0 || modelPM.latitude == 0 || order.businessId <= 0)
                { orderDM.distance = "--"; orderDM.distance_OrderBy = 9999999.0; }
                else if (order.businessId > 0)  //计算超人当前到商户的距离
                {
                    Degree degree1 = new Degree(modelPM.longitude, modelPM.latitude);   //超人当前的经纬度
                    Degree degree2 = new Degree(order.Longitude.Value, order.Latitude.Value); //商户经纬度
                    var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                    orderDM.distance = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                    orderDM.distance_OrderBy = res;
                }
                if (order.businessId > 0 && order.ReceviceLongitude != null && order.ReceviceLatitude != null
                    && order.ReceviceLongitude != 0 && order.ReceviceLatitude != 0)  //计算商户到收货人的距离
                {
                    Degree degree1 = new Degree(order.Longitude.Value, order.Latitude.Value);  //商户经纬度
                    Degree degree2 = new Degree(order.ReceviceLongitude.Value, order.ReceviceLatitude.Value);  //收货人经纬度
                    var res = ParseHelper.ToDouble(CoordDispose.GetDistanceGoogle(degree1, degree2));
                    orderDM.distanceB2R = res < 1000 ? (Math.Round(res).ToString() + "米") : ((res / 1000).ToString("f2") + "公里");
                }
                else
                    orderDM.distanceB2R = "--";
            }
        }


        /// <summary>
        /// 骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        public ResultModel<object> GetJobC(GetJobCPM model)
        {
            model.TopNum = GlobalConfigDao.GlobalConfigGet(0).ClienterOrderPageSize;// top 值
            model.PushRadius = GlobalConfigDao.GlobalConfigGet(0).PushRadius; //距离
            return ResultModel<object>.Conclude(SystemEnum.Success, orderDao.GetJobC(model));
        }


        public int GetOrderStatus(int orderId, int businessId)
        {
            return orderDao.GetOrderStatus(orderId, businessId);
        }

        public void UpdateTake(OrderPM modelPM)
        {
            float takeLongitude = (float)modelPM.longitude;
            float takeLatitude = (float)modelPM.latitude;
            orderDao.UpdateTake(modelPM.OrderId, modelPM.ClienterId, takeLongitude, takeLatitude);
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150520</UpdateTime>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetStatus(int id)
        {
            return orderDao.GetStatus(id);
        }


        /// <summary>
        /// 商户端 取消订单  商户端只能取消 待接单的订单  add by caoehyang  20150521 
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns></returns>
        public ResultModel<bool> CancelOrderB(CancelOrderBPM paramodel)
        {
           
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                order order = new order();
                CancelOrderStatus tempresult = CheckCancelOrderB(paramodel, ref order);
                if (tempresult != CancelOrderStatus.Success)
                {
                    return ResultModel<bool>.Conclude(tempresult, true); 
                }
                int result = orderDao.CancelOrderStatus(paramodel.OrderNo, OrderConst.OrderStatus3, "商家取消订单", OrderConst.OrderStatus0, order.SettleMoney);
                if (result > 0 & AsyncOrderStatus(paramodel.OrderNo))
                {
                    BusinessDao businessDao = new BusinessDao();
                    businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                    {
                        Id = paramodel.BusinessId,
                        Money = order.SettleMoney
                    });
                    BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();
                    businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                    {
                        BusinessId = paramodel.BusinessId,//商户Id
                        Amount = order.SettleMoney,//流水金额  结算金额
                        Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                        RecordType = (int)BusinessBalanceRecordRecordType.CancelOrder,
                        Operator = string.Format("商家:{0}",paramodel.BusinessId),
                        WithwardId = paramodel.OrderId,
                        RelationNo = paramodel.OrderNo,
                        Remark = "商户取消订单返回配送费"
                    });
                    tran.Complete();
                    return ResultModel<bool>.Conclude(CancelOrderStatus.Success, true);
                }
                else
                {
                    return ResultModel<bool>.Conclude(CancelOrderStatus.CancelOrderError, true);
                }
            }
        }

        /// <summary>
        /// 商户端 取消订单数据验证  add by caoehyang  20150521 
        /// </summary>
        /// <param name="paramodel">参数</param>
        /// <param name="order">订单实体 ref</param>
        /// <returns></returns>
        private CancelOrderStatus CheckCancelOrderB(CancelOrderBPM paramodel,ref order order)
        {
            if (paramodel.OrderId <= 0 || string.IsNullOrWhiteSpace(paramodel.OrderNo))
            {
                return CancelOrderStatus.OrderEmpty;
            }
            else if (string.IsNullOrWhiteSpace(paramodel.Version))
            {
                return CancelOrderStatus.VersionError;
            }
            order = orderDao.GetOrderById(paramodel.OrderId,paramodel.BusinessId,OrderConst.OrderStatus0);
            if (order == null)
            {
                return CancelOrderStatus.CancelOrderError;
            }
            return CancelOrderStatus.Success;
        }
    }
}
