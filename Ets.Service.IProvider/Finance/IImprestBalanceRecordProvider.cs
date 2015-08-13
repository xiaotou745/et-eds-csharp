using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    /// 备用金操作记录  add by caoheyang 20150812
    /// </summary>
    public interface IImprestBalanceRecordProvider
    {
        /// <summary>
        /// 查询备用金流水列表  add by 彭宜  20150812
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ImprestBalanceRecord> GetImprestBalanceRecordList(ImprestBalanceRecordSearchCriteria criteria);
    }
}
