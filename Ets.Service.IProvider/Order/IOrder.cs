using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Order
{
    interface  IOrder
    {
        PagedList<order> GetOrders(ClientOrderSearchCriteria criteria);
    }
}
