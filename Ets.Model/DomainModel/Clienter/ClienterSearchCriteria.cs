using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Clienter
{
    public class ClienterSearchCriteria
    {
        public NewPagingResult PagingRequest { get; set; }
        public IList<OrderByItem<ClienterModel>> OrderByItems { get; set; }

        public string clienterName { get; set; } //商户名称
        public string clienterPhone { get; set; } //商户电话
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
