using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
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

        /// <summary>
        /// 子订单列表
        /// </summary>
        public List<OrderChlidPM> listOrderChlid { get; set; }

    }
}
