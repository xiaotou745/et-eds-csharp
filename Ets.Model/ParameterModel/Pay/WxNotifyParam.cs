using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Pay
{

    //回调响应实例
    //[{"transaction_id":"1004120646201602183353757981","nonce_str":"vp4agwe9prklzokafeml48q7yr1viqlv","bank_type":"CFT","openid":"oKdbAtzkSkQOYZgOSDXt6RAUSklE","sign":"DE843E771BAF045FA86E4404DAB385B6","fee_type":"CNY","mch_id":"1243442302","cash_fee":"1","out_trade_no":"1987160218110046074","appid":"wxb89ebba3cec98a8c","total_fee":"1","trade_type":"NATIVE","result_code":"SUCCESS","attach":"http://172.18.10.73:7178/pay/BusinessRechargeNotify|2","time_end":"20160218110528","is_subscribe":"N","return_code":"SUCCESS"}]
    public class WxNotifyParam
    {
        public string transaction_id { get; set; }
        public string nonce_str { get; set; }
        public string bank_type { get; set; }
        public string openid { get; set; }
        public string sign { get; set; }
        public string fee_type { get; set; }
        public string mch_id { get; set; }
        public string cash_fee { get; set; }
        public string out_trade_no { get; set; }
        public string appid { get; set; }
        public string total_fee { get; set; }
        public string trade_type { get; set; }
        public string result_code { get; set; }
        public string attach { get; set; }
        public string time_end { get; set; }
        public string is_subscribe { get; set; }
        public string return_code { get; set; }

    }
}
