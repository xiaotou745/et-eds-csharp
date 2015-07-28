using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.YeePay;
using Ets.Model.ParameterModel.YeePay;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.YeePay
{
    public interface IYeePayRunningProvider
    {
        PageInfo<YeePayRunningAccountModel> Get(YeePayRunningCriteria yeePayRunningCriteria);

        IList<YeePayRunningAccountModel> GetYeePayRunningAccountList();

        ResultModel<object> Add(YeePayRunningAccountModel yeePayRunningAccountModel);
    }
}
