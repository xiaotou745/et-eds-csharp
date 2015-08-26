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
        private int auditStatus = -1; //默认查询所有状态
        private int isNotRealOrder = -1;//是否无效订单，默认所有
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
        public int AuditStatus  //订单状态
        {
            get { return auditStatus; }
            set { auditStatus = value; }
        }   
        public string orderPubStart { get; set; }
        public string orderPubEnd { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }
        public string businessCity { get; set; }  //商户城市
        public string hidDaochu { get; set; }  //是否导出数据
        /// <summary>
        /// 物流公司ID
        /// </summary>
        public string deliveryCompany { get; set; }

        public int IsNotRealOrder
        {
            get { return isNotRealOrder; }
            set { isNotRealOrder = value; }
        }
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
