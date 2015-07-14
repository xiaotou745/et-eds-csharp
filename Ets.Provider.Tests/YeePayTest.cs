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

        [Test]
        public void Register()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string re= 1 + Convert.ToInt64(ts.TotalSeconds).ToString();
            var sp = new StringBuilder();

            string requestid = re;

            string bindmobile = "18513958521";

            string customertype = "PERSON";

            string signedname = "林春晓";

            string linkman = "林春晓";

            string idcard = "130924198603073514";

            string businesslicence = "";

            string legalperson = "林春晓";

            string minsettleamount = "0.1";

            string riskreserveday = "1";

            string bankaccountnumber = "6225880123108731";

            string bankname = "招商银行股份有限公司北京大运村支行";

            string accountname = "林春晓";

            string bankaccounttype = "PrivateCash";

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
    }
}
