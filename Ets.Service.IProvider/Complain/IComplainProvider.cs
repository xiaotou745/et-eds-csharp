using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain;
using Ets.Model.DomainModel.Complain;
using Ets.Model.ParameterModel.Complain;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.Complain
{
    public interface IComplainProvider
    {
        ResultModel<object> Complain(ComplainModel ccb);

        PageInfo<ComplainDomain> Get(ComplainCriteria complainCriteria); 
    }
}
