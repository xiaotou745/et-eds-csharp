using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;

namespace SuperManWebApi.Controllers
{ 
    /// <summary>
    /// 商户相关接口 add by caoheyang
    /// </summary>
    public class BusinessController : ApiController
    {
        private  readonly  IBusinessFinanceProvider _businessFinanceProvider=new BusinessFinanceProvider();

        /// <summary>
        /// 商户交易流水API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<BusinessBalanceRecord> Records()
        {
            int businessId = ParseHelper.ToInt(HttpContext.Current.Request.Form["businessId"]);
            return _businessFinanceProvider.GetRecords(businessId);
        }
    }
}
