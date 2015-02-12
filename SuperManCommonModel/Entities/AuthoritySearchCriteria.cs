using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Entities
{
    public class AuthoritySearchCriteria
    {
        public PagingResult PagingRequest { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
    }
}
