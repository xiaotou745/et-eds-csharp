﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderListModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickUpAddress { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualDoneDate { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool? IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        public decimal? OrderCommission { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal? WebsiteSubsidy { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int clienterId { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public int businessId { get; set; }
        /// <summary>
        /// 收获人所在城市
        /// </summary>
        public string ReceviceCity { get; set; }
        /// <summary>
        /// 收获地址经度
        /// </summary>
        public double ReceviceLongitude { get; set; }
        /// <summary>
        /// 收货地址纬度
        /// </summary>
        public double ReceviceLatitude { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }
        /// <summary>
        /// 原始订单Id
        /// </summary>
        public long OriginalOrderId { get; set; }
        /// <summary>
        /// 原始订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        ///数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 收货省份
        /// </summary>
        public string ReceiveProvince { get; set; }
        /// <summary>
        /// 收货地区
        /// </summary>
        public string ReceiveArea { get; set; }
        /// <summary>
        /// 收货省份编码
        /// </summary>
        public string ReceiveProvinceCode { get; set; }
        /// <summary>
        /// 收货城市编码
        /// </summary>
        public string ReceiveCityCode { get; set; }
        /// <summary>
        /// 收货地区编码
        /// </summary>
        public string ReceiveAreaCode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
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
        /// 送餐时间
        /// </summary>
        public string SongCanDate { get; set; }
        public int OrderCount { get; set; }
        public decimal CommissionRate { get; set; }
        public string OrderSign { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 商家手机号
        /// </summary>
        public string BusinessPhoneNo { get; set; }
        /// <summary>
        /// 商家地址
        /// </summary>
        public string BusinessAddress { get; set; }
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士手机号
        /// </summary>
        public string ClienterPhoneNo { get; set; }
        /// <summary>
        /// 订单所属商家的集团名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 商家所在城市
        /// </summary>
        public string BusinessCity { get; set; }
        

       
    }
}