using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Entities
{
    public class MyIncomeSearchCriteria
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNo { get; set; }
        public PagingResult PagingRequest { get; set; }
    }
}
