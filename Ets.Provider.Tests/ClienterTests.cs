using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
using ETS.Security;
using ETS.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Provider.Tests
{
    [TestFixture]
    public class ClienterTests
    {
        [Test]
        public void ClienterTest()
        {
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
            string ss = "易代送";
            string pwd = "np!u5chin@adm!n1aaaaaaaa";
            string vi = "np!u5chin@adm!n1aaaaaaaa";
            //string ess = DESAPP.Encrypt_AES(ss);
            //string dss = DESAPP.Decrypt_AES(ess);
            //string ess = AESHelper.AESEncrypt(ss, pwd);
            //string dss = AESHelper.AESDecrypt(ess, pwd);
            //string ess = Security.AesEncrypt(ss);
            //string dss = Security.AesDecrypt(ess);
            string s = MD5Helper.MD5("123456");
        }

    }
}
