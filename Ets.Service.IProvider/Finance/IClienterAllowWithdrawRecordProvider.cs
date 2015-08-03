#region
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Collections.Generic;
#endregion

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    /// 业务领域对象类IClienterAllowWithdrawRecordRepos 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-03 15:34:50
    /// </summary>


    public interface IClienterAllowWithdrawRecordProvider
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        long Create(ClienterAllowWithdrawRecord clienterAllowWithdrawRecord);

    }

}
