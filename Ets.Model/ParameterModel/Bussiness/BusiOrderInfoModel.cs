using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// B端发布订单所需数据
    /// </summary>
    public class BusiOrderInfoModel
    {
        /// <summary>
        /// 当前发布者
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receviceName { get; set; }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string recevicePhone { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string receviceAddress { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double laitude { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }

        /// <summary>
        /// 订单签名（保证唯一性）
        /// </summary>
        public string OrderSign { get; set; }

    }
}
