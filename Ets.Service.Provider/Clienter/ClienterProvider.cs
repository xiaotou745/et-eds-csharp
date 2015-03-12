using System.Collections.Generic;
using Ets.Dao.Clienter;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.Clienter;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider:IClienterProvider
    {
        readonly ClienterDao clienterDao=new ClienterDao();

        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            return clienterDao.GetOrdersNoLoginLatest(criteria);
        }

        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        public int ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel) {
            return clienterDao.ChangeWorkStatusToSql(paraModel);
        }
    }
}
