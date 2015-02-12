using SuperManCore.Paging;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Entities
{
    /// <summary>
    /// 页面的查询参数
    /// </summary>
    public class BusinessSearchCriteria
    {        
        public PagingResult PagingRequest { get; set; }
        public IList<OrderByItem<business>> OrderByItems { get; set; }

        public string businessName { get; set; } //商户名称
        public string businessPhone { get; set; } //商户电话
        public int Status { get; set; }      //订单状态

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
