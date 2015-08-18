using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain; 

namespace Ets.Service.IProvider.Complain
{
    public interface IComplainProvider
    {
        ResultModel<object> Complain(ComplainModel ccb);
    }
}
