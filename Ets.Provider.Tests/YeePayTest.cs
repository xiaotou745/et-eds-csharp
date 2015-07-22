using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
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
            var result1 = new PayProvider().RegisterYee(new YeeRegisterParameter()
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
                BankCity = "北京",
                UserId=1,
                UserType=0
            });//注册帐号
        }

        /// <summary>
        /// 提现
        /// </summary>
        [Test]
        public void CashTransfer()
        {
            var result = new Transfer().CashTransfer(APP.B, 97, "10012474356", "3");//提现
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
