using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using ETS;
using Ets.Model.DataModel.Finance;
using ETS.Pay.YeePay;
using Ets.Service.Provider.Pay;
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
        public void Register1()
        {
            string requestid = string.Concat(APP.B.ToString(), "-t", "2313123", "-", Config.WithdrawType);// +"-" + TimeHelper.GetTimeStamp(false);

            //var result1 = new PayProvider().RegisterYee(new YeeRegisterParameter()
            //{
            //    BindMobile = "18553507220",
            //    CustomerType=CustomertypeEnum.ENTERPRISE,
            //    SignedName = "曹赫洋",
            //    LinkMan = "曹赫洋",
            //    IdCard = "",
            //    BusinessLicence="2323232323232",
            //    LegalPerson = "曹赫洋",
            //    BankAccountNumber = "6226200105376660",
            //    BankName = "民生银行",
            //    AccountName = "曹赫洋",
            //    BankProvince = "北京",
            //    BankCity = "北京",
            //    UserId=1,
            //    UserType=UserTypeYee.Business.GetHashCode()
            //});//注册帐号
        }

        /// <summary>
        /// 提现
        /// </summary>
        [Test]
        public void CashTransfer()
        {
            //var result = new Transfer().CashTransfer(APP.B, -1, "10012474356", "0.1");//提现

            var result1 = new PayProvider().CashTransferYee(new YeeCashTransferParameter()
            {
                UserType = UserTypeYee.Business.GetHashCode(),
                WithdrawId = 1212,
                Ledgerno = "10012474356",
                App = APP.B,
                Amount = "0.2"
            });
        }


        /// <summary>
        /// 查询余额
        /// </summary>
        [Test]
        public void GetBalance()
        {
            var result = new PayProvider().QueryBalanceYee(new YeeQueryBalanceParameter()
            {
                Ledgerno = "10012474356"
            });//账户余额
        }


        /// <summary>
        /// 转账
        /// </summary>

        [Test]
        public void TransferAccounts()
        {
            var model= new PayProvider().TransferAccountsYee(new YeeTransferParameter()
            {
                UserType=1,
                WithdrawId=1212,
                Ledgerno = "10012474356",
                SourceLedgerno = "",
                Amount="2"
            });
        }
     
    }
}
