using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;

namespace Ets.Service.Provider.OpenApi
{
    public class DefaultGroup : IGroupProviderOpenApi
    {
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            throw new NotImplementedException();
        }
    }
}
