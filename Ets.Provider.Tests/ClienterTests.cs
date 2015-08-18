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
            //string s = DES.Encrypt3DES(strdes);
            string s = "2YtqwPeCw15xxIHhs4Zu+9TjY3UR55Q0ZHkrvneEOnjO59tnaqz2S2HYGJgR186hJPOBI3HF1riH6wBQH7zntRecWq/RRAt8vl7hF+Y5QD8=";
            string strdecode= DES.Decrypt3DES(s);

            

        }

    }
}
