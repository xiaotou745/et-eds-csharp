using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Clienter
{
    public class ClientOrderResultModel
    {
        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
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

        public string businessPhone2 { get; set; }
        
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

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }

        public int OrderFrom { get; set; }
        /// <summary>
        ///  是否需要做取货码验证 0 不需要 1 需要
        /// </summary>
        public int NeedPickupCode { get; set; }
        /// <summary>
        /// 已经上传的小票数量
        /// wc
        /// </summary>
        public int HadUploadCount { get; set; }
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
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 源订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        public int OrderFrom { get; set; }
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
        /// 骑士距离客户的距离用来排序
        /// </summary>
        public double distance_OrderBy { get; set; }
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
        public string businessPhone2 { get; set; }        
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

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }


        /// <summary>
        ///  是否需要做取货码验证 0 不需要 1 需要
        /// </summary>
        public int NeedPickupCode { get; set; }
        public int HadUploadCount { get;set; }
    }
    public class degree
    {
        public static double longitude { get; set; }
        public static double latitude { get; set; }
    }
}
