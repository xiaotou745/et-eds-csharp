using Ets.Dao.Strategy;
using Ets.Model.Common;
using Ets.Model.DataModel.Strategy;
using ETS.Util;
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

        public ResultInfo<int> AddStrategy(StrategyModel config)
        {
            var result = new ResultInfo<int> { Message = "", Result = false, Data = 0 };
            try
            {
                result.Data = strategyDao.InsertDataStrategy(config);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        public ResultInfo<bool> UpdateStrategyName(StrategyModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = strategyDao.UpdateStrategyName(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }   

        public IList<StrategyModel> GetStrategyList()
        {
            return strategyDao.GetStrategyList();
        }     
     
        public ResultInfo<bool> HasExistsStrategy(StrategyModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = strategyDao.HasExistsStrategy(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
    }
}
