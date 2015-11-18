using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
using NUnit.Framework;

namespace Ets.Provider.Tests
{
    [TestFixture]
    public class TaoDianDianTest
    {
        [Test]
        public void Receive_C()
        {
            OrderReceiveModel model = new OrderReceiveModel()
            {
                businessId = 260,
                orderNo = "260151117130628029",
                version = "2.0.2",
                orderId = 109,
                IsTimely = 1,
                Longitude = 132.22222222F,
                Latitude = 32.22222F,
                ClienterId=119
            };
            new ClienterProvider().Receive_C(model);
        }


        [Test]
        public void Complete()
        {
            OrderCompleteModel model = new OrderCompleteModel();
            new ClienterProvider().FinishOrder(model);
        }


        [Test]
        public void ConfirmTake()
        {
            OrderPM pm = new OrderPM();
            new OrderProvider().UpdateTake(pm);
        }



    }
}
