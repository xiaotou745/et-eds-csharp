using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ets.Model.Common;
using OpenApi;

namespace OpenApi.Controllers
{

    /// <summary>
    /// 淘宝处理器  
    /// caoheyang 2015118
    /// </summary>
    [OpenApiActionError] 
    public class TaoBaoController : ApiController
    {
        /// <summary>
        /// 淘宝取消订单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [HttpPost]
        public ResultModel<object> CaonelOrder(string data)
        {
            return null;
        }
    }
}
