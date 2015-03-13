using CalculateCommon;
using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    public class ClientOrderResultModel
    {
        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal? income { get; set; }
        /// <summary>
        /// 距你
        /// </summary>
        //public double distance { get; set; }
        public string distance { get; set; }
        
        /// <summary>
        ///  商户到收货人的距离
        /// </summary>
        public string distanceB2R { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string pubDate { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        public string businessName { get; set; }
        /// <summary>
        /// 取货城市
        /// </summary>
        public string pickUpCity { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string pickUpAddress { get; set; }
        /// <summary>
        /// 发布电话
        /// </summary>
        public string businessPhone { get; set; }
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string receviceName { get; set; }
        /// <summary>
        /// 收货人城市
        /// </summary>
        public string receviceCity { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        public string receviceAddress { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string recevicePhone { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 买家是否付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }
    }

    public class ClientOrderNoLoginResultModel
    {
        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 源订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal? income { get; set; }
        /// <summary>
        /// 距你
        /// </summary>
        //public double distance { get; set; }
        public string distance { get; set; }
        /// <summary>
        /// 商户到收货人的距离
        /// </summary>
        //public double distanceB2R { get; set; }
        public string distanceB2R { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string pubDate { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        public string businessName { get; set; }
        /// <summary>
        /// 取货城市
        /// </summary>
        public string pickUpCity { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string pickUpAddress { get; set; }
        /// <summary>
        /// 发布电话
        /// </summary>
        public string businessPhone { get; set; }
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string receviceName { get; set; }
        /// <summary>
        /// 收货人城市
        /// </summary>
        public string receviceCity { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string receviceAddress { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string recevicePhone { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 买家是否付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }
    }
    public class degree
    {
        public static double longitude { get; set; }
        public static double latitude { get; set; }
    }
    public class ClientOrderResultModelTranslator : TranslatorBase<order, ClientOrderResultModel>
    {
        public static readonly ClientOrderResultModelTranslator Instance = new ClientOrderResultModelTranslator();
        public override ClientOrderResultModel Translate(order from)
        {
            var resultModel = new ClientOrderResultModel();
            if (from.clienterId != null)
                resultModel.userId = from.clienterId.Value;
            resultModel.OrderNo = from.OrderNo;
            resultModel.income = OrderLogic.orderLogic().GetCurrenOrderCommission(from);  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
            resultModel.Amount = OrderLogic.orderLogic().GetCurrenOrderPrice(from); //C端 获取订单的金额 Edit bycaoheyang 20150305
            //resultModel.income = from.DistribSubsidy + from.WebsiteSubsidy + from.OrderCommission;
            //resultModel.Amount = from.Amount.Value;
            business _business = null;
            if (from.businessId.HasValue)
            {
                _business = BusiLogic.busiLogic().GetBusinessById(from.businessId.Value);
            }
            if (_business != null)
            {
                resultModel.businessName = _business.Name;
                resultModel.businessPhone = _business.PhoneNo2;
                resultModel.pickUpCity = _business.City.Replace("市", "");
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
            resultModel.OrderCount = from.OrderCount;
            if (_business != null)
            {
                Degree degree1 = new Degree(degree.longitude, degree.latitude);
                Degree degree2 = new Degree(_business.Longitude.Value, _business.Latitude.Value);
                double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                resultModel.distance = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
            }
            else
                resultModel.distance = "--";
            if (_business != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
            {
                Degree degree1 = new Degree(_business.Longitude.Value, _business.Latitude.Value);
                Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                resultModel.distanceB2R = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
            }
            else
                resultModel.distanceB2R = "--";
            return resultModel;
        }

        public override order Translate(ClientOrderResultModel from)
        {
            throw new NotImplementedException();
        }
    }

    public class ClientOrderNoLoginResultModelTranslator : TranslatorBase<order, ClientOrderNoLoginResultModel>
    {
        public static readonly ClientOrderNoLoginResultModelTranslator Instance = new ClientOrderNoLoginResultModelTranslator();
        public override ClientOrderNoLoginResultModel Translate(order from)
        {
            var resultModel = new ClientOrderNoLoginResultModel();
            if (from.clienterId != null)
                resultModel.userId = from.clienterId.Value;
            resultModel.OrderNo = from.OrderNo;
            resultModel.OriginalOrderNo = from.OriginalOrderNo; //来源订单号
            resultModel.income = OrderLogic.orderLogic().GetCurrenOrderCommission(from);  //计算设置当前订单骑士可获取的佣金 Edit bycaoheyang 20150305
            resultModel.Amount = OrderLogic.orderLogic().GetCurrenOrderPrice(from); //C端 获取订单的金额 Edit bycaoheyang 20150305
            //resultModel.income = from.DistribSubsidy + from.WebsiteSubsidy + from.OrderCommission; //骑士的收入= 网站补贴+ 外送费 + 订单佣金
            //resultModel.Amount = from.Amount.Value;
            business _business = null;
            if (from.businessId.HasValue)
            {
                _business = BusiLogic.busiLogic().GetBusinessById(from.businessId.Value);                
            }
            if (_business != null)
            {
                resultModel.businessName = _business.Name;
                resultModel.businessPhone = _business.PhoneNo2;
                resultModel.pickUpCity = _business.City.Replace("市", "");
            }
            //if (from.business != null)
            //{
            //    resultModel.businessName = from.business.Name;
            //    resultModel.businessPhone = from.business.PhoneNo2;
            //    if (from.business.City != null)
            //        resultModel.pickUpCity = from.business.City.Replace("市", "");
            //}
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
            resultModel.OrderCount = from.OrderCount;
            if (degree.longitude != null && degree.latitude != null && _business.Longitude != null && _business.Latitude != null)
            {
                Degree degree1 = new Degree(degree.longitude, degree.latitude);
                Degree degree2 = new Degree(_business.Longitude.Value, _business.Latitude.Value);
                double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                resultModel.distance = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
            }
            else
                resultModel.distance = "--";
            if (_business != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
            {
                Degree degree1 = new Degree(_business.Longitude.Value, _business.Latitude.Value);
                Degree degree2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                double res = CoordDispose.GetDistanceGoogle(degree1, degree2);
                resultModel.distanceB2R = res < 1000 ? (res.ToString("f2") + "m") : ((res / 1000).ToString("f2") + "km");
            }
            else
                resultModel.distanceB2R = "--";
            return resultModel;
        }

        public override order Translate(ClientOrderNoLoginResultModel from)
        {
            throw new NotImplementedException();
        }
    }
}