using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Bussiness
{
    public class BusinessCommissionSearchCriteria : ListParaBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string T1
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string T2
        {
            get;
            set;
        }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商户电话
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 商户城市
        /// </summary>
        public string BusinessCity { get; set; }
    }
}
