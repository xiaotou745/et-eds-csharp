using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 骑士相关接口 add by caoheyang
    /// </summary>
    public class ClienterController : ApiController
    {
        IClienterFinanceProvider _iClienterFinanceProvider = new ClienterFinanceProvider();
        /// <summary>
        /// 骑士交易流水API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IList<ClienterBalanceRecord> Records()
        {
            int clineterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["clineterId"]);
            return _iClienterFinanceProvider.GetRecords(clineterId);
        }
    }
}
