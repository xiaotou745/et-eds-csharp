using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.AliPay;
using Ets.Service.Provider.Pay;
using NUnit.Framework;

namespace Ets.Provider.Tests
{
    /// <summary>
    /// 支付宝测试类
    /// danny-20150914
    /// </summary>
    [TestFixture]
    public class AlipayTest
    {
        /// <summary>
        /// 转账
        /// </summary>
        [Test]
        public void AlipayTransfer()
        {
            var model = new PayProvider().AlipayTransfer(new AlipayTransferParameter()
            {
                Partner = "2088911703660069",//2088911703660069
                InputCharset = "GBK",
                NotifyUrl = "http://pay153.yitaoyun.net:8011",
                Email = "info@edaisong.com",
                AccountName = "宋桥",
                PayDate = "20150914",
                BatchNo = "2010080100000211",
                BatchFee = "20",
                BatchNum = "1",
                DetailData = "10000001^dou631@163.com^白玉^1^测试转账"
            });
        }
    }
}
