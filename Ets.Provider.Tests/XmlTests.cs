﻿using System.Collections.Generic;
using Common.Logging;
using Ets.Model.DataModel.Finance;
using ETS.Util;
using Letao.Util;
using NUnit.Framework;
using Ets.Dao.User;
using Ets.Service.Provider.MyPush;

namespace Ets.Provider.Tests
{
    [TestFixture]
    public class XmlTests
    {
        private ILog logger = LogManager.GetCurrentClassLogger();

        private List<BankModel> lstBanks = new List<BankModel>()
        {
            new BankModel() {Id = 1, Name = "交通银行"},
            new BankModel() {Id = 2, Name = "工商银行"},
        };

        [Test]
        public void SerializeTest()
        {
            XmlHelper.ToXml("bank.config", lstBanks);
        }

        [Test]
        public void DeSerializeTest()
        {
            var bankModels = XmlHelper.ToObject<List<BankModel>>("bank.config");

            logger.Info(JsonHelper.ToJson(bankModels));
        }

        [Test]
        public void UnityTest()
        {

        }

        public void businessTest()
        {
            //Ets.Model.DomainModel.Bussiness.BusinessDM model = new BusinessDao().GetByOrderId(1424211);
        }



    }
}