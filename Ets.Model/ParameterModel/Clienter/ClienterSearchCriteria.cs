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
    public class ClienterSearchCriteria
    {

        private int pageindex = 1; //默认第一页
        private int pagesize=20; //默认每页20
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
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }


        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set {  pagesize=value; }
        }

    }
}
