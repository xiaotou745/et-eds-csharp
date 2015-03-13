﻿using CalculateCommon;
using Ets.Dao.Order;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using ETS.Page;
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
                resultModel.OrderCount = from.OrderCount.HasValue ? from.OrderCount.Value : 1;
                var orderComm = new OrderCommission() { Amount = from.Amount, CommissionRate = from.CommissionRate, DistribSubsidy = from.DistribSubsidy, OrderCount = resultModel.OrderCount, WebsiteSubsidy = from.WebsiteSubsidy };
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
                if (from.BusiLatitude.Value != null && from.BusiLongitude.Value != null)
                {
                    var degree1 = new Degree(degree.longitude, degree.latitude);
                    var degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);

                    var dTmp = CoordDispose.GetDistanceGoogle(degree1, degree2) / 1000;
                    var sTmp = dTmp.ToString("f2");
                    resultModel.distance = sTmp;
                }
                else
                    resultModel.distance = "0.0";
                if (from.BusiLatitude.Value != null && from.BusiLongitude.Value != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
                {
                    var d1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);
                    var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                    var dTmp = CoordDispose.GetDistanceGoogle(d1, d2) / 1000;
                    var sTmp = dTmp.ToString("f2");

                    resultModel.distanceB2R = sTmp;
                }
                else
                { 
                    resultModel.distanceB2R = "0.0"; 
                }
                list.Add(resultModel);
            }
            return list;
        }



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
                resultModel.OrderCount = from.OrderCount.HasValue ? from.OrderCount.Value : 1;
                var orderComm = new OrderCommission() { Amount = from.Amount, CommissionRate = from.CommissionRate, DistribSubsidy = from.DistribSubsidy, OrderCount = resultModel.OrderCount, WebsiteSubsidy = from.WebsiteSubsidy };
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
                if (from.BusiLatitude.Value != null && from.BusiLongitude.Value != null)
                {
                    var degree1 = new Degree(degree.longitude, degree.latitude);
                    var degree2 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);

                    var dTmp = CoordDispose.GetDistanceGoogle(degree1, degree2) / 1000;
                    var sTmp = dTmp.ToString("f2");
                    resultModel.distance = sTmp;
                }
                else
                    resultModel.distance = "0.0";
                if (from.BusiLatitude.Value != null && from.BusiLongitude.Value != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
                {
                    var d1 = new Degree(from.BusiLongitude.Value, from.BusiLatitude.Value);
                    var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                    var dTmp = CoordDispose.GetDistanceGoogle(d1, d2) / 1000;
                    var sTmp = dTmp.ToString("f2");

                    resultModel.distanceB2R = sTmp;
                }
                else
                {
                    resultModel.distanceB2R = "0.0";
                }
                list.Add(resultModel);
            }
            return list;
        }        
    }
}
