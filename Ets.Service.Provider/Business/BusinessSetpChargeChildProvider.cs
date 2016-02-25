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
using Ets.Dao.Business;
using Ets.Service.IProvider.Business;
namespace Ets.Service.Provider.Business
{
    /// <summary>
    ///
    /// </summary>
    public class BusinessSetpChargeChildProvider : IBusinessSetpChargeChildProvider
    {
        readonly BusinessSetpChargeChildDao businessSetpChargeChildDao = new BusinessSetpChargeChildDao();

        public BusinessSetpChargeChild GetDetails(int setpChargeId)
        {
            return businessSetpChargeChildDao.GetDetails(setpChargeId);
        }
    }
}
