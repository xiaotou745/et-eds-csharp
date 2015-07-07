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

        public int PageIndex { get; set; }
    }
}
