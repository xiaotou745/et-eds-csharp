using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class BusinessBalanceRecordSerchCriteria:BusinessBalanceRecord
    {
        /// <summary>
        /// 操作时间（开始）
        /// </summary>
        public string OperateTimeStart { get; set; }
        /// <summary>
        /// 操作时间（结束）
        /// </summary>
        public string OperateTimeEnd { get; set; }
    }
}
