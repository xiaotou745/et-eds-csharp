using Ets.Model.Common;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// 管理后台 超人管理列表页查询条件实体
    /// </summary>
    public class ClienterSearchCriteria:ListParaBase
    {
        private int status = -1; //默认查询所有状态
        public NewPagingResult PagingRequest { get; set; }
        public IList<OrderByItem<ClienterModel>> OrderByItems { get; set; }

        public string clienterName { get; set; } //商户名称
        public string clienterPhone { get; set; } //商户电话
        public int Status
        {
            get { return status; }
            set { status = value; }
        }      //订单状态

        /// <summary>
        /// 查询类型,按当天,本周,还是本月
        /// </summary>
        public int searchType { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
        /// <summary>
        /// 商户城市
        /// </summary>
        public string businessCity { get; set; }
        /// <summary>
        /// 推荐人电话
        /// </summary>
        public string recommonPhone { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string txtPubStart { get; set; }
        
    }
}
