
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Expand;
namespace SuperManWebApi.Models.Business
{
    public class NewPostPublishOrderModel
    {
        /// <summary>
        /// 易代送平台的商户Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 原订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 原平台商户Id
        /// </summary>
        public int OriginalBusinessId { get; set; }
        /// <summary>
        /// 原平台订单创建时间
        /// </summary>
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ReceivePhoneNo { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 份数
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 收货人所在省
        /// </summary>
        public string Receive_Province { get; set; }
        /// <summary>
        /// 收货人所在市
        /// </summary>
        public string Receive_City { get; set; }
        /// <summary>
        /// 收货人所在区
        /// </summary>
        public string Receive_Area { get; set; }
        public string Receive_ProvinceCode { get; set; }
        public string Receive_CityCode { get; set; }
        public string Receive_AreaCode { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string Receive_Address { get; set; }
        public int OrderFrom { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        public decimal OrderCommission { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal WebsiteSubsidy { get; set; }
        /// <summary>
        /// 收货人所在区域经度
        /// </summary>
        public double Receive_Longitude { get; set; }
        /// <summary>
        /// 收货人所在区域纬度
        /// </summary>
        public double Receive_Latitude { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 订单总重量
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 订单类型： 1送餐订单，2取餐盒订单
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 公里数，商户地址到收货人地址的距离
        /// </summary>
        public double KM { get; set; }
        /// <summary>
        /// 锅具数量
        /// </summary>
        public int GuoJuQty { get; set; }
        /// <summary>
        /// 炉具数量
        /// </summary>
        public int LuJuQty { get; set; }
        /// <summary>
        /// 送餐时间 客户要求的送餐时间
        /// </summary>
        public DateTime? SongCanDate { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }


    }    
    public enum OrderPublicshStatus : int
    {
        [DisplayText("订单发布成功")]
        Success = 1,
        [DisplayText("订单发布失败")]
        Failed = 0,
        [DisplayText("原始订单号不能为空")]
        OriginalOrderNoEmpty = 301,

        [DisplayText("原平台商户Id不能为空")]
        OriginalBusinessIdEmpty = 302,

        [DisplayText("请确认是否已付款")]
        IsPayEmpty = 303,
        [DisplayText("收货人不能为空")]
        ReceiveNameEmpty = 304,
        [DisplayText("收货人手机号不能为空")]
        ReceivePhoneEmpty = 305,

        [DisplayText("收货人所在省不能为空")]
        ReceiveProvinceEmpty = 306,

        [DisplayText("收货人所在市不能为空")]
        ReceiveCityEmpty = 307,

        [DisplayText("收货人所在区不能为空")]
        ReceiveAreaEmpty = 308,

        [DisplayText("收货人地址不能为空")]
        ReceiveAddressEmpty = 309,

        [DisplayText("订单来源不能为空")]
        OrderFromEmpty = 310,
        [DisplayText("商户不存在,请先注册商户")]
        BusinessNoExist = 311,
        [DisplayText("该订单已存在")]
        OrderHadExist = 312,
        [DisplayText("商户未审核")]
        BusinessNotAudit = 313


    }
}

