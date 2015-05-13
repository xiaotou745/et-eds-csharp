using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
namespace Ets.Model.DomainModel.Order
{
    public class OrderDM
    {
        public OrderDM() { }    
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 其它平台的来源订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }

        /// <summary>
        /// 订单来源，默认0表示E代送B端订单，1易淘食,2万达，3全时，4美团
        /// </summary>
        public int OrderFrom { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        public decimal? OrderCommission { get; set; }

        /// <summary>
        /// 距离
        /// </summary>    
        public string distance { get; set; }
        /// <summary>
        /// 骑士距离客户的距离用来排序
        /// </summary>
        public double distance_OrderBy { get; set; }
        /// <summary>
        ///  商户到收货人的距离
        /// </summary>
        public string distanceB2R { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDate { get; set; }

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
        public string PickUpAddress { get; set; }

        /// <summary>
        /// 发布电话
        /// </summary>
        public string businessPhone { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }

        /// <summary>
        /// 收货人城市
        /// </summary>
        public string receviceCity { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单状态  0:订单新增 1：订单已完成 2：订单已接单 3：订单已取消  30 待接入订单
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }      

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }       
         

        /// <summary>
        /// 取货码（目前只有全时再用）
        /// </summary>
        public string PickupCode { get; set; }

        /// <summary>
        /// 支付类型 0 现金
        /// </summary>
        public int? Payment { get; set; }
            

        /// <summary>
        /// 子订单集合
        /// </summary>
        public List<OrderChildInfo> listOrderChild { get; set; }

        /// <summary>
        /// 订单明细集合
        /// </summary>
        public List<OrderDetailInfo> listOrderDetail { get; set; }        
    }

    public class OrderChildInfo
    {
        public OrderChildInfo() { }     
        /// <summary>
        /// 子订单ID(从1开始，顺序递增，以订单为单位)
        /// </summary>
        public int ChildId { get; set; }
        /// <summary>
        /// 商品总价格
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal GoodPrice { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal DeliveryPrice { get; set; }
        /// <summary>
        /// 支付方式(1 用户支付 2 骑士代付)
        /// </summary>
        public int? PayStyle { get; set; }
        /// <summary>
        /// 支付类型(1 支付宝 2 微信 3 网银)
        /// </summary>
        public int? PayType { get; set; }
        /// <summary>
        /// 支付状态(1待支付 2 已支付)
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 支付人
        /// </summary>
        public string PayBy { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayPrice { get; set; }
        /// <summary>
        /// 是否上传上票
        /// </summary>
        public bool HasUploadTicket { get; set; }
        /// <summary>
        /// 小票图片路径
        /// </summary>
        public string TicketUrl { get; set; }     
    
    }

    public class OrderDetailInfo
    {
        public OrderDetailInfo() { }
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        ///// <summary>
        ///// 订单号
        ///// </summary>
        //public string OrderNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 第三方平台明细id,与GroupID组成联合唯一约束
        /// </summary>
        public int FormDetailID { get; set; }
        /// <summary>
        /// 集团id,与第三方平台明细id组成联合唯一约束
        /// </summary>
        public int GroupID { get; set; }

    }   
    ///// <summary>
    ///// 订单实体
    ///// </summary>
    //public class BusiGetOrderModel
    //{
    //    /// <summary>
    //    /// 订单号
    //    /// </summary>
    //    public string OrderNo { get; set; }
    //    public string superManName { get; set; }
    //    public string superManPhone { get; set; }
    //    public string PickUpName { get; set; }
    //    /// <summary>
    //    /// 取货地址
    //    /// </summary>
    //    public string PickUpAddress { get; set; }
    //    /// <summary>
    //    /// 发布时间
    //    /// </summary>
    //    public string PubDate { get; set; }
    //    /// <summary>
    //    /// 收货人
    //    /// </summary>
    //    public string ReceviceName { get; set; }
    //    /// <summary>
    //    /// 收货电话
    //    /// </summary>
    //    public string RecevicePhoneNo { get; set; }
    //    /// <summary>
    //    /// 收货地址
    //    /// </summary>
    //    public string ReceviceAddress { get; set; }
    //    /// <summary>
    //    /// 实际完成时间
    //    /// </summary>
    //    public string ActualDoneDate { get; set; }
    //    /// <summary>
    //    /// 是否已付款
    //    /// </summary>
    //    public bool? IsPay { get; set; }
    //    /// <summary>
    //    /// 订单金额
    //    /// </summary>
    //    public decimal? Amount { get; set; }
    //    /// <summary>
    //    /// 配送说明
    //    /// </summary>
    //    public string Remark { get; set; }
    //    /// <summary>
    //    /// 订单状态
    //    /// </summary>
    //    public byte? Status { get; set; }
    //    /// <summary>
    //    /// 商家到收货人的距离
    //    /// </summary>
    //    public double distanceB2R { get; set; }
    //}
}
