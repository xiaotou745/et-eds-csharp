using Ets.Dao.Strategy;
using Ets.Model.DataModel.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Strategy
{
    public class StrategyProvider : IStrategyProvider
    {
        StrategyDao strategyDao = new StrategyDao();
        public StrategyModel GetCurrenStrategy(int businessId)
        {
            return strategyDao.GetCurrenStrategy(businessId);
        }
    }
}
