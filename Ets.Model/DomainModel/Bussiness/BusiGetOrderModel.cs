using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ets.Model.DomainModel.Bussiness
{
    /// <summary>
    /// B端获取订单app查询数据实体 add by caoheyang 20140311
    /// </summary>
    public class BusiGetOrderModel
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 超人姓名
        /// </summary>
        public string superManName { get; set; }
        /// <summary>
        /// 超人电话
        /// </summary>
        public string superManPhone { get; set; }
        public string PickUpName { get; set; }
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
        /// 收货电话
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public string ActualDoneDate { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool? IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }
        /// <summary>
        /// 商家到收货人的距离
        /// </summary>
        public double distanceB2R { get; set; }
        /// <summary>
        /// OrderFromName 0B端商家1易淘食4美团
        /// </summary>
        public string OrderFromName { get; set; }
        /// <summary>
        /// OrderFrom 0B端商家1易淘食4美团
        /// </summary>
        public string OrderFrom { get; set; }
        /// <summary>
        /// OriginalOrderNo原平台订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }

       public int MealsSettleMode { get; set; }
    }
}