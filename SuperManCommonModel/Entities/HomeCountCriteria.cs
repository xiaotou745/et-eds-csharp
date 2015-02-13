using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Entities
{
    public class HomeCountCriteria
    {
        public PagingResult PagingRequest { get; set; }

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
