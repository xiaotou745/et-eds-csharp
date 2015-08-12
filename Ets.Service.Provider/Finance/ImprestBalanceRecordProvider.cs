using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;

namespace Ets.Service.Provider.Finance
{ 
    /// <summary>
    /// 备用金操作记录  add by caoheyang 20150812
    /// </summary>
    public class ImprestBalanceRecordProvider : IImprestBalanceRecordProvider
    {
        private readonly ImprestBalanceRecordDao _imprestBalanceRecordDao = new ImprestBalanceRecordDao();

        /// <summary>
        /// 查询备用金流水列表  add by 彭宜  20150812
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ETS.Data.PageData.PageInfo<ImprestBalanceRecordModel> GetImprestBalanceRecordList(ImprestBalanceRecordSearchCriteria criteria)
        {
            return _imprestBalanceRecordDao.GetImprestBalanceRecordList(criteria);
        }
    }
}
