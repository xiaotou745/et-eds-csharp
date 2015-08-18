using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
using ETS.Security;
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

            string strdes = "eds易代送";
            string key = "np!u5chin@adm!n1aaaaaaaa";
            string ss = DESAPP.DES3Encrypt(strdes, key);

            //string s = DES.Encrypt3DES(strdes);
            string s = "yDjBQC0opeUZVFp9v90oFO8+UMq1uLfx0BhmF+clDp7cPqK2SCRFLygsY0NT9MhJkZ2PDkkxbo94Hj6z4kE99747R0Bx5pxHgz8JkbaorBs=";
            string strdecode= DES.Decrypt3DES(s);
            Console.WriteLine();//651554c5
            string sss = DESAPP.DES3Decrypt(ss, key);


        }

    }
}
