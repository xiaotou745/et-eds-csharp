using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
    /// <summary>
    /// 推荐统计参数
    /// </summary>
    public class RecommendQuery
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RecommendPhone { get; set; }
       /// <summary>
       /// 1 商家 2 骑士
       /// </summary>
        public int DataType { get; set; }
        public int PageIndex { get; set; }
        /// <summary>
        /// 查询手机号
        /// </summary>
        public string QueryPhone { get; set; }
    }
}
