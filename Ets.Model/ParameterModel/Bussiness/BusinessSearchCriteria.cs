using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    public class BusinessSearchCriteria
    {
        //public PagingResult PagingRequest { get; set; }
        public NewPagingResult PagingRequest { get; set; }
        
        public IList<OrderByItem<BusListResultModel>> OrderByItems { get; set; }

        public string businessName { get; set; } //商户名称
        public string businessPhone { get; set; } //商户电话
        public decimal BusinessCommission { get; set; } //商户结算比例
        public int Status { get; set; }      //订单状态

        /// <summary>
        /// 查询类型,按当天,本周,还是本月
        /// </summary>
        public int searchType { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 省编码
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 市编码
        /// </summary>
        public string CityCode { get; set; }
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
