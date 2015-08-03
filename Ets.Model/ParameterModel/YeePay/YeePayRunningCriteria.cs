using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.YeePay
{
    public class YeePayRunningCriteria : ListParaBase
    {
        /// <summary>
        /// 充值开始时间,财务实际充值易宝账户的时间
        /// </summary>
        public string RechargeStarteTime { get; set; }
        /// <summary>
        /// 充值结束时间
        /// </summary>
        public string RechargeEndTime { get; set; }
        /// <summary>
        /// 录入开始时间,财务录入系统的时间
        /// </summary>
        public string OptStarteTime { get; set; }
        /// <summary>
        /// 录入结束时间
        /// </summary>
        public string OptEndTime { get; set; }
        /// <summary>
        /// 数据有效
        /// </summary>
        public int IsEnable { get; set; }
    }
}
