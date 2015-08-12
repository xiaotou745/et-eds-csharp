using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 备用金流水查询参数  add by 彭宜  20150812
    /// </summary>
    public class ImprestBalanceRecordSearchCriteria : ListParaBase
    {
        /// <summary>
        /// 类型：1、充值、2、骑士支出
        /// </summary>
        public int OptType { get; set; }
        /// <summary>
        /// 骑士电话,如果是查充值记录,不需要传递此字段
        /// </summary>
        public string ClienterPhoneNo { get; set; }
        /// <summary>
        /// 操作时间起
        /// </summary>
        public string OptDateStart { get; set; }
        /// <summary>
        /// 操作时间止
        /// </summary>
        public string OptDateEnd { get; set; }
    }
}
