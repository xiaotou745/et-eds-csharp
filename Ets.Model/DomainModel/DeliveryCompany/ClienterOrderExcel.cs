using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.DeliveryCompany
{
    public class ClienterOrderExcel
    {
        [Description("订单号")]
        public string OrderNo { get; set; }
        [Description("商户名称")]
        public string BusinessName { get; set; }
        [Description("骑士ID")]
        public int ClienterId { get; set; }
        [Description("骑士姓名")]
        public string ClienterName { get; set; }
        [Description("下单时间")]
        public DateTime PubDate { get; set; }
        [Description("接单时间")]
        public DateTime? JieDanTime { get; set; }
        [Description("取货时间")]
        public DateTime? QuHuoTime { get; set; }
        [Description("完成时间")]
        public DateTime? ActualDoneDate { get; set; }
        [Description("订单金额")]
        public decimal? Amount { get; set; }
        [Description("订单数量")]
        public int OrderCount { get; set; }
        [Description("结算类型")]
        public string SettleType { get; set; }
        [Description("公司结算数值")]
        public decimal CompanySettleValue { get; set; }
        [Description("骑士结算数值")]
        public decimal SuperManSettleValue { get; set; }
        [Description("公司总结算")]
        public decimal CompanySettleValueAll { get; set; }
        [Description("骑士总结算")]
        public decimal SuperManSettleValueAll { get; set; }
        [Description("订单状态")]
        public string OrderStatus { get; set; }
        [Description("城市")]
        public string BusinessCity { get; set; }
        [Description("是否在线支付")]
        public string MealsSettleMode { get; set; }
        [Description("是否异常订单")]
        public string IsNotRealOrder { get; set; }



    }
}
