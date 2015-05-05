using Ets.Model.Common;
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

        ResultInfo<int> AddStrategy(StrategyModel config);

        /// <summary>
        /// 更新策略名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultInfo<bool> UpdateStrategyName(StrategyModel model);

        IList<StrategyModel> GetStrategyList();

        ResultInfo<bool> HasExistsStrategy(StrategyModel model);        
    }
}
