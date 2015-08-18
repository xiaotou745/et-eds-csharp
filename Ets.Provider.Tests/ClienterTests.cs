using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
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
            ClienterProvider order = new ClienterProvider();
          
            order.GetDetails(6381);
        }

    }
}
