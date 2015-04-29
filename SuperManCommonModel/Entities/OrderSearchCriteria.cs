using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SuperManDataAccess;
namespace SuperManCommonModel.Entities
{
    /// <summary>
    /// 页面的查询参数
    /// </summary>
    public class OrderSearchCriteria
    {
        public PagingResult PagingRequest { get; set; }
        public IList<OrderByItem<order>> OrderByItems { get; set; }

        public string LoginName { get; set; }
        public string superManPhone { get; set; } //超人电话
        public string businessPhone { get; set; } //商家电话
        public string orderId { get; set; }          //订单号
        public string OriginalOrderNo { get; set; } //原平台订单号
        public string superManName { get; set; }  //超人姓名
        public string businessName { get; set; }  //商户名称
        public int orderStatus { get; set; }      //订单状态
        public string orderPubStart { get; set; }
        public string orderPubEnd { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
        public int LoginId { get; set; } //登录用户Id
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
