using System.Collections.Generic;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;

namespace Ets.Service.IProvider.Clienter
{
    public interface IClienterProvider
    {
        List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria);

        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        ETS.Enums.ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel);
    }
}
