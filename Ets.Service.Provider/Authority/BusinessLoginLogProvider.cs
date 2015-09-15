using Ets.Model.Common;
using Ets.Model.DomainModel.Authority;
using Ets.Service.IProvider.AuthorityMenu;
using Ets.Dao.Authority;
namespace Ets.Service.Provider.Authority
{
    public class BusinessLoginLogProvider: IBusinessLoginLogProvider
    {

        BusinessLoginLogDao businessLoginLogDao = new BusinessLoginLogDao();
        public long Create(BusinessLoginLogDM businessLoginLogDM)
        {
            return businessLoginLogDao.Insert(businessLoginLogDM);
        }
    }
}
