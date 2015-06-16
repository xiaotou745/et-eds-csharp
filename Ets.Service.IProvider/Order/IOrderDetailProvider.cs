using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Order;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Order
{
    public interface IOrderDetailProvider
    {
        List<OrderDetailInfo> GetByOrderNo(string orderNo);
    }
}
