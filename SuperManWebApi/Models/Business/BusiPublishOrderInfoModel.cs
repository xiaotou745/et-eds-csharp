using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class BusiPublishOrderInfoModel
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string B_Name { get; set; }
        /// <summary>
        /// 商户联系电话
        /// </summary>
        public string B_Phone { get; set; }
        /// <summary>
        /// 商户所在省份
        /// </summary>
        public string B_Province { get; set; }
        /// <summary>
        /// 商户所在城市
        /// </summary>
        public string B_City { get; set; }
        /// <summary>
        /// 商户所在区域
        /// </summary>
        public string B_Area { get; set; }
        /// <summary>
        /// 商户所在区域经度
        /// </summary>
        public double B_Longitude { get; set; }
        /// <summary>
        /// 商户所在区域纬度
        /// </summary>
        public double B_Latitude { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickUpAddress { get; set; }
        /// <summary>
        /// 订单发布时间
        /// </summary>
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 订单总份数
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        public decimal OrderCommission { get; set;}
        /// <summary>
        /// 配送补贴
        /// </summary>
        public decimal DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal WebsiteSubsidy { get; set; }
        /// <summary>
        /// 收货人所在省份
        /// </summary>
        public string Recevicer_Province { get; set; }
        /// <summary>
        /// 收货人所在城市
        /// </summary>
        public string Recevicer_City { get; set; }
        /// <summary>
        /// 收货人所在区域
        /// </summary>
        public string Recevicer_Area { get; set; }
        /// <summary>
        /// 收货人区域经度
        /// </summary>
        public double Recevicer_Longitude { get; set; }
        /// <summary>
        /// 收货人区域纬度
        /// </summary>
        public double Recevicer_Latitude { get; set; }
        /// <summary>
        /// 订单来源
        /// 1易淘食
        /// </summary>
        public int OrderFrom { get; set; }
    }
}