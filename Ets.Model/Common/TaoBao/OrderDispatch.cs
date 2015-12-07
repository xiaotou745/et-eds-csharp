using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.TaoBao
{
    public class OrderDispatch
    {
        /// <summary>
        /// 物流单号
        /// </summary>
        public long delivery_order_no { get; set; }
        /// <summary>
        /// 通知时间
        /// </summary>
        public long notify_time { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        ///店铺名称
        /// </summary>
        public string store_name { get; set; }
        /// <summary>
        /// 店铺地址
        /// </summary>
        public string store_address { get; set; }

        /// <summary>
        /// 订单确认结束时间
        /// </summary>
        public long end_time { get; set; }

        /// <summary>
        /// 店铺经纬度
        /// </summary>
        public string starting_point { get; set; }

        /// <summary>
        /// 收货地址经纬度
        /// </summary>
        public string destination_point { get; set; }

        /// <summary>
        /// 卖家电话
        /// </summary>
        public string shipper_phone { get; set; }

        /// <summary>
        /// 买家收货地址
        /// </summary>
        public string consignee_address { get; set; }

        /// <summary>
        /// 买家姓名
        /// </summary>
        public string consignee_name { get; set; }

        /// <summary>
        /// 买家电话
        /// </summary>
        public string consignee_phone { get; set; }

        /// <summary>
        /// 实付金额（单位：分）
        /// </summary>
        public long actually_paid { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 期望取件时间
        /// </summary>
        public long expected_take_time { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        public string city_code { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string city_name { get; set; }

        /// <summary>
        /// 期望送达时间
        /// </summary>
        public long expected_delivery { get; set; }
        /// <summary>
        /// 配送费（单位：分）
        /// </summary>
        public long delivery_fee { get; set; }

        /// <summary>
        /// 复杂对象json
        /// </summary>
        public string items { get; set; }
        /// <summary>
        /// 复杂对象json	订单商品信息
        /// </summary>
        public List<Commodity> itemsList { get; set; }
        /// <summary>
        /// 小票ID
        /// </summary>
        public string receipt_id { get; set; }
        /// <summary>
        /// 订单扩展信息(tradeId：交易订单号)
        /// </summary>
        public string order_ext_info { get; set; }
    }

    public class Commodity
    {    
        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        public string unit { get; set; }
        
        /// <summary>
        /// 单位重量
        /// </summary>
        public string unitWeight { get; set; }
        
        /// <summary>
        /// 单位价格（分）
        /// </summary>
        public long unitPrice { get; set; }
        
        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }
        
        /// <summary>
        /// 总重量
        /// </summary>
        public string totalWeight { get; set; }
        
        /// <summary>
        /// 总价格（分）
        /// </summary>
        public long totalPrice { get; set; }


    }
}
