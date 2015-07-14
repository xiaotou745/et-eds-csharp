using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using Ets.Model.DataModel.Finance;
using ETS.Pay.YeePay;
using ETS.Util;
using Letao.Util;
using NUnit.Framework;
using Ets.Dao.User;
using Ets.Service.Provider.MyPush;

namespace Ets.Provider.Tests
{
    [TestFixture]
   public   class YeePayTest
    {

        /// <summary>
        /// 注册
        /// </summary>
        [Test]
        public void Register()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string re= 1 + Convert.ToInt64(ts.TotalSeconds).ToString();
            var sp = new StringBuilder();

            string requestid = re;

            string bindmobile = "18553507220";  //绑定手机

            string customertype = CustomertypeEnum.PERSON.ToString(); //注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业

            string signedname = "曹赫洋"; //签约名   商户签约名；个人，填写姓名；企业，填写企业名称。

            string linkman = "曹赫洋"; //联系人

            string idcard = "370685199110161712"; //身份证  customertype为PERSON时，必填

            string businesslicence = ""; //营业执照号 customertype为ENTERPRISE时，必填

            string legalperson = "曹赫洋";

            string minsettleamount = "0.1"; //起结金额

            string riskreserveday = "1"; //姓名  PERSON时，idcard对应的姓名； ENTERPRISE时，企业的法人姓名

            string bankaccountnumber = "6226200105376660"; //银行卡号

            string bankname = "民生银行"; //开户行

            string accountname = "曹赫洋"; //开户名

            string bankaccounttype = BankaccounttypeEnum.PrivateCash.ToString();  //银行卡类别  PrivateCash：对私 PublicCash： 对公

            string bankprovince = "北京";

            string bankcity = "北京";

            string manualsettle = "Y";

            var result1 = new Register().RegSubaccount(requestid, bindmobile, customertype, signedname, linkman,
                idcard, businesslicence, legalperson, minsettleamount, riskreserveday, bankaccountnumber, bankname,
                accountname, bankaccounttype, bankprovince, bankcity, manualsettle);//注册帐号
            //var result = Helpers<Transfer>.Instance.TransferAccounts(requestid, "10012472116", "0.1", "");//转账
            //var result = Helpers<QueryBalance>.Instance.GetBalance("");//账户余额
            var result = new Transfer().CashTransfer(requestid, "", "0.1", "");//提现

        }

        /// <summary>
        /// 查询余额
        /// </summary>
        [Test]
        public void CashTransfer()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string re = 1 + Convert.ToInt64(ts.TotalSeconds).ToString();
            var result = new Transfer().CashTransfer(re, "10012474271", "1.1", "");//提现

        }


        /// <summary>
        /// 查询余额
        /// </summary>
        [Test]
        public void GetBalance()
        {
            var result = new QueryBalance().GetBalance("10012474271");//账户余额
        }


        /// <summary>
        /// 转账
        /// </summary>

        [Test]
        public void TransferAccounts()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string re = 1 + Convert.ToInt64(ts.TotalSeconds).ToString();
            var result = new Transfer().TransferAccounts(re, "10012474271", "1.1", "");//转账   主账户转给子账户
            //var result1 = new Transfer().TransferAccounts(re, "", "0.1", "10012474239");//转账   子账户转给总账户
        }
     
    }
}
