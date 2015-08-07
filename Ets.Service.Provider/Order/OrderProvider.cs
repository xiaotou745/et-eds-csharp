#region
using CalculateCommon;
using Ets.Dao.Finance;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Finance;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.Provider.MyPush;
using Ets.Service.Provider.Subsidy;
using ETS.Enums;
using ETS.Data.PageData;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Ets.Model.DomainModel.Area;
using System.Data;
using Ets.Dao.Clienter;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Ets.Dao.Business;
using Ets.Model.ParameterModel.WtihdrawRecords;
#endregion
namespace Ets.Service.Provider.Order
{
    public class OrderProvider : IOrderProvider
    {
        private OrderDao orderDao = new OrderDao();
        private IBusinessProvider iBusinessProvider = new BusinessProvider();
        private ClienterProvider iClienterProvider = new ClienterProvider();
        readonly OrderOtherDao orderOtherDao = new OrderOtherDao();
        private ISubsidyProvider iSubsidyProvider = new SubsidyProvider();
        private IBusinessGroupProvider iBusinessGroupProvider = new BusinessGroupProvider();
        private readonly BusinessDao _businessDao = new BusinessDao();
        private readonly ClienterDao clienterDao = new ClienterDao();
        ClienterBalanceRecordDao clienterBalanceRecordDao=new ClienterBalanceRecordDao();
        ClienterAllowWithdrawRecordDao  clienterAllowWithdrawRecordDao=new ClienterAllowWithdrawRecordDao();

        private readonly BusinessBalanceRecordDao _businessBalanceRecordDao = new BusinessBalanceRecordDao();
        ClienterFinanceDao clienterFinanceDao = new ClienterFinanceDao();
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
                    resultModel.businessPhone2 = from.BusinessPhone2;
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
                    if (from.GroupId == GroupType.Group3.GetHashCode()) //全时 需要做验证码验证
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
                    resultModel.businessPhone2 = from.BusinessPhone2;
                    resultModel.HadUploadCount = from.HadUploadCount;
                    resultModel.GroupId = from.GroupId;
                    if (from.GroupId == GroupType.Group3.GetHashCode())
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

        /// <summary>
        /// 转换B端发布的订单信息为 数据库中需要的 订单 数据
        /// </summary>
        /// <param name="busiOrderInfoModel"></param>
        /// <param name="business">返回商户信息</param>
        /// <returns></returns>
        public order TranslateOrder(Ets.Model.ParameterModel.Business.BussinessOrderInfoPM busiOrderInfoModel, out BusListResultModel business)
        {
            order to = new order();
            ///TODO 订单号生成规则，定了以后要改；
            to.OrderNo = Helper.generateOrderCode(busiOrderInfoModel.userId, busiOrderInfoModel.TimeSpan);  //根据userId生成订单号(15位)
            to.businessId = busiOrderInfoModel.userId; //当前发布者
            business = iBusinessProvider.GetBusiness(busiOrderInfoModel.userId);
            if (business != null)
            {
                to.PickUpCity = business.City;  //商户所在城市
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                to.ReceviceCity = business.City; //城市
                to.DistribSubsidy = business.DistribSubsidy.HasValue ? business.DistribSubsidy : 0; ;//设置外送费,从商户中找。
                to.BusinessCommission = ParseHelper.ToDecimal(business.BusinessCommission);//商户结算比例
                to.BusinessName = business.Name;
                to.CommissionType = business.CommissionType;//结算类型：1：固定比例 2：固定金额
                to.CommissionFixValue = ParseHelper.ToDecimal(business.CommissionFixValue);//固定金额     
                to.BusinessGroupId = business.BusinessGroupId;
                to.MealsSettleMode = business.MealsSettleMode;
                to.OneKeyPubOrder = business.OneKeyPubOrder;
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
            to.Status = Convert.ToByte(OrderStatus.Status0.GetHashCode());
            to.TimeSpan = busiOrderInfoModel.TimeSpan;
            to.listOrderChild = busiOrderInfoModel.listOrderChlid;

            if (!(bool)to.IsPay && to.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())//未付款且线上支付
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
                    Operator = order.BusinessName,
                    Remark = "扣除商家结算费",
                    WithwardId = result,
                    RelationNo = order.OrderNo
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
                //异步回调第三方，推送通知
                Task.Factory.StartNew(() =>
                {
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", order.businessId.ToString(), string.Empty);
                });
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
        /// 第三方订单列表根据订单号 修改订单状态   平杨  TODO 目前支适用于美团
        /// </summary>
        ///  <UpdateBy>确认接入时扣除商家结算费功能  caoheyang 20150526</UpdateBy>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">目标订单状态</param>
        /// <param name="remark"></param>
        /// <param name="status">原始订单状态</param>
        /// <returns></returns>
        public int UpdateOrderStatus(string orderNo, int orderStatus, string remark, int? status)
        {
            int reurnRes = 0;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                OrderListModel order = orderDao.GetOrderByNo(orderNo);
                if (order == null || order.Status != status)  //当前订单状态与原始订单状态不一致 返回0
                {
                    return 0;
                }
                int result = orderDao.CancelOrderStatus(orderNo, orderStatus, remark, status);
                if (result > 0)
                {
                    //确认接入订单时   扣除 商家结算费 
                    if (orderStatus == OrderStatus.Status0.GetHashCode())
                    {
                        //扣商户金额
                        _businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                        {
                            Id = Convert.ToInt32(order.businessId),
                            Money = -order.SettleMoney
                        });
                        _businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                        {
                            BusinessId = Convert.ToInt32(order.businessId),
                            Amount = -order.SettleMoney,
                            Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                            RecordType = (int)BusinessBalanceRecordRecordType.PublishOrder,
                            Operator = order.BusinessName,
                            WithwardId = order.Id,
                            RelationNo = order.OrderNo,
                            Remark = "商户发单，系统自动扣商家结算费"
                        });
                    }
                    reurnRes = 1;
                    tran.Complete();
                }
            }
            //异步回调第三方，推送通知
            Task.Factory.StartNew(() =>
            {
                if (reurnRes > 0)
                {
                    AsyncOrderStatus(orderNo);
                }
            });
            return reurnRes;
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

