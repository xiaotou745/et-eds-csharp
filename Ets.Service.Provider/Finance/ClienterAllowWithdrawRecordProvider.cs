using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;

namespace Ets.Service.Provider.Finance
{
    /// <summary>
    /// Service类ClienterAllowWithdrawRecordService 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-03 15:34:50
    /// </summary>

    public class ClienterAllowWithdrawRecordProvider : IClienterAllowWithdrawRecordProvider
    {
        private readonly ClienterAllowWithdrawRecordDao clienterAllowWithdrawRecordDao = new ClienterAllowWithdrawRecordDao();
        public ClienterAllowWithdrawRecordProvider()
        {
        }
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        public long Create(ClienterAllowWithdrawRecord clienterAllowWithdrawRecord)
        {
            return clienterAllowWithdrawRecordDao.Insert(clienterAllowWithdrawRecord);
        }

    }
}
