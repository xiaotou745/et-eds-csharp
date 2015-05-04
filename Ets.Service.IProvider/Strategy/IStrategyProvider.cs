using Ets.Model.DataModel.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Strategy
{
    public interface IStrategyProvider
    {
        StrategyModel GetCurrenStrategy(int businessId);
      
    }
}
