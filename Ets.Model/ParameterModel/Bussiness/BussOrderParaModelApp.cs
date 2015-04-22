using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// B端获取订单app查询条件实体 add by caoheyang 20140311
    /// </summary>
    public class BussOrderParaModelApp
    {
        public PagingResult PagingResult { get; set; }
        /// <summary>
        /// 商户id
        /// </summary>
        public int? userId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int? Status { get; set; }
        
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }
        
    }
}