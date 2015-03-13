using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperManDataAccess;
using SuperManCore;
using CalculateCommon;
namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        public int clienterId { get; set; }
        /// <summary>
        /// 商户信息
        /// </summary>
        public BusinessModel BusinessModel { get; set; }
        /// <summary>
        /// 超人信息
        /// </summary>
        public ClienterModel ClienterModel { get; set; }
        public string ClienterName { get; set; }
        public string ClienterPhoneNo { get; set; }
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
        /// 商家到收货人的距离
        /// </summary>
        public double distanceB2R { get; set; }

        /// <summary>
        /// 原平台订单id
        /// </summary>
        public long? OriginalOrderId { get; set; }
        /// <summary>
        /// 原平台订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 集团名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }
        
    }

    /// <summary>
    /// 订单实体
    /// </summary>
    public class BusiGetOrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        public string superManName { get; set; }
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
    }
    
    
}
