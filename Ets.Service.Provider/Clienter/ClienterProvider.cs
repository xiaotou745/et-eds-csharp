using System.Collections.Generic;
using Ets.Dao.Clienter;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider
    {
        readonly ClienterDao clienterDao=new ClienterDao();

        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            return clienterDao.GetOrdersNoLoginLatest(criteria);
        }
    }
}
