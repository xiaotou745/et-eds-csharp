using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Subsidy
{
    public class HomeCountCriteria : ListParaBase
    {
        public NewPagingResult PagingRequest { get; set; }

        /// <summary>
        /// 查询类型,按当天,本周,还是本月
        /// </summary>
        public int searchType { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
    }
}
