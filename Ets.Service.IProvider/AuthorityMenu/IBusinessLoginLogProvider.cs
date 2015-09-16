using Ets.Model.Common;
using Ets.Model.DomainModel.Authority;
namespace Ets.Service.IProvider.AuthorityMenu
{
    public interface IBusinessLoginLogProvider
    {
        long Create(BusinessLoginLogDM businessLoginLogDM);
    }
}
