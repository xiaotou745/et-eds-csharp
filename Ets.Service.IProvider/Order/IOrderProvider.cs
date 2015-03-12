using Ets.Model.DataModel.Clienter;
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
    public interface IOrderProvider
    {
        PagedList<order> GetOrders(ClientOrderSearchCriteria criteria);
    }
}
