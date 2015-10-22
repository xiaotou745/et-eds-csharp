using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETS.Pay.AliPay;
using Ets.Service.Provider.Pay;

namespace SuperMan.Controllers
{
    public class AlipayController : Controller
    {
        // GET: Alipay
        public ActionResult Index()
        {
            /// <summary>
            /// 支付宝打款测试页面
            /// 茹化肖
            /// 2015年10月16日11:46:06  TODO 开发完之后删除
            /// </summary>
            /// <returns></returns>
            var model = new PayProvider().AlipayTransfer(new AlipayTransferParameter()
            {
                Partner = "2088911703660069",//2088911703660069
                InputCharset = "utf-8",
                NotifyUrl = "http://pay153.yitaoyun.net:8011/pay/AlipayForBatch",
                Email = "info@edaisong.com",
                AccountName = "易代送网络科技（北京）有限公司",
                PayDate = DateTime.Now.ToString("YYYYmmdd"),
                BatchNo = "20151022123456789",//批次号不可重复
                BatchFee = "1",
                BatchNum = "1",
                DetailData = "10000008^dou631@163.com^白玉^1^测试转账给窦海超"//流水号必须在提现表记录下
            });
            return Content(model);
        }

    }
}