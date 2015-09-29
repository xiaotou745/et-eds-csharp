using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.AliPay
{
    /// <summary>
    /// 支付宝转账请求参数
    /// danny-20150914
    /// </summary>
    public class AlipayTransferParameter
    {

        /// <summary>
        /// 合作身份者ID(签约的支付宝账号对应的支付宝唯一用户号。以2088开头的16位纯数字组成。)
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 参数编码字符集(商户网站使用的编码格式，如utf-8、gbk、gb2312等。)
        /// </summary>
        public string InputCharset { get; set; }

        /// <summary>
        /// 服务器异步通知页面路径(需http://格式的完整路径，不允许加?id=123这类自定义参数)
        /// </summary>
        public string NotifyUrl { get; set; }
        /// <summary>
        /// 付款账号(必填)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 付款账户名(必填，个人支付宝账号是真实姓名公司支付宝账号是公司名称)
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 付款当天日期(必填，格式：年[4位]月[2位]日[2位]，如：20100801)
        /// </summary>
        public string PayDate { get; set; }
        /// <summary>
        /// 批次号(必填，格式：当天日期[8位]+序列号[3至16位]，如：201008010000001)
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 付款总金额(必填，即参数detail_data的值中所有金额的总和)
        /// </summary>
        public string BatchFee { get; set; }
        /// <summary>
        /// 付款笔数(必填，即参数detail_data的值中，“|”字符出现的数量加1，最大支持1000笔（即“|”字符出现的数量999个）)
        /// </summary>
        public string BatchNum { get; set; }
        /// <summary>
        /// 付款详细数据(必填，格式：流水号1^收款方帐号1^真实姓名^付款金额1^备注说明1|流水号2^收款方帐号2^真实姓名^付款金额2^备注说明2....)
        /// </summary>
        public string DetailData { get; set; }
    }
}