                paramodel.businessId = orderDao.CreateToSqlAddBusiness(paramodel);  //商户id
            }
            else
                paramodel.businessId = ParseHelper.ToInt(bussinessIdstr);
            #endregion
            BusListResultModel business = iBusinessProvider.GetBusiness(paramodel.businessId);
            if (business != null)
            {
                paramodel.CommissionType = business.CommissionType;//结算类型：1：固定比例 2：固定金额
                paramodel.CommissionFixValue = business.CommissionFixValue;//固定金额     
                paramodel.BusinessGroupId = business.BusinessGroupId;
                paramodel.MealsSettleMode = business.MealsSettleMode;
            }


            #region 根据集团id为店铺设置外送费，结算比例等财务相关信息add by caoheyang 20150417

            //此处其实应该取数据库，但是由于发布订单时关于店铺的逻辑后期要改，暂时这么处理 
            IGroupProviderOpenApi groupProvider = OpenApiGroupFactory.Create(paramodel.store_info.group);
            paramodel = groupProvider.SetCommissonInfo(paramodel);

            #endregion

            #region 佣金相关  add by caoheyang 20150416
            paramodel.CommissionFormulaMode = ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet(1).CommissionFormulaMode);//默认第三方策略，是默认组下的默认策略，目前即是普通策略 
            //paramodel.CommissionType = 1;//结算类型：1：固定比例 2：固定金额
            //paramodel.CommissionFixValue = 0;//固定金额
            //paramodel.BusinessGroupId = 1;//分组ID

            //计算获得订单骑士佣金
            OrderCommission orderComm = new OrderCommission()
            {
                Amount = paramodel.total_price, /*订单金额*/
                DistribSubsidy = paramodel.store_info.delivery_fee,/*外送费*/
                OrderCount = paramodel.package_count ?? 1,/*订单数量，默认为1*/
                BusinessCommission = paramodel.store_info.businesscommission,/*商户结算比例*/
                BusinessGroupId = paramodel.BusinessGroupId,
                StrategyId = business.StrategyId,
                OrderWebSubsidy = paramodel.websitesubsidy

            }/*网站补贴*/;
            OrderPriceProvider commissonPro = CommissionFactory.GetCommission(0);//万达、全时采用默认分组下策略
            paramodel.ordercommission = commissonPro.GetCurrenOrderCommission(orderComm);  //骑士佣金
            paramodel.websitesubsidy = commissonPro.GetOrderWebSubsidy(orderComm);//网站补贴
            paramodel.commissionrate = commissonPro.GetCommissionRate(orderComm);//订单佣金比例
            paramodel.settlemoney = commissonPro.GetSettleMoney(orderComm);//订单结算金额
            paramodel.adjustment = commissonPro.GetAdjustment(orderComm);//订单额外补贴金额

            #endregion
            if (!(bool)paramodel.is_pay && paramodel.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())//未付款且线上支付
            {
                //paramodel.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(paramodel.total_price) +
                //               ParseHelper.ToDecimal(paramodel.store_info.delivery_fee) * ParseHelper.ToInt(paramodel.package_count), 2);
                //只有一个子订单
                paramodel.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(paramodel.total_price) +
                              ParseHelper.ToDecimal(paramodel.store_info.delivery_fee), 2);
            }

            string orderNo = null; //订单号码
            try
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    if (bussinessIdstr == null)
                    {

                    }
                    //

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
                        SettleMoney = paramodel.settlemoney,
                        Adjustment = paramodel.adjustment,
                        Id = orderId,
                        BusinessName = paramodel.store_info.store_name,
                        Amount = paramodel.total_price,
                        OrderNo = orderNo,
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
                WithwardId = order.Id,
                RelationNo = order.OrderNo,
                Remark = "商户发单，系统自动扣商家结算费"
            });
            #endregion

            //
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
        /// <param name="orderNo">参数实体</param>
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
        public Ets.Model.Common.ResultModel<NewPostPublishOrderResultModel> NewPostPublishOrder_B(NewPostPublishOrderModel model)
        {
            try
            {

                #region 转换省市区
                //转换省
                var _province = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = model.Receive_Province, JiBie = 1 });
                if (_province != null)
                {
                    model.Receive_ProvinceCode = _province.NationalCode.ToString();
                }
                string cityName = model.Receive_City;
                model.Receive_City = string.IsNullOrEmpty(cityName) ? string.Empty : cityName.Trim().Replace("城区", "市").Replace("郊区", "市");
                //转换市
                var _city = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = model.Receive_City, JiBie = 2 });
                if (_city != null)
                {

                    model.Receive_CityCode = _city.NationalCode.ToString();
                }
                //转换区
                var _area = iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate() { Name = model.Receive_Area, JiBie = 3 });
                if (_area != null)
                {
                    model.Receive_AreaCode = _area.NationalCode.ToString();
                }
                #endregion
                List<OrderChlidPM> list = new List<OrderChlidPM>
                {
                    new OrderChlidPM { ChildId=1,GoodPrice=model.Amount- ParseHelper.ToDecimal(model.DistribSubsidy, 0) }
                };
                model.listOrderChlid = list;

                ResultModel<NewPostPublishOrderResultModel> currResModel = Verification(model);
                if (currResModel.Status == OrderPublicshStatus.VerificationSuccess.GetHashCode())
                {
                    order dborder = OrderInstance(model);  //整合订单信息 
                    PubOrderStatus addResult = AddOrder(dborder);    //添加订单记录，并且触发极光推送。          
                    if (addResult == PubOrderStatus.Success)
                    {
                        NewPostPublishOrderResultModel resultModel = new NewPostPublishOrderResultModel { OriginalOrderNo = model.OriginalOrderNo, OrderNo = dborder.OrderNo };
                        LogHelper.LogWriter("订单发布成功", new { model = model, resultModel = resultModel });
                        return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Success, resultModel);
                    }
                }

                return currResModel;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<NewPostPublishOrderResultModel> NewPostPublishOrder_B()方法出错", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Failed);
            }

        }
        ResultModel<NewPostPublishOrderResultModel> Verification(NewPostPublishOrderModel model)
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

            BusinessDao busDao = new BusinessDao();
            BusinessModel busi = busDao.GetBusiByOriIdAndOrderFrom(model.OriginalBusinessId, model.OrderFrom);
            if (busi == null)
            {
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNoExist);
            }
            else
            {
                //商户必须是审核通过的， 商户审核通过 意味着 已经设置结算比例， 因为在后台管理系统中 商户审核通过时会验证商户结算比例是否设置
                if (busi.Status != (byte)BusinessStatus.Status1.GetHashCode())
                {
                    return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNotAudit);
                }

            }
            //验证该平台 商户 订单号 是否存在
            Ets.Dao.Order.OrderDao orderDao = new Dao.Order.OrderDao();
            var order = orderDao.GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);

            if (order != null)
            {
                if (order.Status == OrderStatus.Status3.GetHashCode())    // 在存在订单的情况下如果是去掉订单的状态，直接修改为订单待接单状态
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

            return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.VerificationSuccess);

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
                //修改该接口时需要把orderfrom的id join出name来，本次为了改一个聚网客居然要单独上线。。。。
                if (order.OrderFrom == 0)
                    order.OrderFromName = "B端";
                else if (order.OrderFrom == 1)
                    order.OrderFromName = "聚网客";
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
            BusinessModel abusiness = businessDao.GetBusiByOriIdAndOrderFrom(from.OriginalBusinessId, from.OrderFrom);

            if (abusiness == null) return null;
            //if (abusiness != null)
            //{
            //    from.BusinessId = abusiness.Id;
            //}
            //else
            //{
            //    return null;
            //}
            to.OrderNo = Helper.generateOrderCode(abusiness.Id);  //根据userId生成订单号(15位)
            to.businessId = abusiness.Id; //当前发布者
            BusListResultModel business = businessDao.GetBusiness(abusiness.Id);  //根据发布者id,获取发布者的相关信息实体
            if (business != null)
            {
                to.PickUpCity = business.City;  //商户所在城市
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                //to.ReceviceCity = business.City; //城市
                //to.DistribSubsidy = business.DistribSubsidy;//设置外送费,从商户中找。
                to.BusinessCommission = ParseHelper.ToDecimal(business.BusinessCommission);//商户结算比例
                to.BusinessName = business.Name;
                to.CommissionType = business.CommissionType;//结算类型：1：固定比例 2：固定金额
                to.CommissionFixValue = ParseHelper.ToDecimal(business.CommissionFixValue);//固定金额     
                to.BusinessGroupId = business.BusinessGroupId;
                to.MealsSettleMode = business.MealsSettleMode;
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
            if (!(bool)to.IsPay && to.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())//未付款且线上支付
            {
                to.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(from.Amount), 2);
            }

            to.CommissionFormulaMode = business.StrategyId;
            to.Adjustment = commProvider.GetAdjustment(orderComm);//订单额外补贴金额

            to.Status = (byte)OrderStatus.Status0.GetHashCode();

            //to.TimeSpan = busiOrderInfoModel.TimeSpan;
            to.listOrderChild = from.listOrderChlid;

            if (!(bool)to.IsPay && to.MealsSettleMode == MealsSettleMode.LineOn.GetHashCode())//未付款且线上支付
            {
                to.BusinessReceivable = Decimal.Round(ParseHelper.ToDecimal(to.Amount) +
                               ParseHelper.ToDecimal(to.DistribSubsidy) * ParseHelper.ToInt(to.OrderCount), 2);
            }
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
                DealFlag = false
            };
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                var orderModel = orderDao.GetOrderByIdWithNolock(orderOptionModel.OrderId);
                if (orderModel == null)
                {
                    dealResultInfo.DealMsg = "未查询到订单信息！";
                    return dealResultInfo;
                }
                orderModel.OptUserName = orderOptionModel.OptUserName;
                orderModel.Remark = "管理后台取消订单：" + orderOptionModel.OptLog;
                var orderTaskPayStatus = orderDao.GetOrderTaskPayStatus(orderModel.Id);
                #region 订单不可取消
                if (orderModel.Status == 3)//订单已为取消状态
                {
                    dealResultInfo.DealMsg = "订单已为取消状态，不能再次取消操作！";
                    return dealResultInfo;
                }
                if (orderModel.IsJoinWithdraw == 1)//订单已分账
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

                if (orderDao.CancelOrder(orderModel, orderOptionModel))
                {
                    if (orderModel.Status == 1 && orderTaskPayStatus == 2 &&
                        orderModel.HadUploadCount == orderModel.NeedUploadCount) //已完成订单
                    {
                        //if (!orderDao.OrderCancelReturnClienter(orderModel))
                        //{
                        //    dealResultInfo.DealMsg = "扣除骑士佣金失败！";
                        //    return dealResultInfo;
                        //}

                        decimal accountBalance = decimal.Parse(clienterDao.GetUserInfoByUserId(orderModel.clienterId).AccountBalance.ToString());
                        decimal allowWithdrawPrice = clienterDao.GetUserInfoByUserId(orderModel.clienterId).AllowWithdrawPrice;

                        clienterFinanceDao.ClienterRecharge(new ClienterOptionLog()
                                    {
                                        RechargeAmount = -orderModel.RealOrderCommission,
                                        ClienterId = orderModel.clienterId
                                    }
                            );

                        ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
                        {
                            ClienterId = orderModel.clienterId,
                            Amount =  -orderModel.RealOrderCommission,
                            Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                            Balance = accountBalance,
                            RecordType = ClienterBalanceRecordRecordType.BalanceAdjustment.GetHashCode(),
                            Operator = orderModel.OptUserName,
                            WithwardId =  orderModel.Id,
                            RelationNo = orderModel.OrderNo,
                            Remark = orderModel.Remark
                        };
                        clienterBalanceRecordDao.Insert(cbrm);

                        ClienterAllowWithdrawRecord cawrm = new ClienterAllowWithdrawRecord()
                        {
                            ClienterId = orderModel.clienterId,
                            Amount =  -orderModel.RealOrderCommission,
                            Status = ClienterAllowWithdrawRecordStatus.Success.GetHashCode(),
                            Balance = allowWithdrawPrice,
                            RecordType = ClienterAllowWithdrawRecordType.BalanceAdjustment.GetHashCode(),
                            Operator = orderModel.OptUserName,
                            WithwardId =  orderModel.Id,
                            RelationNo =orderModel.OrderNo,
                            Remark = orderModel.Remark
                        };
                        clienterAllowWithdrawRecordDao.Insert(cawrm);
                    }
                    if (!orderDao.OrderCancelReturnBusiness(orderModel))
                    {
                        dealResultInfo.DealMsg = "商家应收返回失败！";
                        return dealResultInfo;
                    }
                    dealResultInfo.DealFlag = true;
                    dealResultInfo.DealMsg = "订单取消成功！";
                    tran.Complete();
                }
                else
                {
                    dealResultInfo.DealMsg = "订单状态更新失败！";
                }
                Task.Factory.StartNew(() =>
                {
                    if (dealResultInfo.DealFlag)
                    {
                        AsyncOrderStatus(orderModel.OrderNo);
                    }
                });
                return dealResultInfo;
            }
        }

        /// <summary>
        /// 通过订单号手动扣除订单网站补贴
        /// 彭宜-20150803
        /// </summary>
        /// <returns></returns>
        public DealResultInfo DeductWebSubsidy(OrderOptionModel orderOptionModel)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            if (string.IsNullOrEmpty(orderOptionModel.OptLog))
            {
                dealResultInfo.DealMsg = "请填写扣除网站补贴原因！";
                return dealResultInfo;
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                var orderModel = orderDao.GetOrderByIdWithNolock(orderOptionModel.OrderId);

                #region 点击浏览器后退按钮 
        
                if (orderModel.IsJoinWithdraw == 1)//订单已分账
                {
                    dealResultInfo.DealMsg = "订单已分账，不能审核拒绝！";
                    return dealResultInfo;
                }              
                #endregion


                //如果要扣除的金额大于0， 写流水
                if (orderModel.OrderCommission > orderModel.SettleMoney)
                {
                     ClienterBalanceRecord currModel = clienterBalanceRecordDao.GetByOrderId(orderModel.Id);
                     if (currModel == null)
                     {
                         decimal diffOrderCommission = orderModel.SettleMoney - orderModel.OrderCommission.Value;
                         iClienterProvider.UpdateNotRealOrderClienterAccount(orderModel, diffOrderCommission);
                     }                   
                }

                //更新订单真实佣金
                decimal realOrderCommission = orderModel.OrderCommission.Value;
                realOrderCommission = realOrderCommission > orderModel.SettleMoney ? orderModel.SettleMoney : realOrderCommission;
                orderDao.UpdateOrderRealOrderCommission(orderModel.Id.ToString(), realOrderCommission);

                //加可提现                
                clienterDao.UpdateAllowWithdrawPrice(realOrderCommission, orderModel.clienterId);
                orderDao.UpdateJoinWithdraw(orderModel.Id);
                orderDao.UpdateAuditStatus(orderModel.Id, OrderAuditStatusCommon.Refuse.GetHashCode());

                //加可提现金额
                ClienterAllowWithdrawRecord cawrm = new ClienterAllowWithdrawRecord()
                {
                    ClienterId = orderModel.clienterId,
                    Amount = Convert.ToDecimal(realOrderCommission),
                    Status = ClienterAllowWithdrawRecordStatus.Success.GetHashCode(),
                    RecordType = ClienterAllowWithdrawRecordType.OrderCommission.GetHashCode(),
                    Operator = orderOptionModel.OptUserName,
                    WithwardId = orderModel.Id,
                    RelationNo = orderModel.OrderNo,
                    Remark = "管理后台审核拒绝加可提现"
                };
                clienterAllowWithdrawRecordDao.Insert(cawrm);


                //更新扣除补贴原因,扣除补贴方式为手动扣除
                orderOtherDao.UpdateOrderDeductCommissionReason(orderModel.Id, orderOptionModel.OptLog, 2);
                tran.Complete();
                dealResultInfo.DealFlag = true;
                dealResultInfo.DealMsg = "扣取网站补贴成功！";
                return dealResultInfo;
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public DealResultInfo AuditOK(OrderOptionModel orderOptionModel)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                var orderModel = orderDao.GetOrderByIdWithNolock(orderOptionModel.OrderId);
                if (orderModel.IsJoinWithdraw == 1)//订单已分账
                {
                    dealResultInfo.DealMsg = "订单已分账，不能审核通过！";
                    return dealResultInfo;
                }   

                
               #region 调用老接口
                //无效订单
                if (orderModel.OrderCommission > orderModel.SettleMoney)
                {                    
                    //查询余额流水表中，是否扣除补贴，如果扣除，
                    ClienterBalanceRecord currModel = clienterBalanceRecordDao.GetByOrderId(orderModel.Id);
                    if (currModel != null)
                    {
                        if (orderModel.IsPay.Value || (!orderModel.IsPay.Value && orderModel.MealsSettleMode == MealsSettleMode.LineOff.GetHashCode()))
                        {
                            //加余额，加余额流水
                            UpdateClienterAccount(orderModel,-currModel.Amount);
                        }
                        else
                        {
                            //加余额、可提现、余额流水
                            UpdateAccountBalanceAndWithdraw(orderModel, -currModel.Amount);
                        }                 
                    }
                }
                #endregion

                clienterDao.UpdateAllowWithdrawPrice(Convert.ToDecimal(orderModel.OrderCommission), orderModel.clienterId);
                orderDao.UpdateJoinWithdraw(orderModel.Id);
                orderDao.UpdateAuditStatus(orderModel.Id, OrderAuditStatusCommon.Through.GetHashCode());   

                ClienterAllowWithdrawRecord cawrm = new ClienterAllowWithdrawRecord()
                {
                    ClienterId = orderModel.clienterId,
                    Amount =Convert.ToDecimal(orderModel.OrderCommission),
                    Status = ClienterAllowWithdrawRecordStatus.Success.GetHashCode(),
                    RecordType = ClienterAllowWithdrawRecordType.OrderCommission.GetHashCode(),
                    Operator = orderOptionModel.OptUserName,
                    WithwardId = orderModel.Id,
                    RelationNo = orderModel.OrderNo,
                    Remark = "管理后台审核通过加可提现"
                };
                clienterAllowWithdrawRecordDao.Insert(cawrm);


                tran.Complete();
                dealResultInfo.DealFlag = true;;
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
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                int currenStatus = orderDao.GetStatus(paramodel.order_no, paramodel.orderfrom);  //目前订单状态
                if (currenStatus == -1) //订单不存在
                    return ResultModel<object>.Conclude(OrderApiStatusType.OrderNotExist);
                else if (OrderStatus.Status30.GetHashCode() != currenStatus
                    && OrderStatus.Status0.GetHashCode() != currenStatus)  //订单状态非30，,不允许取消订单
                    return ResultModel<object>.Conclude(OrderApiStatusType.OrderIsJoin);
                int result = orderDao.UpdateOrderStatus_Other(paramodel);  //更新第三方订单在E代送的状态成功
                if (result > 0)
                {
                    //当第三方取消订单时订单的状态为待接单时，需要返还商家结算费 TODO 目前只供美团使用
                    if (currenStatus == OrderStatus.Status0.GetHashCode())
                    {
                        OrderListModel order = orderDao.GetOpenOrder(paramodel.order_no, paramodel.orderfrom);
                        BusinessDao businessDao = new BusinessDao();
                        businessDao.UpdateForWithdrawC(new UpdateForWithdrawPM()
                        {
                            Id = order.businessId,
                            Money = order.SettleMoney
                        });
                        BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();
                        businessBalanceRecordDao.Insert(new BusinessBalanceRecord()
                        {
                            BusinessId = order.businessId,//商户Id
                            Amount = order.SettleMoney,//流水金额  结算金额
                            Status = (int)BusinessBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                            RecordType = (int)BusinessBalanceRecordRecordType.CancelOrder,
                            Operator = order.BusinessName,
                            WithwardId = order.Id,
                            RelationNo = order.OrderNo,
                            Remark = "第三方商户调用接口取消订单返回配送费"
                        });
                    }
                    tran.Complete();
                    return ResultModel<object>.Conclude(OrderApiStatusType.Success);
                }
                else
                {
                    return ResultModel<object>.Conclude(OrderApiStatusType.SystemError);
                }
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
            if (order.Status == OrderStatus.Status0.GetHashCode())
            {
                var k = orderDao.CancelOrderStatus(order.OrderNo, OrderStatus.Status3.GetHashCode(), "第三方取消订单", null);
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
            orderDM.businessPhone2 = order.BusinessPhone2;
            orderDM.Landline = order.Landline;
            orderDM.BusinessAddress = order.BusinessAddress;
            orderDM.ReceviceName = order.ReceviceName;
            orderDM.receviceCity = order.ReceviceCity;
            orderDM.RecevicePhoneNo = order.RecevicePhoneNo;
            if (!string.IsNullOrEmpty(order.ReceviceAddress))
                orderDM.ReceviceAddress = order.ReceviceAddress;
            else
            {
                orderDM.ReceviceAddress = OrderConst.ReceviceAddress;
            }
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
            //orderDM.GrabTime = order.GrabTime;
            orderDM.GrabTime = ParseHelper.ToDatetime(order.GrabTime, DateTime.Now).ToString("yyyy-MM-dd HH:mm");
            orderDM.businessId = ParseHelper.ToInt(order.businessId, 0);
            orderDM.TotalAmount = order.TotalAmount;
            orderDM.MealsSettleMode = order.MealsSettleMode;
            orderDM.Longitude = order.Longitude;
            orderDM.Latitude = order.Latitude;
            orderDM.ClienterId = ParseHelper.ToInt(order.clienterId);
            orderDM.OneKeyPubOrder = order.OneKeyPubOrder;

            #region 是否允许修改小票
            orderDM.IsModifyTicket = true;
            if (order.HadUploadCount >= order.OrderCount && order.Status == OrderStatus.Status1.GetHashCode())
            {
                orderDM.IsModifyTicket = false;
            }
            #endregion

            orderDM.distance = order.distance == -1 ? "--" : order.distance > 1000 ? Math.Round(order.distance * 0.001, 2) + "Km" : Math.Round(order.distance, 0) + "m";


            OrderChildProvider orderChildPr = new OrderChildProvider();
            List<OrderChildInfo> listOrderChildInfo = orderChildPr.GetByOrderId(id);
            orderDM.listOrderChild = listOrderChildInfo;

            OrderDetailProvider orderDetailPr = new OrderDetailProvider();
            orderDM.listOrderDetail = orderDetailPr.GetByOrderNo(order.OrderNo);



            bool IsExistsUnFinish = true;//默认是存在有未支付订单
            if (ParseHelper.ToBool(order.IsPay, false) || order.MealsSettleMode == MealsSettleMode.LineOff.GetHashCode())  //线下
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
            IList<GetJobCDM> jobs = new List<GetJobCDM>();
            model.PushRadius = GlobalConfigDao.GlobalConfigGet(0).PushRadius; //距离
            model.ExclusiveOrderTime = ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet(0).ExclusiveOrderTime); //商家专属骑士接单响应时间

            if (model.ExpressId > 0)
            {
                #region 物流公司逻辑
                if (model.SearchType == (int)GetJobCMode.NewJob)//物流公司全部任务
                {
                    model.TopNum = PageSizeType.App_PageSize.GetHashCode().ToString();//50条
                    jobs = orderDao.GetExpressAllJob(model);
                }
                else if (model.SearchType == (int)GetJobCMode.NearbyJob)//物流公司附近任务
                {
                    model.TopNum = GlobalConfigDao.GlobalConfigGet(0).ClienterOrderPageSize;// top 值
                    jobs = orderDao.GetExpressNearJob(model);
                }
                #endregion
            }
            else
            {
                #region 众包业务逻辑
                if (model.SearchType == (int)GetJobCMode.NewJob)//全部任务
                {
                    model.TopNum = PageSizeType.App_PageSize.GetHashCode().ToString();//50条
                    jobs = orderDao.GetLastedJobC(model);
                }
                else if (model.SearchType == (int)GetJobCMode.NearbyJob)//附近任务
                {
                    model.TopNum = GlobalConfigDao.GlobalConfigGet(0).ClienterOrderPageSize;// top 值
                    jobs = orderDao.GetJobC(model);
                }
                else if (model.SearchType == (int)GetJobCMode.EmployerJob)//店内任务
                {
                    model.TopNum = GlobalConfigDao.GlobalConfigGet(0).ClienterOrderPageSize;// top 值
                    jobs = orderDao.GetEmployerJobC(model);
                }
                #endregion
            }
            return ResultModel<object>.Conclude(SystemState.Success, jobs);
        }

        public int GetOrderStatus(int orderId, int businessId)
        {
            return orderDao.GetOrderStatus(orderId, businessId);
        }

        /// <summary>
        /// 更新取货坐标
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150701</UpdateTime>
        /// <param name="modelPM"></param>
        public void UpdateTake(OrderPM modelPM)
        {
            float takeLongitude = (float)modelPM.longitude;
            float takeLatitude = (float)modelPM.latitude;
            orderDao.UpdateTake(modelPM);
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
                    return ResultModel<bool>.Conclude(tempresult, false);
                }
                int result = orderDao.CancelOrderStatus(paramodel.OrderNo, OrderStatus.Status3.GetHashCode(), "商家取消订单", OrderStatus.Status0.GetHashCode(), order.SettleMoney);
                if (result > 0)
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
                        Operator = string.Format("商家:{0}", paramodel.BusinessId),
                        WithwardId = paramodel.OrderId,
                        RelationNo = paramodel.OrderNo,
                        Remark = "商户取消订单返回配送费"
                    });
                    tran.Complete();
                }
                else
                {
                    return ResultModel<bool>.Conclude(CancelOrderStatus.CancelOrderError, false);
                }
            }
            //异步同步第三方订单状态
            Task.Factory.StartNew(() =>
            {
                AsyncOrderStatus(paramodel.OrderNo);
            });
            return ResultModel<bool>.Conclude(CancelOrderStatus.Success, true);
        }

        /// <summary>
        /// 商户端 取消订单数据验证  add by caoehyang  20150521 
        /// </summary>
        /// <param name="paramodel">参数</param>
        /// <param name="order">订单实体 ref</param>
        /// <returns></returns>
        private CancelOrderStatus CheckCancelOrderB(CancelOrderBPM paramodel, ref order order)
        {
            if (paramodel.OrderId <= 0 || string.IsNullOrWhiteSpace(paramodel.OrderNo))
            {
                return CancelOrderStatus.OrderEmpty;
            }
            else if (string.IsNullOrWhiteSpace(paramodel.Version))
            {
                return CancelOrderStatus.VersionError;
            }
            order = orderDao.GetOrderById(paramodel.OrderId, paramodel.BusinessId, OrderStatus.Status0.GetHashCode());
            if (order == null)
            {
                return CancelOrderStatus.CancelOrderError;
            }
            return CancelOrderStatus.Success;
        }
        /// <summary>
        /// 查询配送数据列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public IList<DistributionAnalyzeResult> DistributionAnalyze(OrderDistributionAnalyze model, int pageIndex, int pageSize, out int totalRows)
        {
            totalRows = 0;
            DataTable dt = orderDao.DistributionAnalyze(model, pageIndex, pageSize, out totalRows);

            List<int> clienterIds = new List<int>();
            List<int> businessIds = new List<int>();
            foreach (DataRow item in dt.Rows)
            {
                int clienterid = ParseHelper.ToInt(item["clienterId"], 0);
                if (clienterid == 0)
                {
                    continue;
                }
                clienterIds.Add(clienterid);

                int businessid = ParseHelper.ToInt(item["businessId"], 0);
                if (businessid == 0)
                {
                    continue;
                }
                businessIds.Add(businessid);
            }
            clienterIds = clienterIds.Distinct().ToList();
            businessIds = businessIds.Distinct().ToList();

            IDictionary<int, BusinessModel> businessDic = _businessDao.GetByIds(businessIds);
            IDictionary<int, clienter> clienterDic = clienterDao.GetByIds(clienterIds);

            IList<DistributionAnalyzeResult> list = new List<DistributionAnalyzeResult>();
            foreach (DataRow item in dt.Rows)
            {
                var data = new DistributionAnalyzeResult()
                {
                    Id = int.Parse(item["Id"].ToString()),
                    OrderNo = item["OrderNo"].ToString(),
                    OrderCount = ParseHelper.ToInt(item["OrderCount"], 0),
                    ActualDoneDate = ParseHelper.ToDatetime(item["ActualDoneDate"]),
                    PubDate = ParseHelper.ToDatetime(item["PubDate"]),
                    GrabTime = ParseHelper.ToDatetime(item["GrabTime"]),
                    PickUpAddress = ParseHelper.ToString(item["PickUpAddress"]),
                    ReceviceAddress = ParseHelper.ToString(item["ReceviceAddress"]),
                    TakeTime = ParseHelper.ToDatetime(item["TakeTime"]),
                    ReceviceCity = item["ReceviceCity"].ToString(),
                    TaskMoney = ParseHelper.ToDecimal(item["OrderCommission"], 0)
                };

                int businessId = ParseHelper.ToInt(item["businessId"]);
                if (businessDic.ContainsKey(businessId))
                {
                    data.Business = businessDic[businessId].Address;
                    data.BusinessPhone = businessDic[businessId].PhoneNo;
                }

                int supperId = ParseHelper.ToInt(item["clienterId"]);

                if (clienterDic.ContainsKey(supperId))
                {
                    data.clienter = clienterDic[supperId].TrueName;
                    data.clienterPhone = clienterDic[supperId].PhoneNo;
                }
                list.Add(data);
            }
            return list;
        }
        /// <summary>
        /// 获得订单去重城市列表
        /// </summary>
        /// <returns></returns>
        public IList<string> OrderReceviceCity()
        {
            return orderDao.OrderReceviceCity();
        }

        /// <summary>
        /// 根据orderID获取订单地图数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderMapDetail GetOrderMapDetail(long orderID)
        {
            OrderMapDetail detail = orderDao.GetOrderMapDetail(orderID);
            if (detail != null)
            {
                DateTime d = DateTime.Parse("1900-01-01");
                if (DateTime.Parse(detail.PubDate) == d)
                {
                    detail.PubDate = "暂无";
                }
                if (DateTime.Parse(detail.GrabTime) == d)
                {
                    detail.GrabTime = "暂无";
                }
                if (DateTime.Parse(detail.ActualDoneDate) == d)
                {
                    detail.ActualDoneDate = "暂无";
                }
                if (DateTime.Parse(detail.TakeTime) == d)
                {
                    detail.TakeTime = "暂无";
                }
                #region 如果抢单，取货，完成地点的经度或纬度为0，则其经纬度都取发单经纬度
                if (detail.GrabLatitude == 0 || detail.GrabLongitude == 0)
                {
                    detail.GrabLongitude = detail.PubLongitude;
                    detail.GrabLatitude = detail.PubLatitude;
                }
                if (detail.TakeLatitude == 0 || detail.TakeLongitude == 0)
                {
                    detail.TakeLongitude = detail.PubLongitude;
                    detail.TakeLatitude = detail.PubLatitude;
                }
                if (detail.CompleteLatitude == 0 || detail.CompleteLongitude == 0)
                {
                    detail.CompleteLongitude = detail.PubLongitude;
                    detail.CompleteLatitude = detail.PubLatitude;
                }
                #endregion
            }
            return detail;

        }

        /// <summary>
        /// 一键发单修改地址和电话（内部会抛业务异常）
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="newAddress">新的收货人地址</param>
        /// <param name="newPhone">新的收货人电话</param>
        /// <returns>是否修改成功</returns>
        public int UpdateOrderAddressAndPhone(string orderId, string newAddress, string newPhone)
        {
            return orderDao.UpdateOrderAddressAndPhone(orderId, newAddress, newPhone);
        }


        #region 用户自定义方法
        /// <summary>
        /// 更新用户金额
        /// wc 完成订单时候调用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateClienterAccount(OrderListModel myOrderInfo,decimal money)
        {
            //更新骑士 金额  
            bool b = clienterDao.UpdateClienterAccountBalanceForFinish(new WithdrawRecordsModel() { UserId = myOrderInfo.clienterId, Amount = money });
            //增加记录 
            decimal? accountBalance = myOrderInfo.AccountBalance.Value + money;           

            ///TODO 骑士余额流水表，不是这个吧？
            ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
            {
                ClienterId = myOrderInfo.clienterId,
                Amount = money,
                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                Balance = accountBalance ?? 0,
                RecordType = ClienterBalanceRecordRecordType.BalanceAdjustment.GetHashCode(),
                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士" : myOrderInfo.ClienterName,
                WithwardId = myOrderInfo.Id,
                RelationNo = myOrderInfo.OrderNo,
                Remark = "余额调整"
            };
            clienterBalanceRecordDao.Insert(cbrm);
        }

        /// <summary>
        /// 更新用户金额      
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateAccountBalanceAndWithdraw(OrderListModel myOrderInfo, decimal money)
        {
            int userId = myOrderInfo.clienterId;
            //更新骑士 金额  
            bool b = clienterDao.UpdateAccountBalanceAndWithdraw(new WithdrawRecordsModel() { UserId = userId, Amount = money });
            //增加记录 
            decimal? accountBalance = myOrderInfo.AccountBalance.Value + money;            

            ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
            {
                ClienterId = userId,
                Amount = myOrderInfo.OrderCommission == null ? 0 : Convert.ToDecimal(myOrderInfo.OrderCommission),
                Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                Balance = accountBalance ?? 0,
                RecordType = ClienterBalanceRecordRecordType.BalanceAdjustment.GetHashCode(),
                Operator = string.IsNullOrEmpty(myOrderInfo.ClienterName) ? "骑士:" + userId : myOrderInfo.ClienterName,
                WithwardId = myOrderInfo.Id,
                RelationNo = myOrderInfo.OrderNo,
                Remark = "余额调整"
            };
            clienterBalanceRecordDao.Insert(cbrm);
          
        }

        #endregion

    }
}
