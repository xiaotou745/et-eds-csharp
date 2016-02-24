using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Business;
using Ets.Model.DataModel.Business;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.IProvider.Business
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBusinessSetpChargeChildProvider
    {

        BusinessSetpChargeChild GetDetails(int setpChargeId);
    }
}
