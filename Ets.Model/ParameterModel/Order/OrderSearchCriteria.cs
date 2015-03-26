using Ets.Model.Common;
using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class OrderSearchCriteria : ListParaBase
    {
        private int status = -1; //默认查询所有状态
        public NewPagingResult PagingRequest { get; set; }
        public IList<OrderByItem<order>> OrderByItems { get; set; }


        public string superManPhone { get; set; } //超人电话
        public string businessPhone { get; set; } //商家电话
        public string orderId { get; set; }          //订单号
        public string OriginalOrderNo { get; set; } //原平台订单号
        public string superManName { get; set; }  //超人姓名
        public string businessName { get; set; }  //商户名称
        public int orderStatus  //订单状态
        {
            get { return status; }
            set { status = value; }
        }   
        public string orderPubStart { get; set; }
        public string orderPubEnd { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
    }

    public class OrderByItem<Tentity>
    {
        public OrderByItem(Expression<Func<Tentity, object>> eeySelector, SortOrder direction)
        {
            EeySelector = eeySelector;
            Direction = direction;
        }
        public Expression<Func<Tentity, object>> EeySelector { get; set; }
        public SortOrder Direction { get; set; }
    }
}
