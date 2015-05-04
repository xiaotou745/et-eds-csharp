using System.Collections.Generic;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DataModel.Authority;

namespace Ets.Service.IProvider.Clienter
{
    public interface IClienterCrossShopLogProvider
    {
        bool InsertDataClienterCrossShopLog(int daysAgo);
        IList<BusinessesDistributionModel> GetClienterCrossShopLogInfo(int daysAgo);
    }
}
