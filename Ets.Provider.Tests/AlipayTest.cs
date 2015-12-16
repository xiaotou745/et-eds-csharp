using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.AliPay;
using Ets.Service.Provider.Pay;
using NUnit.Framework;
using ETS.Library.Pay.SSAliPay;
using Aop.Api.Response;

namespace Ets.Provider.Tests
{
    /// <summary>
    /// 支付宝测试类
    /// danny-20150914
    /// </summary>
    [TestFixture]
    public class AlipayTest
    {
        public void Refund()
        {
            AliPayApi pay = new AliPayApi();
            pay.Query();
            AlipayTradeRefundResponse fund = pay.Refund();

        }
        /// <summary>
        /// 转账
        /// </summary>
        [Test]
        public void AlipayTransfer()
        {
            //SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            //sParaTemp.Add("service ", "btn_status_query");
            //sParaTemp.Add("batch_no", "2015102210312566318226");
            //string s = ETS.Pay.AliPay.Submit.BuildRequest(sParaTemp);

            //var model = new PayProvider().AlipayTransfer(new AlipayTransferParameter()
            //{
            //    Partner = "2088911703660069",//2088911703660069
            //    InputCharset = "GBK",
            //    NotifyUrl = "http://pay153.yitaoyun.net:8011",
            //    Email = "info@edaisong.com",
            //    AccountName = "易代送网络科技（北京）有限公司",
            //    PayDate = DateTime.Now.ToString("YYYYmmdd"),
            //    BatchNo = DateTime.Now.ToString("YYYYmmdd")+"001",//"2010080100000211",
            //    BatchFee = "20",
            //    BatchNum = "1",
            //    DetailData = "10000001^dou631@163.com^白玉^1^测试转账"
            //});
        }

        /// <summary>
        /// 生成批次号
        /// </summary>
        [Test]
        public void AlipayCreateNo()
        {
            //var no = new PayProvider().CreateAlipayBatchNo();
        }
    }
}
