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

            string requestid = TimeHelper.GetTimeStamp(false);             

            string bindmobile = "18553507220";  //绑定手机

            string customertype = CustomertypeEnum.PERSON.ToString(); //注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业

            string signedname = "曹赫洋"; //签约名   商户签约名；个人，填写姓名；企业，填写企业名称。

            string linkman = "曹赫洋"; //联系人

            string idcard = "370685199110161712"; //身份证  customertype为PERSON时，必填

            string businesslicence = ""; //营业执照号 customertype为ENTERPRISE时，必填

            string legalperson = "曹赫洋";

            string bankaccountnumber = "6226200105376660"; //银行卡号           交通银行  6222620910009103866

            string bankname = "民生银行"; //开户行

            string accountname = "曹赫洋"; //开户名

            string bankaccounttype = BankaccounttypeEnum.PrivateCash.ToString();  //银行卡类别  PrivateCash：对私 PublicCash： 对公

            string bankprovince = "北京";

            string bankcity = "北京";

            var result1 = new Register().RegSubaccount(requestid, bindmobile, customertype, signedname, linkman,
                idcard, businesslicence, legalperson,  bankaccountnumber, bankname,
                accountname, bankaccounttype, bankprovince, bankcity);//注册帐号
        }


        /// <summary>
        /// 注册
        /// </summary>
        [Test]
        public void Register1()
        {

            string requestid = TimeHelper.GetTimeStamp(false);

            string bindmobile = "18553507220";  //绑定手机

            string customertype = CustomertypeEnum.PERSON.ToString(); //注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业

            string signedname = "曹赫洋"; //签约名   商户签约名；个人，填写姓名；企业，填写企业名称。

            string linkman = "曹赫洋"; //联系人

            string idcard = "370685199110161712"; //身份证  customertype为PERSON时，必填

            string businesslicence = ""; //营业执照号 customertype为ENTERPRISE时，必填

            string legalperson = "曹赫洋";

            string bankaccountnumber = "6226200105376660"; //银行卡号           交通银行  6222620910009103866

            string bankname = "民生银行"; //开户行

            string accountname = "曹赫洋"; //开户名

            string bankaccounttype = BankaccounttypeEnum.PrivateCash.ToString();  //银行卡类别  PrivateCash：对私 PublicCash： 对公

            string bankprovince = "北京";

            string bankcity = "北京";

            var result1 = new Register().RegSubaccount(new YeeRegisterParameter()
            {
                BindMobile = "18553507220",
                CustomerType=CustomertypeEnum.PERSON,
                SignedName = "曹赫洋",
                LinkMan = "曹赫洋",
                IdCard = "370685199110161712",
                BusinessLicence="",
                LegalPerson = "曹赫洋",
                BankAccountNumber = "6226200105376660",
                BankName = "民生银行",
                AccountName = "曹赫洋",
                BankProvince = "北京",
                BankCity = "北京"
            });//注册帐号
        }

        /// <summary>
        /// 提现
        /// </summary>
        [Test]
        public void CashTransfer()
        {
            var result = new Transfer().CashTransfer(APP.B, 97, "10012474356", "3", "http://edstest130.yitaoyun.net/pay/YeePayCashTransferCallback");//提现
        }


        /// <summary>
        /// 查询余额
        /// </summary>
        [Test]
        public void GetBalance()
        {
            var result = new QueryBalance().GetBalance("10012474356");//账户余额
        }


        /// <summary>
        /// 转账
        /// </summary>

        [Test]
        public void TransferAccounts()
        {
            var result = new Transfer().TransferAccounts("10012474356", "5", "");//转账   主账户转给子账户
            //var result1 = new Transfer().TransferAccounts( "", "10", "10012474356");//转账   子账户转给总账户
        }
     
    }
}
