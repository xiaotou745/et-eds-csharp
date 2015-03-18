using CalculateCommon;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.MyPush;
using Ets.Service.Provider.Subsidy;
using Ets.Service.Provider.User;
using ETS.Page;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var income = OrderCommissionProvider.GetCurrenOrderCommission(orderComm);
                var amount = OrderCommissionProvider.GetCurrenOrderPrice(orderComm);

                resultModel.income = income;  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
                resultModel.Amount = amount; //C端 获取订单的金额 Edit bycaoheyang 20150305


                resultModel.businessName = from.BusinessName;
                resultModel.businessPhone = from.BusinessPhone;
                resultModel.pickUpCity = from.PickUpCity.Replace("市", "");


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
                var income = OrderCommissionProvider.GetCurrenOrderCommission(orderComm);
                var amount = OrderCommissionProvider.GetCurrenOrderPrice(orderComm);

                resultModel.income = income;  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
                resultModel.Amount = amount; //C端 获取订单的金额 Edit bycaoheyang 20150305


                resultModel.businessName = from.BusinessName;
                resultModel.businessPhone = from.BusinessPhone;
                resultModel.pickUpCity = from.PickUpCity.Replace("市", "");


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

        #region 订单状态查询功能  add by caoheyang 20150316
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="orderNo">订单号码</param>
        /// <returns></returns>
        public int GetStatus(string orderNo)
        {
            OrderDao OrderDao = new OrderDao();
            return OrderDao.GetStatus(orderNo);
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
            to.ReceviceName = busiOrderInfoModel.receviceName;
            to.RecevicePhoneNo = busiOrderInfoModel.recevicePhone;
            to.ReceviceAddress = busiOrderInfoModel.receviceAddress;
            to.IsPay = busiOrderInfoModel.IsPay;
            to.Amount = busiOrderInfoModel.Amount;
            to.OrderCount = busiOrderInfoModel.OrderCount;  //订单数量
            to.ReceviceLongitude = busiOrderInfoModel.longitude;
            to.ReceviceLatitude = busiOrderInfoModel.laitude;

            //var subsidy = SubsidyLogic.subsidyLogic().GetCurrentSubsidy(groupId: business.GroupId == null ? 0 : Convert.ToInt32(business.GroupId));

            var subsidy = iSubsidyProvider.GetCurrentSubsidy(groupId: business.GroupId == null ? 0 : Convert.ToInt32(business.GroupId));

            if (subsidy != null)
            {
                to.WebsiteSubsidy = subsidy.WebsiteSubsidy;  //网站补贴
                to.CommissionRate = subsidy.OrderCommission == null ? 0 : subsidy.OrderCommission; //佣金比例 

                decimal distribe = 0;  //默认外送费，网站补贴都为0
                if (to.DistribSubsidy != null)//如果外送费有数据，按照外送费计算骑士佣金
                    distribe = Convert.ToDecimal(to.DistribSubsidy);
                else if (to.WebsiteSubsidy != null)//如果外送费没数据，按照网站补贴计算骑士佣金
                    distribe = Convert.ToDecimal(to.WebsiteSubsidy);

                to.OrderCommission = busiOrderInfoModel.Amount * to.CommissionRate + distribe * to.OrderCount;//计算佣金
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


        #region  第三方对接 物流订单接收接口  add by caoheyang 201503167

        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns>订单号码</returns>
        public string Create(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                string orderNo = OrderDao.CreateToSql(paramodel);
                if (!string.IsNullOrWhiteSpace(orderNo))
                    Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！"
                        , string.Empty, paramodel.address.city_code); //激光推送   bug  原来是根据城市推送的，现在没要求传城市相关信息
                tran.Complete();
                return orderNo;
            }
        }

        #endregion
    }
}
