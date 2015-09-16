using Ets.Model.Common;
using Ets.Model.DomainModel.Authority;
using Ets.Service.IProvider.AuthorityMenu;
using Ets.Dao.Authority;
namespace Ets.Service.Provider.Authority
{
    public class ClienterLoginLogProvider : IClienterLoginLogProvider
    {
        ClienterLoginLogDao clienterLoginLogDao = new ClienterLoginLogDao();
        public long Create(ClienterLoginLogDM clienterLoginLogDM)
        {
            return clienterLoginLogDao.Insert(clienterLoginLogDM);
        }
    }
}
