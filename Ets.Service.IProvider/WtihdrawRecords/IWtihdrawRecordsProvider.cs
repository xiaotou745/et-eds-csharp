using Ets.Model.Common;
using Ets.Model.ParameterModel.WtihdrawRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.WtihdrawRecords
{
    public interface IWtihdrawRecordsProvider
    {
        /// <summary>
        /// 提现
        /// 窦海超
        /// 2015年3月23日 12:54:43
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddWtihdrawRecords(WithdrawRecordsModel model);
        /// <summary>
        /// 获取我的余额
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        NewPagedList<IncomeModel> GetMyIncomeList(MyIncomeSearchCriteria criteria);

        /// <summary>
        /// 增加一条流水记录
        /// 平扬 
        /// 2015.3.23
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddRecords(WithdrawRecordsModel model); 
    }
}
