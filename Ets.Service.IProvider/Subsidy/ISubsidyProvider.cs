using Ets.Model.DomainModel.Subsidy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Subsidy
{
    public interface ISubsidyProvider
    {
        SubsidyResultModel GetCurrentSubsidy(int groupId = 0);
    }
}
