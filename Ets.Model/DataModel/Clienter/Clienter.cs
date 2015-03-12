using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;


namespace Ets.Model.DataModel.Clienter
{
 
    public class ClientOrderSearchCriteria
    {
        public PagingResult PagingRequest { get; set; }
        public int userId { get; set; }
        public sbyte? status { get; set; }
        public bool isLatest { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        //城市名称
        public string city { get; set; }
        //城市Id
        public string cityId { get; set; }
        /// <summary>
        /// 订单类型：1送餐订单，2收锅订单
        /// </summary>
        public int OrderType { get; set; }

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
    }
}
