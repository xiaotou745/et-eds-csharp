﻿using Ets.Model.Common;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Pay;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.MyPush;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Pay;
using Ets.Service.IProvider.Order;
using ETS.Security;
using ETS.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.YeePay;

namespace Ets.Provider.Tests
{
    [TestFixture]
    public class ClienterTests
    {
        //IPayProvider payProvider = new PayProvider();
        [Test]
        public void ClienterTest()
        {
            //PayProvider p = new PayProvider();
            //var reg = p.QueryBalanceYee(new YeeQueryBalanceParameter() { Ledgerno = "10012628673" });


            var iPayProvider = new PayProvider();
            iPayProvider.YeePayReconciliation();


            //ClienterProvider order = new ClienterProvider();

            //order.GetDetails(6381);



            //string strdes = "1851867832736B8653D62598A052A7B0CDCA1C3DCDD08C960E28E6FD9A85175D52E0E51C81D";
            //string key = "np!u5chin@adm!n1aaaaaaaa";
            //string ss = DES.Encrypt(strdes);

            ////string s = DES.Encrypt3DES(strdes);
            //string s = "/lfIemeoz9p2kCEbvSXpdghyCfdAFH57YKA0xAlX+ju1Gf5f5FxvSUEayTDCfx0Hjej3P2/dxjmeHstRunZchgw2TiFWxZjOx/HYB0beMkY=";
            ////string strdecode= DES.Encrypt(s);
            //string strdecode = DES.Decrypt(s);
            //Console.WriteLine();//651554c5
            //string sss = DESAPP.DES3Decrypt(ss, key);
            //string ss = "{\"phoneNo\":\"15801495169\",\"passWord\":\"36B8653D62598A052A7B0CDCA1C3DCDD\",\"ssid\":\"81519FDC-71AD-4851-87C8-822875C73B31\"}";
            //string pwd = "np!u5chin@adm!n1aaaaaaaa";
            //string vi = "np!u5chin@adm!n1aaaaaaaa";
            //string ess = DESAPP.Encrypt_AES(ss);
            //string dss = DESAPP.Decrypt_AES(ess);
            //string ess = AESHelper.AESEncrypt(ss, pwd);
            //string dss = AESHelper.AESDecrypt(ess, pwd);
            //AESApp.CheckAES(ss,"asd");
            //string ess = AESApp.AesEncrypt(ss);
            //string dss = AESApp.AesDecrypt(ss);
            //string s = MD5Helper.MD5("123456");
            //Push.PushMessage(new JPushModel()
            //{
            //    Title = "订单提醒",
            //    Alert = "您有新订单了，请点击查看！",
            //    RegistrationId = "C_3237",
            //    TagId = 0,
            //    PushType = 1
            //});

            //var model=new ClienterProvider().GetDetails(3120);
            //OrderProvider orderProvider=new OrderProvider();
            //orderProvider.AutoPushOrder();
        }

    }
}
